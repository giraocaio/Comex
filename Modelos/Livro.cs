using Comex.Interfaces;

namespace Comex.Modelos;

public class Livro : Produto, IIdentificavel
{
    public string Isbn { get; }
    public int TotalPaginas { get; }

    public Livro(string nome, string isbn, int totalPaginas) : base(nome)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("O ISBN é obrigatório.", nameof(isbn));
        if (totalPaginas <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalPaginas), "O total de páginas deve ser positivo.");

        Isbn = isbn.Trim();
        TotalPaginas = totalPaginas;
    }

    public string Identificar() => $"Nome: {Nome} | ISBN: {Isbn}";

    public override void ExibirDados()
    {
        base.ExibirDados();
        Console.WriteLine($"ISBN: {Isbn}");
        Console.WriteLine($"Total de páginas: {TotalPaginas}");
    }
}
