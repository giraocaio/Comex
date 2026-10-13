# Comex | Semana 2 — Orientação a objetos

Aplicação de console em C# para as atividades da Semana 2 do Comex, no treinamento da Alura. Evolui o projeto da Semana 1 com classes, propriedades, construtores, métodos e associação entre objetos.

**Branch de entrega:** `Semana02-CaioGirao`, criada a partir de `semana-1`.

## Requisitos implementados

| Atividade | Implementação |
| --- | --- |
| Criar a classe Produto | [Modelos/Produto.cs](Modelos/Produto.cs): `Nome`, `Descricao`, `PrecoUnitario` e `Quantidade` |
| Instanciar um produto e exibir seus dados | [Testes/TesteObjetos.cs](Testes/TesteObjetos.cs), acessível pela opção **8** do menu |
| Criar a classe Endereco | [Modelos/Endereco.cs](Modelos/Endereco.cs): `Bairro`, `Cidade`, `Complemento`, `Estado`, `Rua` e `Numero` |
| Criar a classe Cliente | [Modelos/Cliente.cs](Modelos/Cliente.cs): `Nome`, `CPF`, `Email`, `Profissao`, `Telefone` e `Endereco` |
| Associar cliente e endereço em uma classe de teste | `TesteObjetos` cria um `Endereco` e passa esse objeto ao construtor de `Cliente` |
| Exigir nome na criação de Produto | Construtor `Produto(string nome)`, sem construtor vazio; rejeita nome nulo, vazio ou só com espaços |
| Continuar o projeto anterior | Cadastros, listagens, ajuste de preço, carrinho e pagamento simulado continuam disponíveis |

Os campos de texto usam `string`; quantidade e número do endereço usam `int`. O preço usa `decimal`, adequado aos valores monetários e mantido da Semana 1.

## Abrir e executar no computador

É necessário ter o **SDK do .NET 8** instalado. O projeto não utiliza pacotes externos.

### Se você já usa GitHub Desktop

1. Selecione o repositório **Comex**.
2. Clique em **Fetch origin** para buscar as branches publicadas.
3. Em **Current branch**, selecione **Semana02-CaioGirao**.
4. Abra a pasta do repositório no VS Code por **File > Open Folder / Arquivo > Abrir Pasta**.
5. Abra **Terminal > New Terminal / Novo Terminal** e execute os comandos abaixo na pasta que contém `comex.csproj`.

### Se preferir baixar os arquivos

