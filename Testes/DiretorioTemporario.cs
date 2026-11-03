namespace Comex.Testes;

// Isola os arquivos de testes e demonstrações para não sobrescrever consultas reais.
internal sealed class DiretorioTemporario : IDisposable
{
    public string Caminho { get; } = Path.Combine(Path.GetTempPath(), "comex-" + Guid.NewGuid().ToString("N"));
    private readonly string? diretorioAnterior;

    public DiretorioTemporario(bool alterarDiretorio = false)
    {
        Directory.CreateDirectory(Caminho);
        if (alterarDiretorio)
        {
            diretorioAnterior = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Caminho);
        }
    }

    public void Dispose()
    {
        if (diretorioAnterior != null)
            Directory.SetCurrentDirectory(diretorioAnterior);
        Directory.Delete(Caminho, recursive: true);
    }
}
