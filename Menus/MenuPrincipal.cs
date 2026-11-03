using Comex.Modelos;
using Comex.Servicos;
using Comex.Testes;

namespace Comex.Menus;

public class MenuPrincipal
{
    private readonly Dictionary<int, Menu> menus;

    public MenuPrincipal(ProdutosApiServico? servicoApi = null) : this(CriarMenus(servicoApi)) { }

    public MenuPrincipal(Dictionary<int, Menu> menus)
    {
        this.menus = menus;
    }

    public static Dictionary<int, Menu> CriarMenus(ProdutosApiServico? servicoApi = null)
    {
        Dictionary<string, Cliente> clientes = new Dictionary<string, Cliente>();
        CatalogoProdutos catalogo = new CatalogoProdutos();
        MenuClientes menuClientes = new MenuClientes(clientes);
        MenuProdutos menuProdutos = new MenuProdutos(catalogo);
        MenuCompras menuCompras = new MenuCompras(clientes, catalogo);
        MenuIdentificacoes menuIdentificacoes = new MenuIdentificacoes(clientes, catalogo);
        MenuConsultaApi menuConsultaApi = new MenuConsultaApi(servicoApi ?? new ProdutosApiServico());

        return new Dictionary<int, Menu>
        {
            [1] = new MenuAcao("Cadastrar cliente", menuClientes.CadastraCliente),
            [2] = new MenuAcao("Listar clientes", menuClientes.ListaClientes),
            [3] = new MenuAcao("Cadastrar produto comum", menuProdutos.CadastraProduto),
            [4] = new MenuAcao("Listar produtos", menuProdutos.ListaProdutos),
            [5] = new MenuAcao("Ajustar preço de produto", menuProdutos.AjustarPrecoDeProduto),
            [6] = new MenuAcao("Adicionar produto no carrinho", menuCompras.AdicionaProdutoNoCarrinho),
            [7] = new MenuAcao("Fechar compra", menuCompras.FechaCompra),
            [8] = new MenuAcao("Consultar produtos da API e salvar JSON", menuConsultaApi.ConsultarAsync),
            [9] = new MenuAcao("Cadastrar eletrônico", menuProdutos.CadastraEletronico),
            [10] = new MenuAcao("Cadastrar livro", menuProdutos.CadastraLivro),
            [11] = new MenuAcao("Identificar clientes e livros", menuIdentificacoes.Exibir),
            [12] = new MenuAcao("Listar produtos por nome (A a Z)", menuProdutos.ListaPorNome),
            [13] = new MenuAcao("Listar produtos por preço (menor para maior)", menuProdutos.ListaPorPreco),
            [14] = new MenuAcao("Consultar API e salvar JSON (atalho da Semana 4)", menuConsultaApi.ConsultarAsync),
            [15] = new MenuAcao("Ordenar produtos externos por nome", menuConsultaApi.ListarPorNome),
            [16] = new MenuAcao("Ordenar produtos externos por preço", menuConsultaApi.ListarPorPreco),
            [17] = new MenuAcao("Demonstrar consulta simulada (offline)", DemonstracaoApi.ExecutarAsync),
            [19] = new MenuAcao("Demonstrar herança e interfaces da Semana 3", TesteHerancaEInterfaces.Executar)
        };
    }

    public async Task ExibirMenuDeOpcoesAsync()
    {
        while (true)
        {
            Console.WriteLine("\n=== COMEX | SEMANA 5 ===");
            foreach (KeyValuePair<int, Menu> item in menus.OrderBy(item => item.Key))
                Console.WriteLine($"{item.Key} - {item.Value.Titulo}");
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

            if (opcao == -1)
            {
                Console.WriteLine("COMEX FINALIZADO!");
                return;
            }
            if (menus.TryGetValue(opcao, out Menu? menu))
                await menu.ExecutarAsync();
            else
                Console.WriteLine("Opção inválida. Escolha uma opção do menu.");
        }
    }
}
