using System.Text.Json;
using Comex.GeneradorRelatorio;
using Comex.Modelos;
using Comex.Servicos;

namespace Comex.Menus;

public class MenuGerarRelatorioCsv : Menu
{
    private readonly string caminhoJson;
    private readonly string caminhoCsv;

    public MenuGerarRelatorioCsv(string caminhoJson = "produtos.json",
        string caminhoCsv = "relatorio-produtos.csv") : base("Gerar relatório CSV do JSON salvo")
    {
        this.caminhoJson = Path.GetFullPath(caminhoJson);
        this.caminhoCsv = Path.GetFullPath(caminhoCsv);
    }

    public override async Task ExecutarAsync()
    {
        Console.WriteLine("\n=== Relatório: produtos com preço maior que zero, do maior para o menor ===");
        if (!File.Exists(caminhoJson))
        {
            Console.WriteLine("Arquivo produtos.json não encontrado. Consulte a API pela opção 8 ou 14 primeiro.");
            return;
        }

        try
        {
            string json = await File.ReadAllTextAsync(caminhoJson);
            List<ProdutoApi> produtos = ProdutosApiServico.DesserializarProdutos(json);
            List<ProdutoRelatorioDto> dados = produtos
                .Where(produto => produto.PrecoUnitario > 0)
                .OrderByDescending(produto => produto.PrecoUnitario)
                .ThenBy(produto => produto.Id)
                .Select(produto => new ProdutoRelatorioDto
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.PrecoUnitario
                })
                .ToList();

            IGeradorRelatorio gerador = new GeradorCsv();
            string caminhoSalvo = gerador.Exportar(dados, caminhoCsv);
            Console.WriteLine($"Relatório gerado com {dados.Count} produto(s).");
            if (dados.Count == 0)
                Console.WriteLine("Nenhum produto atende ao filtro. O CSV contém somente o cabeçalho.");
            Console.WriteLine("Preços conforme a origem dos dados, sem conversão de moeda.");
            Console.WriteLine($"Arquivo salvo em: {caminhoSalvo}");
        }
        catch (JsonException)
        {
            Console.WriteLine("produtos.json contém JSON inválido ou dados incompatíveis. O relatório não foi gerado.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Sem permissão para ler o JSON ou gravar o CSV. Verifique as permissões da pasta.");
        }
        catch (IOException)
        {
            Console.WriteLine("Não foi possível ler o JSON ou gravar o CSV. Verifique os caminhos e tente novamente.");
        }
    }
}
