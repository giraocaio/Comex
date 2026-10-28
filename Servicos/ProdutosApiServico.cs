using System.Globalization;
using System.Text.Json;
using Comex.Modelos;

namespace Comex.Servicos;

public class ProdutosApiServico
{
    public const string EnderecoApi = "https://fakestoreapi.com/products";

    // A mesma conexão HTTP pode ser reutilizada durante a execução.
    private static readonly HttpClient clienteCompartilhado = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(15)
    };
    private static readonly StringComparer comparadorNomes =
        StringComparer.Create(CultureInfo.GetCultureInfo("pt-BR"), ignoreCase: true);
    private readonly HttpClient cliente;

    // Um cliente alternativo permite testar respostas sem depender da internet.
    public ProdutosApiServico(HttpClient? cliente = null)
    {
        this.cliente = cliente ?? clienteCompartilhado;
    }

    public async Task<List<ProdutoApi>> ConsultarAsync(CancellationToken cancellationToken = default)
    {
        using HttpResponseMessage resposta = await cliente.GetAsync(EnderecoApi, cancellationToken);
        resposta.EnsureSuccessStatusCode();

        string json = await resposta.Content.ReadAsStringAsync(cancellationToken);
        List<ProdutoApi>? produtos = JsonSerializer.Deserialize<List<ProdutoApi>>(json);

        if (produtos == null || produtos.Any(produto => produto == null ||
            produto.Id <= 0 || string.IsNullOrWhiteSpace(produto.Nome) ||
            produto.PrecoUnitario < 0 || produto.Descricao == null))
        {
            throw new JsonException("A resposta não contém uma lista válida de produtos.");
        }

        return produtos;
    }

    public static IReadOnlyList<ProdutoApi> OrdenarPorNome(IEnumerable<ProdutoApi> produtos) => produtos
        .OrderBy(produto => produto.Nome, comparadorNomes)
        .ThenBy(produto => produto.Id)
        .ToList();

    public static IReadOnlyList<ProdutoApi> OrdenarPorPreco(IEnumerable<ProdutoApi> produtos) => produtos
        .OrderBy(produto => produto.PrecoUnitario)
        .ThenBy(produto => produto.Nome, comparadorNomes)
        .ThenBy(produto => produto.Id)
        .ToList();
}
