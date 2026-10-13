using Comex.Modelos;

namespace Comex.Testes;

// Demonstra a instanciação e a associação solicitadas nos cartões da Semana 2.
public static class TesteObjetos
{
    public static void Executar()
    {
        Produto produto = new Produto("Caderno")
        {
            Descricao = "Caderno de 100 folhas",
            PrecoUnitario = 25.50m,
            Quantidade = 10
        };

        Endereco endereco = new Endereco
        {
            Bairro = "Centro",
            Cidade = "São Paulo",
            Complemento = "Sala 2",
            Estado = "SP",
            Rua = "Rua de Exemplo",
            Numero = 100
        };

        // O terceiro argumento é o próprio objeto Endereco criado acima.
        Cliente cliente = new Cliente("Ana Teste", "11111111111", endereco)
        {
            Email = "ana@example.com",
            Profissao = "Professora",
            Telefone = "11900000000"
        };

        Console.WriteLine("\n=== DEMONSTRAÇÃO DE OBJETOS ===");
        produto.ExibirDados();
        Console.WriteLine();
        cliente.ExibirDados();
        Console.WriteLine("\nDemonstração concluída. Estes objetos não foram adicionados aos cadastros do menu.");
    }
}
