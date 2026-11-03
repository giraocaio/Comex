using System.Globalization;
using System.Text;

namespace Comex.GeneradorRelatorio;

/// <summary>Gera CSV com separador ponto e vírgula, valores em pt-BR e codificação UTF-8.</summary>
public class GeradorCsv : IGeradorRelatorio
{
    /// <summary>Grava o cabeçalho e os produtos, substituindo um arquivo anterior no mesmo caminho.</summary>
    /// <param name="produtos">Produtos a exportar. Uma coleção vazia gera somente o cabeçalho.</param>
    /// <param name="caminhoArquivo">Destino do CSV. A pasta de destino deve existir.</param>
    /// <returns>O caminho absoluto calculado por <see cref="Path.GetFullPath(string)"/>.</returns>
    /// <exception cref="ArgumentNullException">A coleção ou um produto é nulo.</exception>
    /// <exception cref="ArgumentException">O caminho não foi informado.</exception>
    /// <exception cref="IOException">Ocorreu uma falha de gravação.</exception>
    /// <exception cref="UnauthorizedAccessException">Não há permissão de gravação.</exception>
    public string Exportar(IEnumerable<ProdutoRelatorioDto> produtos, string caminhoArquivo)
    {
        ArgumentNullException.ThrowIfNull(produtos);
        ArgumentException.ThrowIfNullOrWhiteSpace(caminhoArquivo);
        string caminhoCompleto = Path.GetFullPath(caminhoArquivo);
        StringBuilder csv = new StringBuilder("Id;Nome;Descricao;Preco\r\n");
        foreach (ProdutoRelatorioDto produto in produtos)
        {
            ArgumentNullException.ThrowIfNull(produto);
            csv.Append(produto.Id.ToString(CultureInfo.InvariantCulture)).Append(';')
                .Append(EscaparTexto(produto.Nome)).Append(';')
                .Append(EscaparTexto(produto.Descricao)).Append(';')
                .Append(produto.Preco.ToString("0.00", CultureInfo.GetCultureInfo("pt-BR")))
                .Append("\r\n");
        }

        // O BOM facilita o reconhecimento dos acentos ao abrir o CSV em planilhas.
        File.WriteAllText(caminhoCompleto, csv.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
        return caminhoCompleto;
    }

    private static string EscaparTexto(string? texto)
    {
        texto ??= string.Empty;
        // Texto vindo de fora não deve ser interpretado como fórmula por uma planilha.
        if (texto.Length > 0 && "=+-@\t\r\n".Contains(texto[0]))
            texto = "'" + texto;
        return "\"" + texto.Replace("\"", "\"\"") + "\"";
    }
}
