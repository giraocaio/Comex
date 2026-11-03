namespace Comex.GeneradorRelatorio;

/// <summary>Transporta os dados de um produto para a biblioteca de relatórios.</summary>
public class ProdutoRelatorioDto
{
    /// <summary>Obtém ou define o identificador do produto.</summary>
    public int Id { get; set; }

    /// <summary>Obtém ou define o nome do produto.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Obtém ou define a descrição do produto.</summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>Obtém ou define o preço na moeda informada pela origem, sem conversão.</summary>
    public decimal Preco { get; set; }
}
