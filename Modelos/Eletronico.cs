namespace Comex.Modelos;

public class Eletronico : Produto
{
    public int Voltagem { get; }
    public decimal Potencia { get; }

    public Eletronico(string nome, int voltagem, decimal potencia) : base(nome)
    {
        if (voltagem <= 0)
            throw new ArgumentOutOfRangeException(nameof(voltagem), "A voltagem deve ser positiva.");
        if (potencia <= 0)
            throw new ArgumentOutOfRangeException(nameof(potencia), "A potência deve ser positiva.");

        Voltagem = voltagem;
        Potencia = potencia;
    }

    public override void ExibirDados()
    {
        base.ExibirDados();
        Console.WriteLine($"Voltagem: {Voltagem} V");
        Console.WriteLine($"Potência: {Potencia.ToString(System.Globalization.CultureInfo.GetCultureInfo("pt-BR"))} W");
    }
}
