using Comex.Modelos;
using static Comex.Utilitarios.EntradaConsole;

namespace Comex.Menus;

public class MenuClientes
{
    private readonly Dictionary<string, Cliente> clientes;

    public MenuClientes(Dictionary<string, Cliente> clientes)
    {
        this.clientes = clientes;
    }

    public void CadastraCliente()
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

    public void ListaClientes()
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
}
