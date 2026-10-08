# Comex | Semana 1

Aplicação de console em C# para praticar os fundamentos do curso **C#: criando sua primeira aplicação**, da Alura. Implementa as tarefas essenciais e opcionais da primeira semana do projeto Comex.

## Executar

É necessário ter o **SDK do .NET 8** instalado.

No terminal, dentro da pasta do projeto:

```sh
dotnet run --project comex.csproj
```

No Visual Studio, abra `comex.csproj` e pressione **Ctrl+F5**. Para conferir a compilação pelo terminal:

```sh
dotnet build comex.csproj
```

## Funcionalidades

| Opção | Funcionalidade | Função |
| --- | --- | --- |
| 1 | Cadastrar cliente por CPF e nome | `CadastraCliente` |
| 2 | Listar clientes | `ListaClientes` |
| 3 | Cadastrar produto por nome e preço | `CadastraProduto` |
| 4 | Listar produtos e preços | `ListaProdutos` |
| 5 | Ajustar preço de produto | `AjustarPrecoDeProduto` |
| 6 | Adicionar produto ao carrinho de um cliente | `AdicionaProdutoNoCarrinho` |
| 7 | Listar o carrinho, somar valores e simular pagamento | `FechaCompra` |
| -1 | Exibir `COMEX FINALIZADO!` e encerrar | `ExibirMenuDeOpcoes` |

## Estrutura e conceitos

O código está no `Program.cs`, com instruções de nível superior e funções locais, como no projeto de referência do curso.

- `Dictionary<string, string>`: CPF como chave e nome do cliente como valor.
- `Dictionary<string, decimal>`: nome do produto como chave e preço como valor.
- `Dictionary<string, List<string>>`: CPF como chave e lista de produtos do carrinho como valor.
- `while` mantém o menu aberto; `switch` escolhe a função; `foreach` percorre os dados.
- `decimal` representa dinheiro. `TryParse` trata entradas numéricas inválidas sem encerrar a aplicação.
- Funções auxiliares centralizam a leitura de texto, CPF, preço e a formatação em reais.

## Regras desta etapa

- Os dados ficam apenas em memória e são perdidos ao encerrar.
- O CPF aceita pontuação, que é removida antes de salvar e consultar. A validação exige 11 dígitos, mas não calcula dígitos verificadores.
- CPF duplicado, nome vazio e produto duplicado não são cadastrados. A busca de produtos ignora maiúsculas e minúsculas, mas distingue acentos.
- Preços aceitam vírgula ou ponto decimal, sem separador de milhar. Devem ser positivos, até R$ 1.000.000,00, com no máximo duas casas decimais significativas.
- Adicionar um produto duas vezes representa duas unidades. Cada cliente tem um carrinho independente.
- O fechamento usa os preços atuais do catálogo, incluindo ajustes feitos depois de adicionar ao carrinho.
- O pagamento é apenas uma simulação: informe um número fictício de 16 dígitos. Não há cobrança, integração bancária, armazenamento de cartão ou solicitação de CVV.
- Enter no campo do cartão cancela o fechamento e preserva o carrinho. Um número inválido também preserva o carrinho. Após a simulação bem-sucedida, o carrinho é esvaziado.
- Categoria, pedido persistido, estoque e banco de dados ficam fora do escopo dos cartões da Semana 1 fornecidos.

## Roteiro para experimentar

Use apenas dados fictícios:

1. Opção **1**: cadastre o CPF `11111111111` com o nome `Cliente Teste`.
2. Opção **2**: confira o cadastro.
3. Opção **3**: cadastre `Caderno` por `29,90`.
4. Opção **4**: confira o catálogo.
5. Opção **5**: ajuste `Caderno` para `25,50`.
6. Opção **6**: adicione `Caderno` ao CPF cadastrado. Repita para duas unidades.
7. Opção **7**: informe o CPF e confira o total de **R$ 51,00**. Use `0000000000000000` como cartão fictício.
8. Repita a opção **7**: o carrinho deve estar vazio.
9. Digite **-1**: a aplicação deve mostrar `COMEX FINALIZADO!`.

Teste também letras no menu, um CPF repetido, produto inexistente, nome vazio e preço negativo. O programa deve informar o problema e voltar ao menu.

## Referências

- [Curso da Alura](https://cursos.alura.com.br/course/csharp-criando-primeira-aplicacao)
- [Projeto Screen Sound da aula 5](https://github.com/alura-cursos/csharp-01/tree/aula_5)

O Screen Sound é a referência didática. Este repositório contém uma aplicação Comex criada separadamente para o exercício.
