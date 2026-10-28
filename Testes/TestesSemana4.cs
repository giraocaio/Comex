using System.Net;
using System.Text.Json;
using Comex.Menus;
using Comex.Modelos;
using Comex.Servicos;

namespace Comex.Testes;

public static class TestesSemana4
{
    public static async Task ExecutarAsync()
    {
        int aprovados = 0;
        async Task Verificar(string nome, Func<Task> teste)
        {
            await teste();
            aprovados++;
            Console.WriteLine($"OK: {nome}");
        }

        await Verificar("HttpClient envia GET para o endereço solicitado", async () =>
        {
            bool recebeu = false;
            using HttpClient cliente = new HttpClient(new RespostaHttpSimulada((pedido, _) =>
            {
                recebeu = true;
                Exigir(pedido.Method == HttpMethod.Get && pedido.RequestUri?.AbsoluteUri == ProdutosApiServico.EnderecoApi);
                return Task.FromResult(RespostaHttpSimulada.CriarJson("[]"));
            }));
            await new ProdutosApiServico(cliente).ConsultarAsync();
            Exigir(recebeu);
        });

        await Verificar("Desserialização mapeia campos e ignora informações extras", async () =>
        {
            using HttpClient cliente = ClienteJson(DemonstracaoApi.JsonExemplo);
            List<ProdutoApi> produtos = await new ProdutosApiServico(cliente).ConsultarAsync();
            Exigir(produtos.Count == 3 && produtos[0].Id == 1 && produtos[0].Nome == "Zebra de brinquedo" &&
                produtos[0].PrecoUnitario == 2 && produtos[1].Descricao == "Livro fictício");
        });
        await Verificar("Preço decimal preservado e descrição opcional", async () =>
        {
            using HttpClient cliente = ClienteJson("[{\"id\":1,\"title\":\"Caderno\",\"price\":19.95}]");
            ProdutoApi produto = (await new ProdutosApiServico(cliente).ConsultarAsync())[0];
            Exigir(produto.PrecoUnitario == 19.95m && produto.Descricao == "");
        });
        await Verificar("Lista vazia é uma resposta válida", async () =>
        {
            using HttpClient cliente = ClienteJson("[]");
            Exigir((await new ProdutosApiServico(cliente).ConsultarAsync()).Count == 0);
        });

        (string Nome, string Json)[] invalidos =
        {
            ("JSON malformado", "{"),
            ("Corpo vazio", ""),
            ("Raiz nula", "null"),
            ("Objeto em vez de lista", "{}"),
            ("Item nulo", "[null]"),
            ("Nome obrigatório ausente", "[{\"id\":1,\"price\":10}]"),
            ("Preço obrigatório ausente", "[{\"id\":1,\"title\":\"Livro\"}]"),
            ("Identificador obrigatório ausente", "[{\"title\":\"Livro\",\"price\":10}]"),
            ("Nome vazio", "[{\"id\":1,\"title\":\" \",\"price\":10}]"),
            ("Preço negativo", "[{\"id\":1,\"title\":\"Livro\",\"price\":-1}]"),
            ("Identificador inválido", "[{\"id\":0,\"title\":\"Livro\",\"price\":10}]"),
            ("Preço com tipo incompatível", "[{\"id\":1,\"title\":\"Livro\",\"price\":\"abc\"}]")
        };
        foreach ((string nome, string json) in invalidos)
        {
            await Verificar($"Rejeita {nome}", async () =>
            {
                using HttpClient cliente = ClienteJson(json);
                await EsperarExcecaoAsync<JsonException>(() => new ProdutosApiServico(cliente).ConsultarAsync());
            });
        }

        await Verificar("LINQ ordena nomes e desempata preços sem alterar a origem", async () =>
        {
            using HttpClient cliente = ClienteJson(DemonstracaoApi.JsonExemplo);
            List<ProdutoApi> produtos = await new ProdutosApiServico(cliente).ConsultarAsync();
            Exigir(ProdutosApiServico.OrdenarPorNome(produtos).Select(p => p.Id).SequenceEqual(new[] { 3, 2, 1 }));
            Exigir(ProdutosApiServico.OrdenarPorPreco(produtos).Select(p => p.Id).SequenceEqual(new[] { 1, 3, 2 }));
            Exigir(produtos.Select(p => p.Id).SequenceEqual(new[] { 1, 2, 3 }));
        });
        await Verificar("Ordenação numérica não confunde 2, 10 e 100", () =>
        {
            ProdutoApi[] produtos =
            {
                new() { Nome = "C", PrecoUnitario = 100 },
                new() { Nome = "B", PrecoUnitario = 10 },
                new() { Nome = "A", PrecoUnitario = 2 }
            };
            Exigir(ProdutosApiServico.OrdenarPorPreco(produtos).Select(p => p.PrecoUnitario).SequenceEqual(new decimal[] { 2, 10, 100 }));
            return Task.CompletedTask;
        });

        foreach (HttpStatusCode status in new[] { HttpStatusCode.Forbidden, HttpStatusCode.InternalServerError })
        {
            await Verificar($"HTTP {(int)status} é tratado sem anunciar sucesso", async () =>
            {
                using HttpClient cliente = ClienteJson("[]", status);
                MenuConsultaApi menu = new MenuConsultaApi(new ProdutosApiServico(cliente));
                string saida = await CapturarAsync(menu.ConsultarAsync);
                Exigir(saida.Contains($"HTTP {(int)status}") && !saida.Contains("Consulta concluída"));
            });
        }
        await Verificar("Falha de rede é tratada", async () =>
        {
            using HttpClient cliente = new HttpClient(new RespostaHttpSimulada((_, _) =>
                Task.FromException<HttpResponseMessage>(new HttpRequestException("Rede indisponível"))));
            string saida = await CapturarAsync(new MenuConsultaApi(new ProdutosApiServico(cliente)).ConsultarAsync);
            Exigir(saida.Contains("Verifique a conexão") && !saida.Contains("Consulta concluída"));
        });
        await Verificar("Timeout real do HttpClient é tratado", async () =>
        {
            using HttpClient cliente = new HttpClient(new RespostaHttpSimulada(async (_, token) =>
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return RespostaHttpSimulada.CriarJson("[]");
            })) { Timeout = TimeSpan.FromMilliseconds(50) };
            string saida = await CapturarAsync(new MenuConsultaApi(new ProdutosApiServico(cliente)).ConsultarAsync);
            Exigir(saida.Contains("tempo limite") && !saida.Contains("Consulta concluída"));
        });
        await Verificar("Cancelamento é propagado pelo serviço", async () =>
        {
            using HttpClient cliente = ClienteJson("[]");
            await EsperarExcecaoAsync<OperationCanceledException>(() =>
                new ProdutosApiServico(cliente).ConsultarAsync(new CancellationToken(canceled: true)));
        });
        await Verificar("JSON inválido gera mensagem adequada no menu", async () =>
        {
            using HttpClient cliente = ClienteJson("<html>erro</html>");
            string saida = await CapturarAsync(new MenuConsultaApi(new ProdutosApiServico(cliente)).ConsultarAsync);
            Exigir(saida.Contains("JSON inválido") && !saida.Contains("Consulta concluída"));
        });
        await Verificar("Ordenação antes da consulta orienta o usuário", async () =>
        {
            using HttpClient cliente = ClienteJson("[]");
            MenuConsultaApi menu = new MenuConsultaApi(new ProdutosApiServico(cliente));
            string saida = await CapturarAsync(() =>
            {
                menu.ListarPorNome();
                menu.ListarPorPreco();
                return Task.CompletedTask;
            });
            Exigir(saida.Split("Consulte a API pela opção 14").Length == 3);
        });
        await Verificar("Falha de atualização preserva a consulta anterior", async () =>
        {
            int chamadas = 0;
            using HttpClient cliente = new HttpClient(new RespostaHttpSimulada((_, _) =>
                Task.FromResult(++chamadas == 1
                    ? RespostaHttpSimulada.CriarJson(DemonstracaoApi.JsonExemplo)
                    : RespostaHttpSimulada.CriarJson("[]", HttpStatusCode.Forbidden))));
            MenuConsultaApi menu = new MenuConsultaApi(new ProdutosApiServico(cliente));
            await CapturarAsync(menu.ConsultarAsync);
            string saida = await CapturarAsync(async () =>
            {
                await menu.ConsultarAsync();
                menu.ListarPorNome();
            });
            Exigir(saida.Contains("última consulta bem-sucedida foi mantida") && saida.Contains("Nome: Abajur"));
        });
        await Verificar("Atualização vazia substitui os dados antigos", async () =>
        {
            int chamadas = 0;
            using HttpClient cliente = new HttpClient(new RespostaHttpSimulada((_, _) =>
                Task.FromResult(RespostaHttpSimulada.CriarJson(++chamadas == 1 ? DemonstracaoApi.JsonExemplo : "[]"))));
            MenuConsultaApi menu = new MenuConsultaApi(new ProdutosApiServico(cliente));
            await CapturarAsync(menu.ConsultarAsync);
            string saida = await CapturarAsync(async () =>
            {
                await menu.ConsultarAsync();
                menu.ListarPorPreco();
            });
            Exigir(saida.Contains("0 produto(s)") && saida.Contains("não retornou produtos") && !saida.Contains("Nome: Abajur"));
        });
        await Verificar("Menu principal consulta e ordena sem alterar catálogo local", async () =>
        {
            int chamadas = 0;
            using HttpClient cliente = new HttpClient(new RespostaHttpSimulada((_, _) =>
            {
                chamadas++;
                return Task.FromResult(RespostaHttpSimulada.CriarJson(DemonstracaoApi.JsonExemplo));
            }));
            MenuPrincipal menu = new MenuPrincipal(new ProdutosApiServico(cliente));
            string saida = await CapturarAsync(menu.ExibirMenuDeOpcoesAsync, "14\n15\n16\n4\n-1\n");
            Exigir(chamadas == 1 && saida.Contains("3 produto(s)") && saida.Contains("Nenhum produto cadastrado."));
            Exigir(saida.Contains("Preço: 10,00") && !saida.Contains("R$"));
            VerificarOrdem(saida, "=== Produtos externos por nome", "Abajur", "Livro de C#", "Zebra de brinquedo");
            VerificarOrdem(saida, "=== Produtos externos por preço", "Zebra de brinquedo", "Abajur", "Livro de C#");
            Exigir(saida.Split("COMEX FINALIZADO!").Length == 2);
        });
        await Verificar("Erro HTTP devolve o controle ao menu principal", async () =>
        {
            using HttpClient cliente = ClienteJson("[]", HttpStatusCode.Forbidden);
            MenuPrincipal menu = new MenuPrincipal(new ProdutosApiServico(cliente));
            string saida = await CapturarAsync(menu.ExibirMenuDeOpcoesAsync, "14\n4\n-1\n");
            Exigir(saida.Contains("HTTP 403") && saida.Contains("Nenhum produto cadastrado.") && saida.Contains("COMEX FINALIZADO!"));
        });
        await Verificar("Demonstração offline é identificada e não preenche a consulta real", async () =>
        {
            using HttpClient cliente = new HttpClient(new RespostaHttpSimulada((_, _) =>
                throw new InvalidOperationException("Não deveria acessar a rede neste cenário.")));
            MenuPrincipal menu = new MenuPrincipal(new ProdutosApiServico(cliente));
            string saida = await CapturarAsync(menu.ExibirMenuDeOpcoesAsync, "17\n15\n4\n-1\n");
            Exigir(saida.Contains("RESPOSTA HTTP SIMULADA") && saida.Contains("não acessa a API real") &&
                saida.Contains("Consulte a API pela opção 14") && saida.Contains("Nenhum produto cadastrado."));
        });

        Console.WriteLine($"\n{aprovados} testes da Semana 4 aprovados.");
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

    private static void VerificarOrdem(string saida, string titulo, params string[] nomes)
    {
        int inicio = saida.IndexOf(titulo, StringComparison.Ordinal);
        Exigir(inicio >= 0);
        string lista = saida[inicio..];
        int anterior = -1;
        foreach (string nome in nomes)
        {
            int posicao = lista.IndexOf("Nome: " + nome, StringComparison.Ordinal);
            Exigir(posicao > anterior);
            anterior = posicao;
        }
    }

    private static void Exigir(bool condicao)
    {
        if (!condicao)
            throw new InvalidOperationException("O resultado não corresponde ao esperado.");
    }

    private static async Task EsperarExcecaoAsync<T>(Func<Task> acao) where T : Exception
    {
        try { await acao(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Era esperada uma exceção {typeof(T).Name}.");
    }
}
