# Comex | Semana 3

Aplicação de console em C# para o treinamento Alura / SEFAZ. Esta entrega continua a Semana 2 e aplica **herança, interfaces, polimorfismo, organização em namespaces e ordenação com LINQ**.

**Branch:** `Semana3-CaioGirao`, criada a partir de `Semana02-CaioGirao`.

**Link da entrega:** https://github.com/giraocaio/Comex/tree/Semana3-CaioGirao

## Atividades atendidas

| Requisito | Onde conferir |
| --- | --- |
| Criar a branch a partir da Semana 2 | Histórico da branch `Semana3-CaioGirao` |
| Organizar classes em pastas e namespaces | `Modelos`, `Interfaces`, `Menus`, `Servicos`, `Utilitarios` e `Testes` |
| Criar Eletronico e Livro como subclasses de Produto | [Eletronico.cs](Modelos/Eletronico.cs) e [Livro.cs](Modelos/Livro.cs) |
| Eletrônico com Voltagem e Potencia | Cadastro pela opção **9** e listagem pela opção **4** |
| Livro com Isbn e TotalPaginas | Cadastro pela opção **10** e listagem pela opção **4** |
| Interface que obriga a implementação de Identificar | [IIdentificavel.cs](Interfaces/IIdentificavel.cs), implementada por `Cliente` e `Livro` |
| Identificar cliente com nome/CPF e livro com nome/ISBN | Opção **11**, usando uma coleção de `IIdentificavel` |
| Refatorar o menu em classes com responsabilidades próprias | [MenuPrincipal.cs](Menus/MenuPrincipal.cs) e demais classes da pasta `Menus` |
| Desafio: ordenar produtos por nome e preço com LINQ | Opções **12** e **13**, implementadas em [CatalogoProdutos.cs](Servicos/CatalogoProdutos.cs) |

Todos os tipos de produto compartilham o catálogo, o ajuste de preços e o carrinho. O nome do livro é representado pela propriedade `Nome`, herdada de `Produto`.

## Como baixar e executar

É necessário ter o **SDK do .NET 8**. Não há pacotes externos nem banco de dados.

### GitHub Desktop

1. Selecione o repositório **Comex** e clique em **Fetch origin**.
2. Em **Current branch**, selecione **Semana3-CaioGirao**.
3. Abra a pasta do repositório no VS Code.
4. Abra **Terminal > New Terminal / Novo Terminal**, na pasta que contém `comex.csproj`.
5. Execute:

```sh
dotnet build comex.csproj
dotnet run --project comex.csproj
```

### Download ZIP

