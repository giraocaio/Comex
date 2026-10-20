using Comex.Modelos;
using Comex.Servicos;
using static Comex.Utilitarios.EntradaConsole;

namespace Comex.Menus;

public class MenuCompras
{
    private readonly Dictionary<string, Cliente> clientes;
    private readonly CatalogoProdutos catalogo;

    public MenuCompras(Dictionary<string, Cliente> clientes, CatalogoProdutos catalogo)
    {
        this.clientes = clientes;
        this.catalogo = catalogo;
    }

    public void AdicionaProdutoNoCarrinho()
    {
        string cpf = LerCpf();
        if (!clientes.ContainsKey(cpf))
        {
            Console.WriteLine("Usuário não cadastrado.");
            return;
        }
        string nome = LerTexto("Qual produto deseja adicionar? ");
        Produto? produto = catalogo.Buscar(nome);
        if (produto == null)
        {
            Console.WriteLine("Produto não cadastrado.");
            return;
        }

        clientes[cpf].AdicionarProdutoAoCarrinho(produto);
        Console.WriteLine($"Produto {produto.Nome} adicionado ao carrinho de {clientes[cpf].Nome} com sucesso.");
    }

    public void FechaCompra()
    {
        string cpf = LerCpf();
        if (!clientes.ContainsKey(cpf))
        {
            Console.WriteLine("Cliente não cadastrado.");
            return;
        }
        Cliente cliente = clientes[cpf];
        if (cliente.Carrinho.Count == 0)
        {
            Console.WriteLine("O carrinho está vazio.");
            return;
        }

        Console.WriteLine($"\nCarrinho de {cliente.Nome}:");
        // A compra considera o preço atual do catálogo no momento do fechamento.
        foreach (Produto produto in cliente.Carrinho)
        {
            Console.WriteLine($"{produto.Nome}: {Moeda(produto.PrecoUnitario)}");
        }
        decimal total = cliente.CalcularTotalDoCarrinho();
        Console.WriteLine($"Total: {Moeda(total)}");
        Console.WriteLine("PAGAMENTO SIMULADO: use somente um número de cartão fictício.");
        string cartao = LerTexto("Número fictício do cartão (16 dígitos, ou Enter para cancelar): ")
            .Replace(" ", "").Replace("-", "");
        if (cartao.Length == 0)
        {
            Console.WriteLine("Compra cancelada. Os produtos continuam no carrinho.");
            return;
        }
        if (cartao.Length != 16 || !SomenteDigitos(cartao))
        {
            Console.WriteLine("Número inválido. Use 16 dígitos fictícios. O carrinho foi mantido.");
            return;
        }

        Console.WriteLine($"Realizando pagamento de {Moeda(total)} no cartão {cartao}.");
        Console.WriteLine("Compra finalizada com sucesso (simulação, sem cobrança real).");
        cliente.EsvaziarCarrinho();
    }
}
