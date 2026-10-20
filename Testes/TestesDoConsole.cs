using Comex.Menus;

namespace Comex.Testes;

// Executa o menu real com entrada e saída redirecionadas, sem interação manual.
public static class TestesDoConsole
{
    public static void Executar()
    {
        int aprovados = 0;
        string Verificar(string nome, string entrada, params string[] mensagens)
        {
            TextReader entradaOriginal = Console.In;
            TextWriter saidaOriginal = Console.Out;
            using StringReader leitor = new StringReader(entrada);
            using StringWriter escritor = new StringWriter();
            try
            {
                Console.SetIn(leitor);
                Console.SetOut(escritor);
                new MenuPrincipal().ExibirMenuDeOpcoes();
            }
            finally
            {
                Console.SetIn(entradaOriginal);
                Console.SetOut(saidaOriginal);
            }
            string saida = escritor.ToString().Replace('\u00a0', ' ');
            foreach (string mensagem in mensagens)
                Exigir(saida.Contains(mensagem), $"{nome}: mensagem ausente: {mensagem}");
            Exigir(saida.Split("COMEX FINALIZADO!").Length == 2, $"{nome}: encerramento incorreto.");
            aprovados++;
            Console.WriteLine($"OK: {nome}");
            return saida;
        }

        string cliente = "1\n11111111111\nAna\nana@example.com\nProfessora\n11900000000\nRua Exemplo\n100\nCentro\nSão Paulo\nSP\nSala 2\n";
        string livro = "10\nAprendendo C#\nDidático\n50\n10\n9780000000002\n200\n";
        string eletronico = "9\nVentilador\nDe mesa\n150\n5\n127\n60,5\n";
        string comum = "3\nCaderno\n100 folhas\n25\n20\n";

        Verificar("Menu inválido e saída", "abc\n99\n-1\n", "Opção inválida.");
        Verificar("Consultas vazias", "2\n4\n11\n12\n13\n-1\n",
            "Nenhum cliente cadastrado.", "Nenhum produto cadastrado.", "Nenhum cliente ou livro cadastrado para identificar.");
        Verificar("Cliente com endereço e identificação", cliente + "2\n11\n-1\n",
            "Usuário Ana (11111111111) cadastrado com sucesso.", "Rua Exemplo, 100", "Nome: Ana | CPF: 11111111111");
        Verificar("CPF duplicado normalizado", cliente + "1\n111.111.111-11\n-1\n", "CPF já cadastrado.");
        Verificar("CPF inválido", "1\n123\n-1\n", "CPF inválido.");
        Verificar("Listagem polimórfica dos três tipos", eletronico + livro + comum + "4\n-1\n",
            "Voltagem: 127 V", "Potência: 60,5 W", "ISBN: 9780000000002", "Total de páginas: 200", "Produto: Caderno");
        Verificar("Identificação dos cadastros pela interface", cliente + livro + "11\n-1\n",
            "Nome: Ana | CPF: 11111111111", "Nome: Aprendendo C# | ISBN: 9780000000002");
        Verificar("Demonstração isolada dos cadastros", "8\n4\n11\n-1\n",
            "Voltagem: 127 V", "Total de páginas: 200", "Nome: Ana Teste | CPF: 11111111111",
            "LINQ por nome: Aprendendo C#, Caderno, Ventilador", "LINQ por preço: Caderno, Aprendendo C#, Ventilador",
            "Nenhum produto cadastrado.", "Nenhum cliente ou livro cadastrado para identificar.");

        string ordenadoNome = Verificar("Menu de ordenação por nome", eletronico + comum + livro + "12\n-1\n", "Produtos por nome (A a Z)");
        VerificarOrdem(ordenadoNome, "=== Produtos por nome", "Aprendendo C#", "Caderno", "Ventilador");
        string ordenadoPreco = Verificar("Menu de ordenação por preço", eletronico + livro + comum + "13\n-1\n", "Produtos por preço (menor para maior)");
        VerificarOrdem(ordenadoPreco, "=== Produtos por preço", "Caderno", "Aprendendo C#", "Ventilador");
        Verificar("Duplicidade entre tipos", livro + "3\naprendendo c#\n4\n-1\n",
            "Produto já cadastrado.", "ISBN: 9780000000002");

        string compra = cliente + livro + eletronico +
            "6\n11111111111\naprendendo c#\n6\n11111111111\nVENTILADOR\n5\nAprendendo C#\n25\n";
        Verificar("Carrinho misto, reajuste e pagamento", compra + "7\n11111111111\n0000000000000000\n7\n11111111111\n4\n-1\n",
            "Total: R$ 175,00", "Compra finalizada com sucesso", "O carrinho está vazio.", "Quantidade cadastrada: 10");
        Verificar("Cancelamento e cartão inválido preservam itens", compra + "7\n11111111111\n\n7\n11111111111\n123\n7\n11111111111\n0000000000000000\n-1\n",
            "Compra cancelada.", "Número inválido.", "Total: R$ 175,00", "Compra finalizada com sucesso");
        Verificar("Consultas inexistentes e carrinho vazio", "5\nAusente\n6\n11111111111\n7\n11111111111\n" + cliente + "6\n11111111111\nAusente\n7\n11111111111\n-1\n",
            "Produto não cadastrado no sistema.", "Usuário não cadastrado.", "Cliente não cadastrado.", "Produto não cadastrado.", "O carrinho está vazio.");
        Verificar("ISBN vazio não cadastra livro", "10\nLivro\nDescrição\n10\n1\n\n4\n-1\n", "O ISBN é obrigatório.", "Nenhum produto cadastrado.");
        Verificar("Páginas zero não cadastram livro", "10\nLivro\nDescrição\n10\n1\nISBN\n0\n4\n-1\n", "Informe um número inteiro maior que zero.", "Nenhum produto cadastrado.");
        Verificar("Voltagem negativa não cadastra eletrônico", "9\nTV\nDescrição\n10\n1\n-127\n4\n-1\n", "Informe um número inteiro maior que zero.", "Nenhum produto cadastrado.");
        Verificar("Potência inválida não cadastra eletrônico", "9\nTV\nDescrição\n10\n1\n127\nabc\n4\n-1\n", "Informe um número positivo", "Nenhum produto cadastrado.");
        Verificar("Preço negativo e quantidade fracionária", "3\nCaderno\nDescrição\n-1\n3\nCaderno\nDescrição\n10\n1,5\n4\n-1\n",
            "Preço inválido.", "Informe um número inteiro maior ou igual a zero.", "Nenhum produto cadastrado.");
        Verificar("Fim da entrada durante cadastro", "10\nLivro\nDescrição\n10\n1\n", "O ISBN é obrigatório.");

        Console.WriteLine($"\n{aprovados} cenários de console aprovados.");
    }

    private static void VerificarOrdem(string saida, string titulo, params string[] nomes)
    {
        string lista = saida[saida.IndexOf(titulo, StringComparison.Ordinal)..];
        int anterior = -1;
        foreach (string nome in nomes)
        {
            int posicao = lista.IndexOf("Produto: " + nome, StringComparison.Ordinal);
            Exigir(posicao > anterior, $"Ordem incorreta para {nome}.");
            anterior = posicao;
        }
    }

    private static void Exigir(bool condicao, string mensagem)
    {
        if (!condicao)
            throw new InvalidOperationException(mensagem);
    }
}
