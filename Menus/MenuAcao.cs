namespace Comex.Menus;

// Adapta as funções das semanas anteriores ao mesmo contrato dos menus novos.
public class MenuAcao : Menu
{
    private readonly Func<Task> acao;

    public MenuAcao(string titulo, Func<Task> acao) : base(titulo)
    {
        this.acao = acao;
    }

    public MenuAcao(string titulo, Action acao) : this(titulo, () =>
    {
        acao();
        return Task.CompletedTask;
    }) { }

    public override Task ExecutarAsync() => acao();
}
