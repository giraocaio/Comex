using Comex.Modelos;

namespace Comex.Testes;

// Verificações executáveis sem instalar pacotes de teste externos.
public static class TestesDoDominio
{
    public static void Executar()
    {
        int aprovados = 0;

        void Verificar(string nome, Action teste)
        {
            teste();
            aprovados++;
            Console.WriteLine($"OK: {nome}");
        }

        Verificar("Produto não possui construtor sem nome", () =>
            Exigir(typeof(Produto).GetConstructor(Type.EmptyTypes) == null));
        Verificar("Produto rejeita nome vazio", () =>
            EsperarExcecao<ArgumentException>(() => { _ = new Produto(""); }));
        Verificar("Produto rejeita espaços no nome", () =>
            EsperarExcecao<ArgumentException>(() => { _ = new Produto("   "); }));
        Verificar("Produto rejeita nome nulo", () =>
            EsperarExcecao<ArgumentException>(() => { _ = new Produto(null!); }));

        Produto produto = new Produto(" Caderno ")
        {
            Descricao = "100 folhas", PrecoUnitario = 29.90m, Quantidade = 10
        };
        Verificar("Produto guarda suas quatro propriedades", () =>
            Exigir(produto.Nome == "Caderno" && produto.Descricao == "100 folhas" &&
                produto.PrecoUnitario == 29.90m && produto.Quantidade == 10));
        Verificar("Preço negativo não modifica o objeto", () =>
        {
            EsperarExcecao<ArgumentOutOfRangeException>(() => produto.PrecoUnitario = -1);
            Exigir(produto.PrecoUnitario == 29.90m);
        });
        Verificar("Quantidade negativa não modifica o objeto", () =>
        {
            EsperarExcecao<ArgumentOutOfRangeException>(() => produto.Quantidade = -1);
            Exigir(produto.Quantidade == 10);
        });

        Endereco endereco = new Endereco
        {
            Rua = "Rua de Exemplo", Numero = 100, Bairro = "Centro",
            Cidade = "São Paulo", Estado = "SP", Complemento = "Sala 2"
        };
        Cliente ana = new Cliente("Ana Teste", "11111111111", endereco)
        {
            Email = "ana@example.com", Profissao = "Professora", Telefone = "11900000000"
        };
        Cliente bruno = new Cliente("Bruno Teste", "22222222222", new Endereco());
        Verificar("Cliente guarda as propriedades e a referência ao endereço", () =>
            Exigir(ana.Nome == "Ana Teste" && ana.CPF == "11111111111" &&
                ana.Email == "ana@example.com" && ana.Profissao == "Professora" &&
                ana.Telefone == "11900000000" && ReferenceEquals(ana.Endereco, endereco)));
        Verificar("Endereço guarda suas seis propriedades", () =>
            Exigir(endereco.Rua == "Rua de Exemplo" && endereco.Numero == 100 &&
                endereco.Bairro == "Centro" && endereco.Cidade == "São Paulo" &&
                endereco.Estado == "SP" && endereco.Complemento == "Sala 2"));
        Verificar("Cliente rejeita endereço nulo", () =>
            EsperarExcecao<ArgumentNullException>(() => { _ = new Cliente("Ana", "11111111111", null!); }));
        Verificar("Endereço rejeita número negativo", () =>
            EsperarExcecao<ArgumentOutOfRangeException>(() => endereco.Numero = -1));
        Verificar("Carrinho vazio tem total zero", () =>
            Exigir(ana.CalcularTotalDoCarrinho() == 0));
        Verificar("Carrinho rejeita produto nulo", () =>
            EsperarExcecao<ArgumentNullException>(() => ana.AdicionarProdutoAoCarrinho(null!)));

        ana.AdicionarProdutoAoCarrinho(produto);
        ana.AdicionarProdutoAoCarrinho(produto);
        Verificar("Duas unidades somam os preços", () =>
            Exigir(ana.Carrinho.Count == 2 && ana.CalcularTotalDoCarrinho() == 59.80m));
        Verificar("Carrinhos dos clientes são independentes", () =>
            Exigir(bruno.Carrinho.Count == 0 && bruno.CalcularTotalDoCarrinho() == 0));
        Verificar("Alteração do preço é refletida no carrinho", () =>
        {
            produto.PrecoUnitario = 25.50m;
            Exigir(ana.CalcularTotalDoCarrinho() == 51.00m);
        });
        Verificar("Consulta do carrinho não permite alteração externa", () =>
            EsperarExcecao<NotSupportedException>(() => ((IList<Produto>)ana.Carrinho).Clear()));
        Verificar("Esvaziar carrinho não altera o catálogo", () =>
        {
            ana.EsvaziarCarrinho();
            Exigir(ana.Carrinho.Count == 0 && produto.PrecoUnitario == 25.50m && produto.Quantidade == 10);
        });

        Console.WriteLine($"\n{aprovados} testes de domínio aprovados.");
    }

    private static void Exigir(bool condicao)
    {
        if (!condicao)
            throw new InvalidOperationException("O resultado não corresponde ao esperado.");
    }

    private static void EsperarExcecao<T>(Action acao) where T : Exception
    {
        try { acao(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Era esperada uma exceção {typeof(T).Name}.");
    }
}
