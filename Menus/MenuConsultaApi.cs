using System.Globalization;
using System.Text.Json;
using Comex.Modelos;
using Comex.Servicos;

namespace Comex.Menus;

public class MenuConsultaApi
{
    private readonly ProdutosApiServico servico;
    private IReadOnlyList<ProdutoApi>? ultimaConsulta;

    public MenuConsultaApi(ProdutosApiServico servico)
    {
        this.servico = servico;
    }

    public async Task ConsultarAsync()
    {
        Console.WriteLine("\nIniciando consulta de produtos...");
        try
        {
            // Só substitui a consulta anterior após receber e validar toda a resposta.
            List<ProdutoApi> produtos = await servico.ConsultarAsync();
            ultimaConsulta = produtos;
            Console.WriteLine($"Consulta concluída: {produtos.Count} produto(s) recebido(s).");
            ExibirProdutos(produtos);
        }
        catch (HttpRequestException erro)
        {
            if (erro.StatusCode.HasValue)
                Console.WriteLine($"Não foi possível consultar a API: HTTP {(int)erro.StatusCode.Value}.");
            else
                Console.WriteLine("Não foi possível conectar à API. Verifique a conexão e tente novamente.");
            InformarConsultaAnterior();
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("A consulta excedeu o tempo limite ou foi cancelada. Tente novamente.");
            InformarConsultaAnterior();
        }
        catch (JsonException)
        {
            Console.WriteLine("A API retornou um JSON inválido ou dados de produtos incompatíveis.");
            InformarConsultaAnterior();
        }
    }

    public void ListarPorNome()
    {
        if (!TemConsulta())
            return;
        Console.WriteLine("\n=== Produtos externos por nome (A a Z) ===");
        ExibirProdutos(ProdutosApiServico.OrdenarPorNome(ultimaConsulta!));
    }

    public void ListarPorPreco()
    {
        if (!TemConsulta())
            return;
        Console.WriteLine("\n=== Produtos externos por preço (menor para maior) ===");
        ExibirProdutos(ProdutosApiServico.OrdenarPorPreco(ultimaConsulta!));
    }

    private bool TemConsulta()
    {
        if (ultimaConsulta != null)
            return true;
        Console.WriteLine("Consulte a API pela opção 14 antes de ordenar os produtos externos.");
        return false;
    }

    private void InformarConsultaAnterior()
    {
        if (ultimaConsulta != null)
            Console.WriteLine("A última consulta bem-sucedida foi mantida para as opções 15 e 16.");
    }

    private static void ExibirProdutos(IEnumerable<ProdutoApi> produtos)
    {
        Console.WriteLine("Preços conforme a API, sem conversão de moeda.");
        bool encontrouProduto = false;
        foreach (ProdutoApi produto in produtos)
        {
            encontrouProduto = true;
            Console.WriteLine($"\nID: {produto.Id} | Nome: {produto.Nome}");
            Console.WriteLine($"Descrição: {produto.Descricao}");
            Console.WriteLine($"Preço: {produto.PrecoUnitario.ToString("0.00", CultureInfo.GetCultureInfo("pt-BR"))}");
        }
        if (!encontrouProduto)
            Console.WriteLine("A consulta não retornou produtos.");
    }
}
