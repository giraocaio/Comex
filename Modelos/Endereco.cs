namespace Comex.Modelos;

public class Endereco
{
    private int numero;

    public string Bairro { get; set; } = "";
    public string Cidade { get; set; } = "";
    public string Complemento { get; set; } = "";
    public string Estado { get; set; } = "";
    public string Rua { get; set; } = "";

    public int Numero
    {
        get => numero;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "O número não pode ser negativo.");
            numero = value;
        }
    }

    public void ExibirDados()
    {
        Console.WriteLine($"Endereço: {Rua}, {Numero}");
        Console.WriteLine($"Bairro: {Bairro}");
        Console.WriteLine($"Cidade: {Cidade} | Estado: {Estado}");
        Console.WriteLine($"Complemento: {(string.IsNullOrWhiteSpace(Complemento) ? "Não informado" : Complemento)}");
    }
}
