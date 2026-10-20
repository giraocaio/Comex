using Comex.Interfaces;
using Comex.Modelos;
using Comex.Servicos;

namespace Comex.Testes;

public static class TesteHerancaEInterfaces
{
    public static void Executar()
    {
        Eletronico eletronico = new Eletronico("Ventilador", 127, 60)
        {
            Descricao = "Ventilador de mesa", PrecoUnitario = 150, Quantidade = 5
        };
        Livro livro = new Livro("Aprendendo C#", "9780000000002", 200)
        {
            Descricao = "Livro didático de exemplo", PrecoUnitario = 50, Quantidade = 10
        };
        Produto produto = new Produto("Caderno")
        {
            Descricao = "100 folhas", PrecoUnitario = 25, Quantidade = 20
        };
        Cliente cliente = new Cliente("Ana Teste", "11111111111", new Endereco
        {
            Rua = "Rua de Exemplo", Numero = 100, Bairro = "Centro",
            Cidade = "São Paulo", Estado = "SP"
        });

        Console.WriteLine("\n=== HERANÇA E POLIMORFISMO ===");
        List<Produto> produtos = new List<Produto> { eletronico, livro, produto };
        foreach (Produto item in produtos)
        {
            item.ExibirDados();
            Console.WriteLine();
        }

        Console.WriteLine("=== INTERFACE IIdentificavel ===");
        List<IIdentificavel> identificaveis = new List<IIdentificavel> { cliente, livro };
        foreach (IIdentificavel item in identificaveis)
            Console.WriteLine(item.Identificar());

        CatalogoProdutos catalogo = new CatalogoProdutos();
        foreach (Produto item in produtos)
            catalogo.Cadastrar(item);
        Console.WriteLine("\nLINQ por nome: " + string.Join(", ", catalogo.OrdenarPorNome().Select(p => p.Nome)));
        Console.WriteLine("LINQ por preço: " + string.Join(", ", catalogo.OrdenarPorPreco().Select(p => p.Nome)));
        Console.WriteLine("Demonstração concluída. Estes objetos não foram adicionados aos cadastros do menu.");
    }
}