Abra o [link da branch](https://github.com/giraocaio/Comex/tree/Semana3-CaioGirao), clique em **Code > Download ZIP** e extraia o arquivo. No VS Code, abra a pasta extraída que contém `comex.csproj` e execute os comandos acima.

### Clone pelo terminal

```sh
git clone --branch Semana3-CaioGirao https://github.com/giraocaio/Comex.git
cd Comex
dotnet run --project comex.csproj
```

## Menu

| Opção | Funcionalidade |
| --- | --- |
| 1 | Cadastrar cliente com contato e endereço |
| 2 | Listar clientes |
| 3 | Cadastrar produto comum |
| 4 | Listar produtos com os campos específicos de cada tipo |
| 5 | Ajustar preço de qualquer produto |
| 6 | Adicionar uma unidade ao carrinho de um cliente |
| 7 | Calcular o total e simular pagamento |
| 8 | Demonstrar herança, interface, polimorfismo e LINQ com exemplos |
| 9 | Cadastrar eletrônico |
| 10 | Cadastrar livro |
| 11 | Identificar clientes e livros cadastrados |
| 12 | Listar produtos por nome, de A a Z |
| 13 | Listar produtos por preço, do menor para o maior |
| -1 | Encerrar com `COMEX FINALIZADO!` |

## Organização e responsabilidades

| Pasta / arquivo | Responsabilidade |
| --- | --- |
| `Program.cs` | Iniciar a aplicação ou executar as verificações com `--testes` |
| `Modelos` / `Comex.Modelos` | Representar Produto, Eletronico, Livro, Cliente e Endereco |
| `Interfaces` / `Comex.Interfaces` | Declarar o contrato `IIdentificavel` |
| `Menus` / `Comex.Menus` | Tratar as interações do usuário, divididas por assunto |
| `Servicos` / `Comex.Servicos` | Manter o catálogo, buscar produtos e ordenar com LINQ |
| `Utilitarios` / `Comex.Utilitarios` | Centralizar leitura, validação de entrada e formatação monetária |
| `Testes` / `Comex.Testes` | Demonstrar os objetos e executar verificações das classes e do menu |

`MenuPrincipal` exibe as opções e encaminha as ações. `MenuClientes` cuida do cadastro e da listagem de clientes; `MenuProdutos`, dos produtos e das listagens ordenadas; `MenuCompras`, do carrinho e do pagamento simulado; `MenuIdentificacoes`, da identificação via interface. Eles compartilham os mesmos cadastros em memória.

## Conceitos que podem ser apresentados à monitoria

**Herança:** `Eletronico : Produto` e `Livro : Produto` reutilizam nome, descrição, preço e quantidade. Seus construtores usam `base(nome)` para atender à exigência de nome criada na Semana 2.

**Interface:** `IIdentificavel` declara `string Identificar()`. `Cliente` retorna nome e CPF; `Livro` retorna nome e ISBN. A opção 11 percorre uma `List<IIdentificavel>` e chama o método sem precisar decidir qual implementação executar.

**Polimorfismo:** `Produto.ExibirDados()` é `virtual`. As subclasses usam `override`, chamam `base.ExibirDados()` e acrescentam seus campos. Assim, uma coleção de `Produto` exibe corretamente os dados de cada tipo real.

**LINQ:** `OrderBy` ordena pelo nome ou pelo preço. Na ordenação por preço, `ThenBy` desempata pelo nome. O nome usa comparação em português do Brasil, ignorando diferenças de maiúsculas e minúsculas. `ToList()` materializa o resultado sem reorganizar o catálogo original.

**Encapsulamento:** o dicionário do catálogo é privado. A inclusão ocorre por `Cadastrar`, que rejeita duplicidade sem substituir o produto existente. O carrinho continua pertencendo ao cliente.

## Roteiro de apresentação e testes manuais

### 1. Demonstração rápida

Execute o projeto e escolha **8**. Serão criados um produto comum, um eletrônico, um livro e um cliente. Confira:

- Eletrônico com voltagem e potência.
- Livro com ISBN e total de páginas.
- Identificação de cliente por nome/CPF e de livro por nome/ISBN.
- Ordenação por nome: **Aprendendo C#, Caderno, Ventilador**.
- Ordenação por preço: **Caderno, Aprendendo C#, Ventilador**.

Os objetos dessa demonstração não são adicionados aos cadastros do menu. Para testar a interação completa, faça os cadastros abaixo na mesma execução.

### 2. Cadastro dos três tipos

Preencha primeiro os campos comuns, na ordem: nome, descrição, preço e quantidade. Depois informe os campos específicos, quando solicitados.

| Opção | Nome | Descrição | Preço | Quantidade | Campos específicos |
| --- | --- | --- | --- | --- | --- |
| 9 | Ventilador | De mesa | 150 | 5 | Voltagem: 127; potência: 60,5 |
| 10 | Aprendendo C# | Didático | 50 | 10 | ISBN: 9780000000002; páginas: 200 |
| 3 | Caderno | 100 folhas | 25 | 20 | Nenhum |

O ISBN acima é um identificador de exemplo, sem relação com uma obra real.

Escolha **4** para conferir os campos herdados e específicos. Use **12** e **13** para comparar as duas ordenações. Os preços esperados na opção 13 são **R$ 25,00, R$ 50,00 e R$ 150,00**.

### 3. Cliente e interface

Na opção **1**, cadastre estes dados fictícios, na ordem solicitada:

| Campo | Valor |
| --- | --- |
| CPF | 11111111111 |
| Nome | Ana Teste |
| E-mail | ana@example.com |
| Profissão | Professora |
| Telefone | 11900000000 |
| Rua | Rua de Exemplo |
| Número | 100 |
| Bairro | Centro |
| Cidade | São Paulo |
| Estado | SP |
| Complemento | Sala 2 |

Escolha **11**. O cliente será identificado com nome e CPF, e o livro cadastrado será identificado com nome e ISBN. Produtos comuns e eletrônicos não implementam essa interface.

### 4. Compra com subclasses

1. Na opção **6**, adicione `Aprendendo C#` ao CPF `11111111111`.
2. Repita a opção **6** para adicionar `Ventilador` ao mesmo CPF.
3. Na opção **5**, ajuste `Aprendendo C#` para `25`.
4. Na opção **7**, informe o CPF e confira o total de **R$ 175,00**.
5. Informe `0000000000000000` como cartão fictício.
6. Abra novamente a opção **7**: o carrinho deverá estar vazio.
7. Encerre com **-1**.

### 5. Casos inválidos

Experimente cadastrar um produto com nome já utilizado, ISBN vazio, páginas iguais a zero, voltagem negativa, potência com letras, preço negativo e quantidade fracionária. O programa deve informar o problema, voltar ao menu e não salvar o cadastro incompleto.

Também é possível cancelar a compra com Enter no campo de cartão. O carrinho deve ser preservado.

## Verificações automatizadas

```sh
dotnet run --project comex.csproj -- --testes
```

O comando executa os testes incluídos no projeto, sem instalar framework externo. O resultado esperado é:

```text
18 testes de domínio aprovados.
19 testes da Semana 3 aprovados.
20 cenários de console aprovados.
```

São **57 verificações**, abrangendo regras anteriores, herança, interface, ordenação, cadastros e compra. Os cenários de console redirecionam entrada e saída para executar o próprio `MenuPrincipal` automaticamente. Uma falha interrompe a execução com uma exceção.

A compilação direta com Roslyn e as 57 verificações foram executadas com sucesso. O build completo pela CLI precisa ser confirmado em um ambiente .NET convencional; a limitação do ambiente de preparação está descrita em [VALIDACAO.md](VALIDACAO.md).

## Regras e limites

- Os dados existem apenas em memória e são perdidos ao fechar o programa.
- O CPF exige 11 dígitos e aceita pontuação, sem validar dígitos verificadores. E-mail e telefone exigem preenchimento, sem validar formato.
- Complemento é opcional; número zero representa endereço sem número.
- O catálogo usa o nome como chave, ignorando maiúsculas e minúsculas. Não é possível repetir o nome entre tipos diferentes de produto.
- O ISBN exige texto não vazio, sem validação de formato ou dígito verificador. Páginas e voltagem são inteiros positivos; potência é um decimal positivo em watts.
- No menu, o preço aceita vírgula ou ponto decimal, deve ser positivo, até R$ 1.000.000,00 e ter até duas casas decimais significativas. Não use separador de milhar.
- A quantidade cadastrada é informativa. Não há reserva ou baixa de estoque.
- Cada inclusão no carrinho representa uma unidade. O total considera o preço atual do produto, inclusive após ajustes.
- O pagamento é simulado e usa apenas número fictício de 16 dígitos. Não existe integração bancária. Cancelamento ou número inválido preserva os itens; simulação bem-sucedida esvazia o carrinho.

## Referências

- [Namespaces e using — Microsoft](https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/program-structure/namespaces)
- [Herança — Microsoft](https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/object-oriented/inheritance)
- [OrderBy — Microsoft](https://learn.microsoft.com/pt-br/dotnet/api/system.linq.enumerable.orderby?view=net-8.0)
- [Entrega da Semana 2](https://github.com/giraocaio/Comex/tree/Semana02-CaioGirao)
