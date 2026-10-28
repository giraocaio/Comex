using System.Text.Json.Serialization;

namespace Comex.Modelos;

// Mapeia apenas os campos necessários da resposta externa.
public class ProdutoApi
{
    [JsonRequired]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonRequired]
    [JsonPropertyName("title")]
    public string Nome { get; set; } = "";

    [JsonRequired]
    [JsonPropertyName("price")]
    public decimal PrecoUnitario { get; set; }

    [JsonPropertyName("description")]
    public string Descricao { get; set; } = "";
}
