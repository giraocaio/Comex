using Comex.Menus;
using Comex.Servicos;

namespace Comex.Testes;

public static class DemonstracaoApi
{
    // Dados fictícios com o mesmo formato esperado da Fake Store API.
    internal const string JsonExemplo = """
        [
          { "id": 1, "title": "Zebra de brinquedo", "price": 2,
            "description": "Produto fictício para demonstração" },
          { "id": 2, "title": "Livro de C#", "price": 10,
            "description": "Livro fictício", "category": "books" },
          { "id": 3, "title": "Abajur", "price": 10,
            "description": "Abajur fictício", "rating": { "rate": 4.5, "count": 10 } }
        ]
        """;

    public static async Task ExecutarAsync()
    {
        Console.WriteLine("\n=== DEMONSTRAÇÃO OFFLINE: RESPOSTA HTTP SIMULADA ===");
        Console.WriteLine("Dados fictícios. Esta demonstração não acessa a API real.");
        using HttpClient cliente = new HttpClient(new RespostaHttpSimulada((_, _) =>
            Task.FromResult(RespostaHttpSimulada.CriarJson(JsonExemplo))));
        MenuConsultaApi menu = new MenuConsultaApi(new ProdutosApiServico(cliente));
        await menu.ConsultarAsync();
        menu.ListarPorNome();
        menu.ListarPorPreco();
        Console.WriteLine("\nDemonstração offline concluída. A consulta real e o catálogo local não foram alterados.");
    }
}
