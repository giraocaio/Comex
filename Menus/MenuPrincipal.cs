using Comex.Modelos;
using Comex.Servicos;
using Comex.Testes;

namespace Comex.Menus;

public class MenuPrincipal
{
    private readonly MenuClientes menuClientes;
    private readonly MenuProdutos menuProdutos;
    private readonly MenuCompras menuCompras;
    private readonly MenuIdentificacoes menuIdentificacoes;

    public MenuPrincipal()
    {
        Dictionary<string, Cliente> clientes = new Dictionary<string, Cliente>();
        CatalogoProdutos catalogo = new CatalogoProdutos();
        // Todos os menus recebem os mesmos cadastros em memória.
        menuClientes = new MenuClientes(clientes);
        menuProdutos = new MenuProdutos(catalogo);
        menuCompras = new MenuCompras(clientes, catalogo);
        menuIdentificacoes = new MenuIdentificacoes(clientes, catalogo);
    }

    public void ExibirMenuDeOpcoes()
    {
        while (true)
        {
            Console.WriteLine("\n=== COMEX | SEMANA 3 ===");
            Console.WriteLine("1 - Cadastrar cliente");
            Console.WriteLine("2 - Listar clientes");
            Console.WriteLine("3 - Cadastrar produto comum");
            Console.WriteLine("4 - Listar produtos");
            Console.WriteLine("5 - Ajustar preço de produto");
            Console.WriteLine("6 - Adicionar produto no carrinho");
            Console.WriteLine("7 - Fechar compra");
            Console.WriteLine("8 - Demonstrar herança e interfaces da Semana 3");
            Console.WriteLine("9 - Cadastrar eletrônico");
            Console.WriteLine("10 - Cadastrar livro");
            Console.WriteLine("11 - Identificar clientes e livros");
            Console.WriteLine("12 - Listar produtos por nome (A a Z)");
            Console.WriteLine("13 - Listar produtos por preço (menor para maior)");
            Console.WriteLine("-1 - Sair");
            Console.Write("Digite sua opção: ");
            string? entrada = Console.ReadLine();

            if (entrada == null)
            {
                Console.WriteLine("COMEX FINALIZADO!");
                return;
            }
            Console.WriteLine($"Opção escolhida: {entrada}");
            if (!int.TryParse(entrada, out int opcao))
            {
                Console.WriteLine("Opção inválida. Digite um número do menu.");
                continue;
            }

            switch (opcao)
            {
                case 1: menuClientes.CadastraCliente(); break;
                case 2: menuClientes.ListaClientes(); break;
                case 3: menuProdutos.CadastraProduto(); break;
                case 4: menuProdutos.ListaProdutos(); break;
                case 5: menuProdutos.AjustarPrecoDeProduto(); break;
                case 6: menuCompras.AdicionaProdutoNoCarrinho(); break;
                case 7: menuCompras.FechaCompra(); break;
                case 8: TesteHerancaEInterfaces.Executar(); break;
                case 9: menuProdutos.CadastraEletronico(); break;
                case 10: menuProdutos.CadastraLivro(); break;
                case 11: menuIdentificacoes.Exibir(); break;
                case 12: menuProdutos.ListaPorNome(); break;
                case 13: menuProdutos.ListaPorPreco(); break;
                case -1:
                    Console.WriteLine("COMEX FINALIZADO!");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Escolha uma opção do menu.");
                    break;
            }
        }
    }
}
