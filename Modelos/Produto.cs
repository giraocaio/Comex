using System.Globalization;

namespace Comex.Modelos;

public class Produto
{
    private decimal precoUnitario;
    private int quantidade;

    // O nome é obrigatório na criação e não pode mudar a chave do catálogo.
    public Produto(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(nome));

        Nome = nome.Trim();
    }

    public string Nome { get; }
    public string Descricao { get; set; } = "";

    // decimal representa valores monetários sem imprecisões binárias de double.
    public decimal PrecoUnitario
    {
        get => precoUnitario;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "O preço não pode ser negativo.");
            precoUnitario = value;
        }
    }

    public int Quantidade
    {
        get => quantidade;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "A quantidade não pode ser negativa.");
            quantidade = value;
        }
    }

    public virtual void ExibirDados()
    {
        Console.WriteLine($"Produto: {Nome}");
        Console.WriteLine($"Descrição: {Descricao}");
        Console.WriteLine($"Preço unitário: {PrecoUnitario.ToString("C2", new CultureInfo("pt-BR"))}");
        Console.WriteLine($"Quantidade cadastrada: {Quantidade}");
    }
}
