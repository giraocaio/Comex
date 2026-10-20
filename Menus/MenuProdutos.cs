using Comex.Modelos;
using Comex.Servicos;
using static Comex.Utilitarios.EntradaConsole;

namespace Comex.Menus;

public class MenuProdutos
{
    private readonly CatalogoProdutos catalogo;

    public MenuProdutos(CatalogoProdutos catalogo)
    {
        this.catalogo = catalogo;
    }

    public void CadastraProduto() => Cadastrar("comum");
    public void CadastraEletronico() => Cadastrar("eletronico");
    public void CadastraLivro() => Cadastrar("livro");

    private void Cadastrar(string tipo)
    {
        string nome = LerTexto("Nome do produto: ");
        if (nome.Length == 0)
        {
            Console.WriteLine("O nome não pode ficar vazio.");
            return;
        }
        if (catalogo.Buscar(nome) != null)
        {
            Console.WriteLine("Produto já cadastrado. Use a opção de ajustar preço.");
            return;
        }
        string descricao = LerTexto("Descrição do produto: ");
        if (descricao.Length == 0)
        {
            Console.WriteLine("A descrição não pode ficar vazia.");
            return;
        }
        if (!LerPreco("Preço unitário (ex.: 29,90): ", out decimal preco))
            return;
        if (!LerInteiroNaoNegativo("Quantidade cadastrada: ", out int quantidade))
            return;

        Produto produto;
        if (tipo == "eletronico")
        {
            if (!LerInteiroPositivo("Voltagem (V): ", out int voltagem))
                return;
            if (!LerDecimalPositivo("Potência (W): ", out decimal potencia))
                return;
            produto = new Eletronico(nome, voltagem, potencia);
        }
        else if (tipo == "livro")
        {
            string isbn = LerTexto("ISBN: ");
            if (isbn.Length == 0)
            {
                Console.WriteLine("O ISBN é obrigatório.");
                return;
            }
            if (!LerInteiroPositivo("Total de páginas: ", out int paginas))
                return;
            produto = new Livro(nome, isbn, paginas);
        }
        else
        {
            produto = new Produto(nome);
        }

        produto.Descricao = descricao;
        produto.PrecoUnitario = preco;
        produto.Quantidade = quantidade;
        catalogo.Cadastrar(produto);
        Console.WriteLine($"Produto {nome} cadastrado com sucesso.");
    }

    public void ListaProdutos() => ExibirLista(catalogo.Listar(), "Catálogo de produtos");

    public void ListaPorNome() => ExibirLista(catalogo.OrdenarPorNome(), "Produtos por nome (A a Z)");

    public void ListaPorPreco() => ExibirLista(catalogo.OrdenarPorPreco(), "Produtos por preço (menor para maior)");

    private static void ExibirLista(IReadOnlyList<Produto> produtos, string titulo)
    {
        Console.WriteLine($"\n=== {titulo} ===");
        if (produtos.Count == 0)
        {
            Console.WriteLine("Nenhum produto cadastrado.");
            return;
        }
        foreach (Produto produto in produtos)
        {
            // A implementação correspondente ao tipo real é chamada (polimorfismo).
            produto.ExibirDados();
            Console.WriteLine();
        }
    }

    public void AjustarPrecoDeProduto()
    {
        string nome = LerTexto("Qual produto deseja ajustar? ");
        Produto? produto = catalogo.Buscar(nome);
        if (produto == null)
        {
            Console.WriteLine("Produto não cadastrado no sistema.");
            return;
        }
        if (!LerPreco("Novo preço (ex.: 29,90): ", out decimal preco))
            return;

        produto.PrecoUnitario = preco;
        Console.WriteLine($"Preço do produto {produto.Nome} ajustado para {Moeda(preco)}.");
    }
}
