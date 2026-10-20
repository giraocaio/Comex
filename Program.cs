using System.Text;
using Comex.Menus;
using Comex.Testes;

Console.OutputEncoding = Encoding.UTF8;

if (args.Contains("--testes"))
{
    TestesDoDominio.Executar();
    TestesSemana3.Executar();
    TestesDoConsole.Executar();
    return;
}

MenuPrincipal menu = new MenuPrincipal();
menu.ExibirMenuDeOpcoes();
