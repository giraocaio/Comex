using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using Comex.GeneradorRelatorio;
using Comex.Menus;
using Comex.Servicos;

namespace Comex.Testes;

public static class TestesSemana5
{
    private const string JsonProdutos = """
        [
          {"id":1,"title":"Grátis","price":0},
          {"id":2,"title":"Livro","price":10,"description":"Edição; especial"},
          {"id":3,"title":"Monitor","price":100},
          {"id":4,"title":"Mouse","price":2}
        ]
        """;

    public static async Task ExecutarAsync()
    {
        int aprovados = 0;
        async Task Verificar(string nome, Func<Task> teste)
        {
            using DiretorioTemporario pasta = new DiretorioTemporario(alterarDiretorio: true);
            await teste();
            aprovados++;
            Console.WriteLine($"OK: {nome}");
        }

        await Verificar("Resposta HTTP é persistida sem reserializar e continua desserializada", async () =>
        {
            string json = "  \n" + JsonProdutos + "\n ";
            using HttpClient cliente = ClienteJson(json);
            ProdutosApiServico servico = new ProdutosApiServico(cliente);
            var produtos = await servico.ConsultarAsync();
            Exigir(await File.ReadAllTextAsync("produtos.json") == json && produtos.Count == 4);
            Exigir(Path.IsPathFullyQualified(servico.CaminhoJson));
        });
        await Verificar("Nova consulta substitui o JSON anterior, sem concatenar", async () =>
        {
            File.WriteAllText("produtos.json", JsonProdutos);
            using HttpClient cliente = ClienteJson("[]");
            await new ProdutosApiServico(cliente).ConsultarAsync();
            Exigir(File.ReadAllText("produtos.json") == "[]");
        });
        await Verificar("JSON inválido de resposta 200 é salvo antes da desserialização", async () =>
        {
            using HttpClient cliente = ClienteJson("{json inválido");
            await EsperarExcecaoAsync<JsonException>(() => new ProdutosApiServico(cliente).ConsultarAsync());
            Exigir(File.ReadAllText("produtos.json") == "{json inválido");
        });
        await Verificar("HTTP 500 não sobrescreve o arquivo anterior", async () =>
        {
            File.WriteAllText("produtos.json", JsonProdutos);
            using HttpClient cliente = ClienteJson("erro", HttpStatusCode.InternalServerError);
            await EsperarExcecaoAsync<HttpRequestException>(() => new ProdutosApiServico(cliente).ConsultarAsync());
            Exigir(File.ReadAllText("produtos.json") == JsonProdutos);
        });
        await Verificar("Falha ao salvar JSON é tratada sem anunciar sucesso", async () =>
        {
            using HttpClient cliente = ClienteJson(JsonProdutos);
            MenuConsultaApi menu = new MenuConsultaApi(new ProdutosApiServico(cliente, "pasta-inexistente/produtos.json"));
            string saida = await CapturarAsync(menu.ConsultarAsync);
            Exigir(saida.Contains("Não foi possível gravar") && !saida.Contains("Consulta concluída"));
        });
        await Verificar("CSV retorna caminho absoluto e grava UTF-8 com cabeçalho", () =>
        {
            IGeradorRelatorio gerador = new GeradorCsv();
            string path = gerador.Exportar(new[] { new ProdutoRelatorioDto { Id = 1, Nome = "Ação", Preco = 29.90m } }, "saida.csv");
            Exigir(path == Path.GetFullPath("saida.csv") && File.Exists(path));
            Exigir(File.ReadAllText(path) == "Id;Nome;Descricao;Preco\r\n1;\"Ação\";\"\";29,90\r\n");
            Exigir(File.ReadAllBytes(path).Take(3).SequenceEqual(new byte[] { 0xef, 0xbb, 0xbf }));
            Exigir(typeof(GeradorCsv).Assembly != typeof(Menu).Assembly);
            return Task.CompletedTask;
        });
        await Verificar("CSV preserva aspas, ponto e vírgula e quebras de linha nos textos", () =>
        {
            IGeradorRelatorio gerador = new GeradorCsv();
            string path = gerador.Exportar(new[] { new ProdutoRelatorioDto
            {
                Id = 7, Nome = "Livro; \"C#\"", Descricao = "Linha 1\nLinha 2", Preco = 12.35m
            } }, "especial.csv");
            Exigir(File.ReadAllText(path).Contains("7;\"Livro; \"\"C#\"\"\";\"Linha 1\nLinha 2\";12,35\r\n"));
            return Task.CompletedTask;
        });
        await Verificar("CSV mantém formato numérico pt-BR em um sistema en-US", () =>
        {
            CultureInfo anterior = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                new GeradorCsv().Exportar(new[] { new ProdutoRelatorioDto { Preco = 1234.56m } }, "cultura.csv");
                Exigir(File.ReadAllText("cultura.csv").Contains(";1234,56\r\n"));
            }
            finally { CultureInfo.CurrentCulture = anterior; }
            return Task.CompletedTask;
        });
        await Verificar("CSV protege texto que poderia ser interpretado como fórmula", () =>
        {
            new GeradorCsv().Exportar(new[] { new ProdutoRelatorioDto { Nome = "=1+1", Descricao = "@teste" } }, "formula.csv");
            Exigir(File.ReadAllText("formula.csv").Contains("\"'=1+1\";\"'@teste\""));
            return Task.CompletedTask;
        });
        await Verificar("CSV vazio substitui arquivo anterior por somente cabeçalho", () =>
        {
            File.WriteAllText("vazio.csv", "conteúdo antigo");
            new GeradorCsv().Exportar(Array.Empty<ProdutoRelatorioDto>(), "vazio.csv");
            Exigir(File.ReadAllText("vazio.csv") == "Id;Nome;Descricao;Preco\r\n");
            return Task.CompletedTask;
        });
        await Verificar("Gerador rejeita coleção nula e caminho vazio", async () =>
        {
            await EsperarExcecaoAsync<ArgumentNullException>(() => Task.FromResult(new GeradorCsv().Exportar(null!, "x.csv")));
            await EsperarExcecaoAsync<ArgumentException>(() => Task.FromResult(new GeradorCsv().Exportar(Array.Empty<ProdutoRelatorioDto>(), " ")));
            Exigir(!File.Exists("x.csv"));
        });
        await Verificar("Relatório sem JSON orienta consultar API e não cria CSV", async () =>
        {
            string saida = await CapturarAsync(new MenuGerarRelatorioCsv().ExecutarAsync);
            Exigir(saida.Contains("não encontrado") && !File.Exists("relatorio-produtos.csv"));
        });
        await Verificar("Relatório lê disco, filtra preço zero, ordena decrescente e projeta DTO", async () =>
        {
            File.WriteAllText("produtos.json", JsonProdutos);
            Menu menu = new MenuGerarRelatorioCsv();
            string saida = await CapturarAsync(menu.ExecutarAsync);
            string csv = File.ReadAllText("relatorio-produtos.csv");
            Exigir(saida.Contains("3 produto(s)") && saida.Contains(Path.GetFullPath("relatorio-produtos.csv")));
            Exigir(!csv.Contains("Grátis") && csv.Contains("Edição; especial"));
            Exigir(csv.IndexOf("Monitor", StringComparison.Ordinal) < csv.IndexOf("Livro", StringComparison.Ordinal));
            Exigir(csv.IndexOf("Livro", StringComparison.Ordinal) < csv.IndexOf("Mouse", StringComparison.Ordinal));
            Exigir(File.ReadAllText("produtos.json") == JsonProdutos);
        });
        await Verificar("Relatório sem produtos elegíveis gera cabeçalho e informa resultado", async () =>
        {
            File.WriteAllText("produtos.json", "[{\"id\":1,\"title\":\"Grátis\",\"price\":0}]");
            string saida = await CapturarAsync(new MenuGerarRelatorioCsv().ExecutarAsync);
            Exigir(saida.Contains("somente o cabeçalho") && File.ReadAllLines("relatorio-produtos.csv").Length == 1);
        });
        await Verificar("Relatório rejeita JSON inválido, nulo, item nulo e preço negativo", async () =>
        {
            File.WriteAllText("relatorio-produtos.csv", "relatório anterior");
            foreach (string json in new[] { "{", "null", "[null]", "[{\"id\":1,\"title\":\"Erro\",\"price\":-1}]" })
            {
                File.WriteAllText("produtos.json", json);
                string saida = await CapturarAsync(new MenuGerarRelatorioCsv().ExecutarAsync);
                Exigir(saida.Contains("JSON inválido") && !saida.Contains("Relatório gerado"));
                Exigir(File.ReadAllText("relatorio-produtos.csv") == "relatório anterior");
            }
        });
        await Verificar("Falha de gravação do CSV retorna mensagem de erro", async () =>
        {
            File.WriteAllText("produtos.json", JsonProdutos);
            string saida = await CapturarAsync(new MenuGerarRelatorioCsv(caminhoCsv: "inexistente/saida.csv").ExecutarAsync);
            Exigir(saida.Contains("Não foi possível ler o JSON ou gravar o CSV") && !saida.Contains("Relatório gerado"));
        });
        await Verificar("Fluxo do dicionário: consulta na opção 8, relatório na 18 e saída", async () =>
        {
            using HttpClient cliente = ClienteJson(JsonProdutos);
            Dictionary<int, Menu> menus = MenuPrincipal.CriarMenus(new ProdutosApiServico(cliente));
            menus.Add(18, new MenuGerarRelatorioCsv());
            string saida = await CapturarAsync(new MenuPrincipal(menus).ExibirMenuDeOpcoesAsync, "8\n18\n-1\n");
            Exigir(saida.Contains("JSON bruto salvo em:") && saida.Contains("Relatório gerado com 3 produto(s)."));
            Exigir(saida.Contains("COMEX FINALIZADO!") && File.Exists("relatorio-produtos.csv"));
        });
        await Verificar("Demonstração offline preserva JSON real existente", async () =>
        {
            File.WriteAllText("produtos.json", "não modificar");
            await CapturarAsync(DemonstracaoApi.ExecutarAsync);
            Exigir(File.ReadAllText("produtos.json") == "não modificar");
        });

        Console.WriteLine($"\n{aprovados} testes da Semana 5 aprovados.");
    }

    private static HttpClient ClienteJson(string json, HttpStatusCode status = HttpStatusCode.OK) =>
        new(new RespostaHttpSimulada((_, _) => Task.FromResult(RespostaHttpSimulada.CriarJson(json, status))));

    private static async Task<string> CapturarAsync(Func<Task> acao, string entrada = "")
    {
        TextReader entradaOriginal = Console.In;
        TextWriter saidaOriginal = Console.Out;
        using StringReader leitor = new StringReader(entrada);
        using StringWriter escritor = new StringWriter();
        try
        {
            Console.SetIn(leitor);
            Console.SetOut(escritor);
            await acao();
            return escritor.ToString();
        }
        finally
        {
            Console.SetIn(entradaOriginal);
            Console.SetOut(saidaOriginal);
        }
    }

    private static void Exigir(bool condicao)
    {
        if (!condicao)
            throw new InvalidOperationException("O resultado da Semana 5 não corresponde ao esperado.");
    }

    private static async Task EsperarExcecaoAsync<T>(Func<Task> acao) where T : Exception
    {
        try { await acao(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Era esperada uma exceção {typeof(T).Name}.");
    }
}
