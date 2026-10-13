using System.Globalization;
using System.Text;
using Comex.Modelos;
using Comex.Testes;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo culturaBrasileira = new CultureInfo("pt-BR");

// As chaves continuam iguais, mas os valores agora são objetos completos.
Dictionary<string, Cliente> clientes = new Dictionary<string, Cliente>();
Dictionary<string, Produto> produtos = new Dictionary<string, Produto>(StringComparer.OrdinalIgnoreCase);

if (args.Contains("--testes"))
{
    TestesDoDominio.Executar();
    return;
}

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
        Console.WriteLine("8 - Demonstrar objetos da Semana 2");
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
            case 8: TesteObjetos.Executar(); break;
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

    string email = LerTexto("E-mail: ");
    string profissao = LerTexto("Profissão: ");
    string telefone = LerTexto("Telefone: ");
    string rua = LerTexto("Rua: ");
    if (!LerInteiroNaoNegativo("Número do endereço (0 para sem número): ", out int numero))
        return;
    string bairro = LerTexto("Bairro: ");
    string cidade = LerTexto("Cidade: ");
    string estado = LerTexto("Estado: ");
    string complemento = LerTexto("Complemento (opcional): ");

    if (email.Length == 0 || profissao.Length == 0 || telefone.Length == 0 ||
        rua.Length == 0 || bairro.Length == 0 || cidade.Length == 0 || estado.Length == 0)
    {
        Console.WriteLine("Preencha todos os campos obrigatórios. Apenas o complemento é opcional.");
        return;
    }

    Endereco endereco = new Endereco
    {
        Rua = rua,
        Numero = numero,
        Bairro = bairro,
        Cidade = cidade,
        Estado = estado,
        Complemento = complemento
    };
    Cliente cliente = new Cliente(nome, cpf, endereco)
    {
        Email = email,
        Profissao = profissao,
        Telefone = telefone
    };
    clientes.Add(cpf, cliente);
    Console.WriteLine($"Usuário {nome} ({cpf}) cadastrado com sucesso.");
}

void ListaClientes()
{
    if (clientes.Count == 0)
    {
        Console.WriteLine("Nenhum cliente cadastrado.");
        return;
    }
    foreach (Cliente cliente in clientes.Values)
    {
        cliente.ExibirDados();
        Console.WriteLine();
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
    string descricao = LerTexto("Descrição do produto: ");
    if (descricao.Length == 0)
    {
        Console.WriteLine("A descrição não pode ficar vazia.");
        return;
    }
    if (!LerPreco("Preço unitário (ex.: 29,90): ", out decimal preco))
        return;
    if (!LerInteiroNaoNegativo("Quantidade cadastrada: ", out int quantidade))
        return;

    Produto produto = new Produto(nome)
    {
        Descricao = descricao,
        PrecoUnitario = preco,
        Quantidade = quantidade
    };
    produtos.Add(produto.Nome, produto);
    Console.WriteLine($"Produto {nome} cadastrado com sucesso.");
}

void ListaProdutos()
{
    if (produtos.Count == 0)
    {
        Console.WriteLine("Nenhum produto cadastrado.");
        return;
    }
    foreach (Produto produto in produtos.Values)
    {
        produto.ExibirDados();
        Console.WriteLine();
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

    produtos[nome].PrecoUnitario = preco;
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

    clientes[cpf].AdicionarProdutoAoCarrinho(produtos[nome]);
    Console.WriteLine($"Produto {produtos[nome].Nome} adicionado ao carrinho de {clientes[cpf].Nome} com sucesso.");
}

void FechaCompra()
{
    string cpf = LerCpf();
    if (!clientes.ContainsKey(cpf))
    {
        Console.WriteLine("Cliente não cadastrado.");
        return;
    }
    Cliente cliente = clientes[cpf];
    if (cliente.Carrinho.Count == 0)
    {
        Console.WriteLine("O carrinho está vazio.");
        return;
    }

    Console.WriteLine($"\nCarrinho de {cliente.Nome}:");
    // A compra considera o preço atual do catálogo no momento do fechamento.
    foreach (Produto produto in cliente.Carrinho)
    {
        Console.WriteLine($"{produto.Nome}: {Moeda(produto.PrecoUnitario)}");
    }
    decimal total = cliente.CalcularTotalDoCarrinho();
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
    cliente.EsvaziarCarrinho();
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

bool LerInteiroNaoNegativo(string mensagem, out int numero)
{
    string entrada = LerTexto(mensagem);
    if (!int.TryParse(entrada, out numero) || numero < 0)
    {
        Console.WriteLine("Valor inválido. Informe um número inteiro maior ou igual a zero.");
        return false;
    }
    return true;
}

string Moeda(decimal valor)
{
    return valor.ToString("C2", culturaBrasileira);
}
