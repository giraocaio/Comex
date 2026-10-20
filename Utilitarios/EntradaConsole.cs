using System.Globalization;

namespace Comex.Utilitarios;

public static class EntradaConsole
{
    public static string LerTexto(string mensagem)
    {
        Console.Write(mensagem);
        return Console.ReadLine()?.Trim() ?? "";
    }

    public static string LerCpf()
    {
        return LerTexto("CPF do cliente: ").Replace(".", "").Replace("-", "");
    }

    public static bool SomenteDigitos(string texto)
    {
        foreach (char caractere in texto)
        {
            if (caractere < '0' || caractere > '9')
                return false;
        }
        return texto.Length > 0;
    }

    public static bool LerPreco(string mensagem, out decimal preco)
    {
        string entrada = LerTexto(mensagem).Replace(',', '.');
        // Sem separador de milhar; aceita vírgula ou ponto para os centavos.
        bool valido = decimal.TryParse(entrada, NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out preco);
        if (!valido || preco <= 0 || preco > 1000000 || decimal.Round(preco, 2) != preco)
        {
            Console.WriteLine("Preço inválido. Use um valor entre 0,01 e 1000000,00, com até 2 casas decimais e sem separador de milhar.");
            return false;
        }
        return true;
    }

    public static bool LerInteiroNaoNegativo(string mensagem, out int numero)
    {
        string entrada = LerTexto(mensagem);
        if (!int.TryParse(entrada, out numero) || numero < 0)
        {
            Console.WriteLine("Valor inválido. Informe um número inteiro maior ou igual a zero.");
            return false;
        }
        return true;
    }

    public static bool LerInteiroPositivo(string mensagem, out int numero)
    {
        string entrada = LerTexto(mensagem);
        if (!int.TryParse(entrada, out numero) || numero <= 0)
        {
            Console.WriteLine("Valor inválido. Informe um número inteiro maior que zero.");
            return false;
        }
        return true;
    }

    public static bool LerDecimalPositivo(string mensagem, out decimal numero)
    {
        string entrada = LerTexto(mensagem).Replace(',', '.');
        if (!decimal.TryParse(entrada, NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out numero) || numero <= 0)
        {
            Console.WriteLine("Valor inválido. Informe um número positivo, sem separador de milhar.");
            return false;
        }
        return true;
    }

    public static string Moeda(decimal valor)
    {
        return valor.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
    }
}
