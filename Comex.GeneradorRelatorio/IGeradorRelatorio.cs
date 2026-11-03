namespace Comex.GeneradorRelatorio;

/// <summary>Define a exportação de produtos, independentemente do formato do relatório.</summary>
public interface IGeradorRelatorio
{
    /// <summary>Exporta os produtos para o arquivo indicado.</summary>
    /// <param name="produtos">Produtos na ordem em que devem ser exportados.</param>
    /// <param name="caminhoArquivo">Caminho relativo ou absoluto do arquivo de destino.</param>
    /// <returns>O caminho absoluto do arquivo gerado.</returns>
    string Exportar(IEnumerable<ProdutoRelatorioDto> produtos, string caminhoArquivo);
}
