using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo culturaBrasileira = new CultureInfo("pt-BR");

// CPF -> nome do cliente; nome do produto -> preço.
Dictionary<string, string> clientes = new Dictionary<string, string>();
Dictionary<string, decimal> produtos = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
// Cada CPF tem sua própria lista. Um produto repetido representa outra unidade.
Dictionary<string, List<string>> carrinhos = new Dictionary<string, List<string>>();

ExibirMenuDeOpcoes();

void ExibirMenuDeOpcoes()
{
    // O loop retorna ao menu sem empilhar chamadas da mesma função.
    while (true)
    {
        Console.WriteLine("\n=== COMEX ===");
        Console.WriteLine("1 - Cadastrar cliente");
        Console.WriteLine("2 - Listar clientes");
        Console.WriteLine("3 - Cadastrar produto");
        Console.WriteLine("4 - Listar produtos");
        Console.WriteLine("5 - Ajustar preço de produto");
        Console.WriteLine("6 - Adicionar produto no carrinho");
        Console.WriteLine("7 - Fechar compra");
        Console.WriteLine("-1 - Sair");
        Console.Write("Digite sua opção: ");
        string? entrada = Console.ReadLine();

        if (entrada == null)
        {
            Console.WriteLine("COMEX FINALIZADO!");
            return;
        }

        Console.WriteLine($"Opção escolhida: {entrada}");
        if (!int.TryParse(entrada, out int opcao))
        {
            Console.WriteLine("Opção inválida. Digite um número do menu.");
            continue;
        }

        switch (opcao)
        {
            case 1: CadastraCliente(); break;
            case 2: ListaClientes(); break;
            case 3: CadastraProduto(); break;
            case 4: ListaProdutos(); break;
            case 5: AjustarPrecoDeProduto(); break;
            case 6: AdicionaProdutoNoCarrinho(); break;
            case 7: FechaCompra(); break;
            case -1:
                Console.WriteLine("COMEX FINALIZADO!");
                return;
            default:
                Console.WriteLine("Opção inválida. Escolha uma opção do menu.");
                break;
        }
    }
}

void CadastraCliente()
{
    string cpf = LerCpf();
    if (cpf.Length != 11 || !SomenteDigitos(cpf))
    {
        Console.WriteLine("CPF inválido. Informe 11 dígitos.");
        return;
    }
    if (clientes.ContainsKey(cpf))
    {
        Console.WriteLine("CPF já cadastrado.");
        return;
    }

    string nome = LerTexto("Nome do cliente: ");
    if (nome.Length == 0)
    {
        Console.WriteLine("O nome não pode ficar vazio.");
        return;
    }

    clientes.Add(cpf, nome);
    carrinhos.Add(cpf, new List<string>());
    Console.WriteLine($"Usuário {nome} ({cpf}) cadastrado com sucesso.");
}

void ListaClientes()
{
    if (clientes.Count == 0)
    {
        Console.WriteLine("Nenhum cliente cadastrado.");
        return;
    }
    foreach (var cliente in clientes)
    {
        Console.WriteLine($"CPF: {cliente.Key} | Nome: {cliente.Value}");
    }
}

void CadastraProduto()
{
    string nome = LerTexto("Nome do produto: ");
    if (nome.Length == 0)
    {
        Console.WriteLine("O nome não pode ficar vazio.");
        return;
    }
    if (produtos.ContainsKey(nome))
    {
        Console.WriteLine("Produto já cadastrado. Use a opção de ajustar preço.");
        return;
    }
    if (!LerPreco("Preço do produto (ex.: 29,90): ", out decimal preco))
        return;

    produtos.Add(nome, preco);
    Console.WriteLine($"Produto {nome} cadastrado com sucesso.");
}

void ListaProdutos()
{
    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }
    foreach (var produto in produtos)
    {
        Console.WriteLine($"Produto: {produto.Key} | Preço: {Moeda(produto.Value)}");
    }
}

void AjustarPrecoDeProduto()
{
    string nome = LerTexto("Qual produto deseja ajustar? ");
    if (!produtos.ContainsKey(nome))
    {
        Console.WriteLine("Produto não cadastrado no sistema.");
        return;
    }
    if (!LerPreco("Novo preço (ex.: 29,90): ", out decimal preco))
        return;

    produtos[nome] = preco;
    Console.WriteLine($"Preço do produto {nome} ajustado para {Moeda(preco)}.");
}

void AdicionaProdutoNoCarrinho()
{
    string cpf = LerCpf();
    if (!clientes.ContainsKey(cpf))
    {
        Console.WriteLine("Usuário não cadastrado.");
        return;
    }
    string nome = LerTexto("Qual produto deseja adicionar? ");
    if (!produtos.ContainsKey(nome))
    {
        Console.WriteLine("Produto não cadastrado.");
        return;
    }

    carrinhos[cpf].Add(nome);
    Console.WriteLine($"Produto {nome} adicionado ao carrinho de {clientes[cpf]} com sucesso.");
}

void FechaCompra()
{
    string cpf = LerCpf();
    if (!clientes.ContainsKey(cpf))
    {
        Console.WriteLine("Cliente não cadastrado.");
        return;
    }
    List<string> carrinho = carrinhos[cpf];
    if (carrinho.Count == 0)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    decimal total = 0;
    Console.WriteLine($"\nCarrinho de {clientes[cpf]}:");
    // A compra considera o preço atual do catálogo no momento do fechamento.
    foreach (string nome in carrinho)
    {
        decimal preco = produtos[nome];
        Console.WriteLine($"{nome}: {Moeda(preco)}");
        total += preco;
    }
    Console.WriteLine($"Total: {Moeda(total)}");
    Console.WriteLine("PAGAMENTO SIMULADO: use somente um número de cartão fictício.");
    string cartao = LerTexto("Número fictício do cartão (16 dígitos, ou Enter para cancelar): ")
        .Replace(" ", "").Replace("-", "");
    if (cartao.Length == 0)
    {
        Console.WriteLine("Compra cancelada. Os produtos continuam no carrinho.");
        return;
    }
    if (cartao.Length != 16 || !SomenteDigitos(cartao))
    {
        Console.WriteLine("Número inválido. Use 16 dígitos fictícios. O carrinho foi mantido.");
        return;
    }

    Console.WriteLine($"Realizando pagamento de {Moeda(total)} no cartão {cartao}.");
    Console.WriteLine("Compra finalizada com sucesso (simulação, sem cobrança real).");
    carrinho.Clear();
}

string LerTexto(string mensagem)
{
    Console.Write(mensagem);
    return Console.ReadLine()?.Trim() ?? "";
}

string LerCpf()
{
    return LerTexto("CPF do cliente: ").Replace(".", "").Replace("-", "");
}

bool SomenteDigitos(string texto)
{
    foreach (char caractere in texto)
    {
        if (caractere < '0' || caractere > '9')
            return false;
    }
    return texto.Length > 0;
}

bool LerPreco(string mensagem, out decimal preco)
{
    string entrada = LerTexto(mensagem).Replace(',', '.');
    // Sem separador de milhar; aceita vírgula ou ponto para os centavos.
    bool valido = decimal.TryParse(entrada, NumberStyles.AllowDecimalPoint,
        CultureInfo.InvariantCulture, out preco);
    if (!valido || preco <= 0 || preco > 1000000 || decimal.Round(preco, 2) != preco)
    {
        Console.WriteLine("Preço inválido. Use um valor entre 0,01 e 1000000,00, com até 2 casas decimais e sem separador de milhar.");
        return false;
    }
    return true;
}

string Moeda(decimal valor)
{
    return valor.ToString("C2", culturaBrasileira);
}
