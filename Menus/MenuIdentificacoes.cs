using Comex.Interfaces;
using Comex.Modelos;
using Comex.Servicos;

namespace Comex.Menus;

public class MenuIdentificacoes
{
    private readonly Dictionary<string, Cliente> clientes;
    private readonly CatalogoProdutos catalogo;

    public MenuIdentificacoes(Dictionary<string, Cliente> clientes, CatalogoProdutos catalogo)
    {
        this.clientes = clientes;
        this.catalogo = catalogo;
    }

    public void Exibir()
    {
        // A interface permite tratar clientes e livros pelo mesmo contrato.
        List<IIdentificavel> identificaveis = new List<IIdentificavel>();
        identificaveis.AddRange(clientes.Values);
        identificaveis.AddRange(catalogo.Listar().OfType<IIdentificavel>());

        if (identificaveis.Count == 0)
        {
            Console.WriteLine("Nenhum cliente ou livro cadastrado para identificar.");
            return;
        }
        foreach (IIdentificavel item in identificaveis)
            Console.WriteLine(item.Identificar());
    }
}
