namespace Comex.Menus;

// Contrato comum: o menu principal executa qualquer opção por meio desta classe base.
public abstract class Menu
{
    public string Titulo { get; }

    protected Menu(string titulo)
    {
        Titulo = titulo;
    }

    public abstract Task ExecutarAsync();
}
