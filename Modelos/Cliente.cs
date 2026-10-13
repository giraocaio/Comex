namespace Comex.Modelos;

public class Cliente
{
    private readonly List<Produto> carrinho = new List<Produto>();

    public Cliente(string nome, string cpf, Endereco endereco)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("O CPF é obrigatório.", nameof(cpf));
        if (endereco == null)
            throw new ArgumentNullException(nameof(endereco));

        Nome = nome.Trim();
        CPF = cpf.Trim();
        Endereco = endereco;
    }

    public string Nome { get; }
    public string CPF { get; }
    public string Email { get; set; } = "";
    public string Profissao { get; set; } = "";
    public string Telefone { get; set; } = "";
    public Endereco Endereco { get; }

    // A lista pode ser consultada, mas só os métodos do cliente a modificam.
    public IReadOnlyList<Produto> Carrinho => carrinho.AsReadOnly();

    public void AdicionarProdutoAoCarrinho(Produto produto)
    {
        if (produto == null)
            throw new ArgumentNullException(nameof(produto));
        carrinho.Add(produto);
    }

    public decimal CalcularTotalDoCarrinho()
    {
        decimal total = 0;
        foreach (Produto produto in carrinho)
            total += produto.PrecoUnitario;
        return total;
    }

    public void EsvaziarCarrinho()
    {
        carrinho.Clear();
    }

    public void ExibirDados()
    {
        Console.WriteLine($"CPF: {CPF} | Nome: {Nome}");
        Console.WriteLine($"E-mail: {Email}");
        Console.WriteLine($"Profissão: {Profissao}");
        Console.WriteLine($"Telefone: {Telefone}");
        Endereco.ExibirDados();
    }
}
