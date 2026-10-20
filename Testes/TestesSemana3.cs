using Comex.Interfaces;
using Comex.Modelos;
using Comex.Servicos;

namespace Comex.Testes;

public static class TestesSemana3
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

        Eletronico eletronico = new Eletronico("Ventilador", 127, 60.5m)
        {
            Descricao = "Mesa", PrecoUnitario = 150, Quantidade = 5
        };
        Livro livro = new Livro("Aprendendo C#", " 9780000000002 ", 200)
        {
            Descricao = "Didático", PrecoUnitario = 50, Quantidade = 10
        };
        Cliente cliente = new Cliente("Ana", "11111111111", new Endereco());

        Verificar("Eletrônico herda os dados de Produto e guarda seus campos", () =>
        {
            Produto baseProduto = eletronico;
            Exigir(baseProduto.Nome == "Ventilador" && baseProduto.Descricao == "Mesa" &&
                baseProduto.PrecoUnitario == 150 && baseProduto.Quantidade == 5 &&
                eletronico.Voltagem == 127 && eletronico.Potencia == 60.5m);
        });
        Verificar("Livro herda os dados de Produto e guarda ISBN e páginas", () =>
        {
            Produto baseProduto = livro;
            Exigir(baseProduto.Nome == "Aprendendo C#" && baseProduto.Descricao == "Didático" &&
                baseProduto.PrecoUnitario == 50 && baseProduto.Quantidade == 10 &&
                livro.Isbn == "9780000000002" && livro.TotalPaginas == 200);
        });
        Verificar("Interface identifica o cliente com nome e CPF", () =>
        {
            IIdentificavel item = cliente;
            Exigir(item.Identificar() == "Nome: Ana | CPF: 11111111111");
        });
        Verificar("Interface identifica o livro com nome e ISBN", () =>
        {
            IIdentificavel item = livro;
            Exigir(item.Identificar() == "Nome: Aprendendo C# | ISBN: 9780000000002");
        });
        Verificar("Subclasses exigem nome válido", () =>
        {
            EsperarExcecao<ArgumentException>(() => { _ = new Livro("", "ISBN", 10); });
            EsperarExcecao<ArgumentException>(() => { _ = new Eletronico(" ", 127, 60); });
        });
        Verificar("Livro rejeita ISBN vazio ou nulo", () =>
        {
            EsperarExcecao<ArgumentException>(() => { _ = new Livro("Livro", " ", 10); });
            EsperarExcecao<ArgumentException>(() => { _ = new Livro("Livro", null!, 10); });
        });
        Verificar("Livro rejeita zero ou páginas negativas", () =>
        {
            EsperarExcecao<ArgumentOutOfRangeException>(() => { _ = new Livro("Livro", "ISBN", 0); });
            EsperarExcecao<ArgumentOutOfRangeException>(() => { _ = new Livro("Livro", "ISBN", -1); });
        });
        Verificar("Eletrônico rejeita voltagem não positiva", () =>
        {
            EsperarExcecao<ArgumentOutOfRangeException>(() => { _ = new Eletronico("TV", 0, 60); });
            EsperarExcecao<ArgumentOutOfRangeException>(() => { _ = new Eletronico("TV", -127, 60); });
        });
        Verificar("Eletrônico rejeita potência não positiva", () =>
        {
            EsperarExcecao<ArgumentOutOfRangeException>(() => { _ = new Eletronico("TV", 127, 0); });
            EsperarExcecao<ArgumentOutOfRangeException>(() => { _ = new Eletronico("TV", 127, -1); });
        });
        Verificar("Carrinho aceita os dois subtipos e considera reajuste", () =>
        {
            cliente.AdicionarProdutoAoCarrinho(livro);
            cliente.AdicionarProdutoAoCarrinho(eletronico);
            Exigir(cliente.CalcularTotalDoCarrinho() == 200);
            livro.PrecoUnitario = 40;
            Exigir(cliente.CalcularTotalDoCarrinho() == 190);
        });

        CatalogoProdutos catalogo = new CatalogoProdutos();
        Produto z = new Produto("Zebra") { PrecoUnitario = 2 };
        Livro b = new Livro("beta", "ISBN-B", 10) { PrecoUnitario = 10 };
        Eletronico a = new Eletronico("Álbum", 220, 1) { PrecoUnitario = 10 };
        Produto c = new Produto("Caderno") { PrecoUnitario = 100 };
        catalogo.Cadastrar(z);
        catalogo.Cadastrar(b);
        catalogo.Cadastrar(a);
        catalogo.Cadastrar(c);

        Verificar("Catálogo rejeita duplicidade entre tipos sem substituir objeto", () =>
            Exigir(!catalogo.Cadastrar(new Produto("BETA")) && ReferenceEquals(catalogo.Buscar("beta"), b)));
        Verificar("Busca ignora caixa e espaços externos", () =>
            Exigir(ReferenceEquals(catalogo.Buscar(" BETA "), b)));
        Verificar("Busca inexistente retorna nulo", () => Exigir(catalogo.Buscar("Ausente") == null));
        Verificar("Catálogo rejeita objeto nulo", () =>
            EsperarExcecao<ArgumentNullException>(() => catalogo.Cadastrar(null!)));
        Verificar("LINQ ordena nomes com acento e caixa segundo pt-BR", () =>
            Exigir(catalogo.OrdenarPorNome().SequenceEqual(new Produto[] { a, b, c, z })));
        Verificar("LINQ ordena preço numericamente e desempata pelo nome", () =>
            Exigir(catalogo.OrdenarPorPreco().SequenceEqual(new Produto[] { z, a, b, c })));
        Verificar("Ordenação preserva o catálogo original e suas referências", () =>
        {
            catalogo.OrdenarPorNome();
            catalogo.OrdenarPorPreco();
            Exigir(catalogo.Listar().SequenceEqual(new Produto[] { z, b, a, c }));
        });
        Verificar("Ordenação considera preço atualizado", () =>
        {
            c.PrecoUnitario = 1;
            Exigir(ReferenceEquals(catalogo.OrdenarPorPreco()[0], c));
        });
        Verificar("Catálogo vazio pode ser ordenado", () =>
        {
            CatalogoProdutos vazio = new CatalogoProdutos();
            Exigir(vazio.OrdenarPorNome().Count == 0 && vazio.OrdenarPorPreco().Count == 0);
        });

        Console.WriteLine($"\n{aprovados} testes da Semana 3 aprovados.");
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
