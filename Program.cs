using System.Text;
using Comex.Menus;
using Comex.Testes;

Console.OutputEncoding = Encoding.UTF8;

if (args.Contains("--testes"))
{
    using DiretorioTemporario pastaTestes = new DiretorioTemporario(alterarDiretorio: true);
    TestesDoDominio.Executar();
    TestesSemana3.Executar();
    TestesDoConsole.Executar();
    await TestesSemana4.ExecutarAsync();
    await TestesSemana5.ExecutarAsync();
    return;
}

Dictionary<int, Menu> menus = MenuPrincipal.CriarMenus();
menus.Add(18, new MenuGerarRelatorioCsv());
MenuPrincipal menu = new MenuPrincipal(menus);
await menu.ExibirMenuDeOpcoesAsync();
