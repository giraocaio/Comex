using System.Globalization;
using Comex.Modelos;

namespace Comex.Servicos;

public class CatalogoProdutos
{
    // Produto, Eletronico e Livro compartilham o mesmo catálogo.
    private readonly Dictionary<string, Produto> produtos = new(StringComparer.OrdinalIgnoreCase);
    private static readonly StringComparer comparadorNomes =
        StringComparer.Create(CultureInfo.GetCultureInfo("pt-BR"), ignoreCase: true);

    public bool Cadastrar(Produto produto)
    {
        ArgumentNullException.ThrowIfNull(produto);
        return produtos.TryAdd(produto.Nome, produto);
    }

    public Produto? Buscar(string nome)
    {
        produtos.TryGetValue(nome.Trim(), out Produto? produto);
        return produto;
    }

    public IReadOnlyList<Produto> Listar() => produtos.Values.ToList();

    public IReadOnlyList<Produto> OrdenarPorNome() => produtos.Values
        .OrderBy(produto => produto.Nome, comparadorNomes)
        .ToList();

    public IReadOnlyList<Produto> OrdenarPorPreco() => produtos.Values
        .OrderBy(produto => produto.PrecoUnitario)
        .ThenBy(produto => produto.Nome, comparadorNomes)
        .ToList();
}