1. Abra a [branch da Semana 2](https://github.com/giraocaio/Comex/tree/Semana02-CaioGirao).
2. Clique em **Code > Download ZIP**.
3. Extraia o ZIP antes de abrir o projeto.
4. No VS Code, abra a pasta extraída que contém `comex.csproj` e abra um novo terminal.

```sh
dotnet build comex.csproj
dotnet run --project comex.csproj
```

No Visual Studio, abra `comex.csproj` e pressione **Ctrl+F5**.

Para clonar diretamente com Git:

```sh
git clone --branch Semana02-CaioGirao https://github.com/giraocaio/Comex.git
cd Comex
dotnet run --project comex.csproj
```

## Menu

| Opção | Ação |
| --- | --- |
| 1 | Cadastrar cliente com contato e endereço |
| 2 | Listar todos os dados dos clientes |
| 3 | Cadastrar produto com descrição, preço e quantidade |
| 4 | Listar todos os dados dos produtos |
| 5 | Ajustar preço de produto |
| 6 | Adicionar uma unidade de produto ao carrinho do cliente |
| 7 | Exibir carrinho, calcular total e simular pagamento |
| 8 | Criar e exibir os objetos de demonstração da Semana 2 |
| -1 | Exibir `COMEX FINALIZADO!` e encerrar |

## Como explicar a orientação a objetos

- **Classe:** define os dados e comportamentos. `Produto` é a classe; o caderno criado a partir dela é um objeto.
- **Propriedades:** representam os dados do objeto, como `Descricao` e `Quantidade`.
- **Construtor:** inicializa o objeto. `new Produto("Caderno")` fornece o nome obrigatório. `new Produto()` não compila, pois não existe construtor sem parâmetros.
- **Associação:** a propriedade `Cliente.Endereco` guarda um objeto `Endereco`, com seus próprios campos.
- **Métodos:** representam ações. `ExibirDados()` apresenta o objeto; `CalcularTotalDoCarrinho()` soma os produtos do cliente.
- **Encapsulamento:** o nome do produto só pode ser definido no construtor; preço e quantidade rejeitam valores negativos; o carrinho é alterado pelos métodos do cliente.

Os dicionários continuam no `Program.cs`, mas agora armazenam objetos: `Dictionary<string, Cliente>` e `Dictionary<string, Produto>`. Cada cliente mantém uma lista própria de produtos no carrinho.

Exemplo presente na classe de demonstração:

```csharp
Produto produto = new Produto("Caderno")
{
    Descricao = "Caderno de 100 folhas",
    PrecoUnitario = 25.50m,
    Quantidade = 10
};

produto.ExibirDados();
```

O sufixo `m` indica um literal do tipo `decimal`.

## Roteiro de demonstração para a monitoria

Use dados fictícios. Os dados ficam em memória enquanto o programa está aberto.

1. Escolha **8**. Confira os quatro campos do produto, os dados do cliente e os seis campos do endereço. Os objetos de exemplo não são adicionados aos cadastros do menu.
2. Escolha **1** e preencha, na ordem solicitada:

| Campo | Valor de exemplo |
| --- | --- |
| CPF | `11111111111` |
| Nome | `Ana Teste` |
| E-mail | `ana@example.com` |
| Profissão | `Professora` |
| Telefone | `11900000000` |
| Rua | `Rua de Exemplo` |
| Número | `100` |
| Bairro | `Centro` |
| Cidade | `São Paulo` |
| Estado | `SP` |
| Complemento | `Sala 2` |

3. Escolha **2** e confira todos os dados, incluindo o endereço associado.
4. Escolha **3** e cadastre `Caderno`, descrição `100 folhas`, preço `29,90` e quantidade `10`.
5. Escolha **4** e confira as quatro propriedades do produto.
6. Escolha **5** e ajuste o preço de `Caderno` para `25,50`.
7. Escolha **6**, informe o CPF e o produto. Repita para adicionar duas unidades.
8. Escolha **7**, informe o CPF e confira o total de **R$ 51,00**. Use `0000000000000000` como cartão fictício.
9. Repita **7**: o carrinho deve estar vazio após a simulação bem-sucedida.
10. Digite **-1** e confira a mensagem de encerramento.

Experimente também nome vazio, CPF repetido, produto inexistente, preço negativo, quantidade negativa e letras no menu. O programa deve informar o problema e retornar ao menu.

## Testes executáveis

Para executar as 18 verificações das classes, sem entrar no menu:

```sh
dotnet run --project comex.csproj -- --testes
```

O resultado esperado é **18 testes de domínio aprovados.** O código dessas verificações está em [Testes/TestesDoDominio.cs](Testes/TestesDoDominio.cs). É um executor simples incluído na própria aplicação, sem framework externo de testes.

Também foram executados 27 cenários de console durante a preparação da entrega. Veja os resultados e o limite da verificação do ambiente em [VALIDACAO.md](VALIDACAO.md).

## Regras e limites desta etapa

- Os dados não são gravados em banco ou arquivo e são perdidos ao encerrar.
- O CPF aceita pontuação e exige 11 dígitos; não verifica os dígitos verificadores. E-mail e telefone são obrigatórios no menu, sem validação de formato.
- Apenas o complemento é opcional no cadastro do cliente. Número `0` representa endereço sem número.
- Produto duplicado não é cadastrado. A busca ignora maiúsculas e minúsculas, mas distingue acentos.
- No menu, preços devem ser positivos, até R$ 1.000.000,00, com até duas casas decimais significativas. Aceitam vírgula ou ponto decimal, sem separador de milhar.
- A classe Produto impede preço negativo; um objeto recém-criado pode ter preço zero até receber um valor. A regra de preço positivo é aplicada no cadastro e no ajuste pelo menu.
- `Quantidade` é um dado do cadastro. Esta etapa não controla estoque, reserva ou baixa de unidades ao comprar.
- Cada inclusão no carrinho representa uma unidade. O total usa o preço atual dos objetos do catálogo, inclusive após reajustes.
- O pagamento é simulado. Use somente um número fictício de 16 dígitos. Enter cancela e preserva o carrinho; um número inválido também preserva os itens; uma simulação bem-sucedida esvazia o carrinho.
- Categoria, pedidos persistidos, banco de dados e integração de pagamento ficam fora do escopo desta entrega.

## Referências

- [C#: aplicando a Orientação a Objetos — Alura](https://cursos.alura.com.br/course/csharp-aplicando-orientacao-objetos)
- [Entrega anterior — Semana 1](https://github.com/giraocaio/Comex/tree/semana-1)
