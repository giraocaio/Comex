# Comex | Semana 4

Aplicação de console em C# do treinamento Alura / SEFAZ. Esta entrega continua a Semana 3 e acrescenta **consulta HTTP a uma API externa, tratamento de exceções, desserialização de JSON e ordenação com LINQ**.

**Branch:** `Semana4-CaioGirao`, criada a partir de `Semana3-CaioGirao`.

**Link para entrega:** https://github.com/giraocaio/Comex/tree/Semana4-CaioGirao

## Atividades atendidas

| Atividade | Implementação |
| --- | --- |
| Criar branch a partir da Semana 3 | Histórico da branch `Semana4-CaioGirao` |
| Consultar API externa usando HttpClient | [ProdutosApiServico.cs](Servicos/ProdutosApiServico.cs), GET em `https://fakestoreapi.com/products` |
| Acrescentar opção ao menu e imprimir o resultado | Opção **14**, que exibe os campos dos produtos recebidos |
| Tratar falhas com try/catch | [MenuConsultaApi.cs](Menus/MenuConsultaApi.cs): erros HTTP/rede, timeout/cancelamento e JSON inválido |
| Desserializar JSON em objetos C# | `JsonSerializer.Deserialize<List<ProdutoApi>>` e [ProdutoApi.cs](Modelos/ProdutoApi.cs) |
| Ordenar produtos por título/nome e preço com LINQ | Opções **15** e **16**, com `OrderBy`, `ThenBy` e `ToList` |

O resultado da API é convertido em objetos e apresentado com ID, nome, descrição e preço. Os campos `category`, `image` e `rating` não precisam ser mapeados para cumprir a atividade.

## Como executar

É necessário ter o **SDK do .NET 8**. Não há pacotes NuGet externos.

No GitHub Desktop, selecione **Comex**, clique em **Fetch origin** e escolha **Semana4-CaioGirao** em **Current branch**. Abra a pasta no VS Code e execute no terminal, dentro da pasta que contém `comex.csproj`:

```sh
dotnet build comex.csproj
dotnet run --project comex.csproj
```

Também é possível abrir o [link da branch](https://github.com/giraocaio/Comex/tree/Semana4-CaioGirao), usar **Code > Download ZIP**, extrair o ZIP e abrir a pasta no VS Code.

Para clonar pelo terminal:

```sh
git clone --branch Semana4-CaioGirao https://github.com/giraocaio/Comex.git
cd Comex
dotnet run --project comex.csproj
```

## Novas opções do menu

| Opção | Ação |
| --- | --- |
| 14 | Consultar a API real e exibir os produtos recebidos |
| 15 | Ordenar a última consulta bem-sucedida por nome, de A a Z |
| 16 | Ordenar a última consulta bem-sucedida por preço, do menor para o maior |
| 17 | Demonstrar o fluxo com resposta HTTP simulada, sem internet |

As opções **15** e **16** usam a lista já recebida, sem fazer novas chamadas HTTP. Para atualizar a lista, use novamente a opção **14**.

A opção **17** é uma demonstração adicional, claramente identificada como offline. Ela utiliza dados fictícios no formato esperado da API e percorre o mesmo serviço de HTTP/desserialização e a mesma lógica de exibição/ordenação, por meio de um `HttpMessageHandler` simulado. Não substitui silenciosamente a consulta real nem modifica seus dados.

## Roteiro para apresentar à monitoria

1. Execute a aplicação e escolha **14**.
2. Se a API responder com sucesso, confira a quantidade recebida e os campos exibidos de cada produto.
3. Escolha **15** para ver os nomes em ordem alfabética.
4. Escolha **16** para ver os preços em ordem crescente. Empates são resolvidos pelo nome e, depois, pelo ID.
5. Volte à opção **14** para realizar outra consulta.
6. Encerre com **-1** e confira a mensagem `COMEX FINALIZADO!`.

Se ocorrer erro HTTP, falta de conexão, timeout ou resposta inválida, a aplicação deve informar o problema e voltar ao menu, sem apresentar uma consulta malsucedida como sucesso.

### Disponibilidade da API e demonstração offline

Na verificação de **28/10/2026**, a chamada real feita pela aplicação retornou **HTTP 403 (Forbidden)** a partir do ambiente de preparação. O erro foi tratado e a aplicação continuou funcionando. Esse resultado não permite afirmar que a consulta real retornou produtos neste ambiente, nem prever a resposta na rede do professor.

Se a API também estiver indisponível na apresentação, a opção **17** permite demonstrar a desserialização e as duas ordenações com três produtos fictícios:

| Produto | Preço informado |
| --- | --- |
| Zebra de brinquedo | 2,00 |
| Livro de C# | 10,00 |
| Abajur | 10,00 |

A ordem por nome será **Abajur, Livro de C#, Zebra de brinquedo**. A ordem por preço será **Zebra de brinquedo, Abajur, Livro de C#**. O empate de preço evidencia o uso de `ThenBy`.

A demonstração offline não preenche a lista da consulta real. Se nenhuma consulta real tiver sido concluída, as opções 15 e 16 continuarão orientando a usar a opção 14.

## Como explicar o código

**HttpClient e async/await:** `ProdutosApiServico.ConsultarAsync` envia um GET para o endereço solicitado. O cliente HTTP padrão é compartilhado e tem timeout de 15 segundos. `await` aguarda a resposta antes de processar os dados e retornar ao menu.

**Status HTTP:** `EnsureSuccessStatusCode()` transforma uma resposta HTTP de erro em `HttpRequestException`. Assim, uma página de erro HTTP não é tratada como uma lista de produtos.

**Tratamento de exceções:** o `try/catch` no menu envolve a consulta, a desserialização e a apresentação. Captura `HttpRequestException`, `OperationCanceledException` e `JsonException`, mostrando mensagens adequadas para cada caso.

**Desserialização:** `JsonSerializer.Deserialize` transforma o texto JSON em `List<ProdutoApi>`. Os atributos `JsonPropertyName` relacionam os nomes da API às propriedades em português:

| JSON | Propriedade C# | Tipo |
| --- | --- | --- |
| `id` | `Id` | `int` |
| `title` | `Nome` | `string` |
| `price` | `PrecoUnitario` | `decimal` |
| `description` | `Descricao` | `string` |

ID, título e preço são obrigatórios. Uma lista vazia é válida; JSON nulo, formato incorreto, campos obrigatórios ausentes ou dados incompatíveis são tratados como erro. A descrição é opcional e recebe texto vazio quando ausente.

**LINQ:** `OrderBy` ordena pelo nome ou pelo preço numérico. `ThenBy` define os desempates. `ToList` materializa uma nova lista, preservando a ordem original da resposta. A comparação de nomes usa português do Brasil e ignora diferenças de maiúsculas e minúsculas.

**Separação de responsabilidades:** o serviço cuida da comunicação, desserialização e ordenação. `MenuConsultaApi` cuida das mensagens e mantém a última consulta válida. `MenuPrincipal` encaminha as opções. `ProdutoApi` representa os dados externos.

## Testes automatizados

Execute:

```sh
dotnet run --project comex.csproj -- --testes
```

Resultado esperado:

```text
18 testes de domínio aprovados.
19 testes da Semana 3 aprovados.
20 cenários de console aprovados.
30 testes da Semana 4 aprovados.
```

São **87 verificações**, todas executadas com sucesso. Os 30 testes novos simulam respostas HTTP para testar sucesso, desserialização, ordenação, HTTP 403/500, falha de rede, timeout, cancelamento, dados inválidos e integração com o menu. Eles não dependem da disponibilidade da API real. As verificações anteriores continuam cobrindo clientes, produtos, herança, interfaces, carrinho e compra.

O **build completo foi concluído com zero erros e zero avisos**, usando o SDK .NET 8.0.415. Veja [VALIDACAO.md](VALIDACAO.md) para os resultados e o registro da consulta real.

## Funcionalidades das semanas anteriores

| Opção | Ação |
| --- | --- |
| 1 / 2 | Cadastrar e listar clientes |
| 3 / 4 | Cadastrar produto comum e listar o catálogo local |
| 5 | Ajustar preço de produto local |
| 6 / 7 | Adicionar ao carrinho e simular pagamento |
| 8 | Demonstrar herança e interfaces |
| 9 / 10 | Cadastrar eletrônico e livro |
| 11 | Identificar clientes e livros pela interface |
| 12 / 13 | Ordenar produtos locais por nome e preço |
| -1 | Encerrar |

O [README da Semana 3](https://github.com/giraocaio/Comex/blob/Semana3-CaioGirao/README.md) contém o roteiro dos cadastros e da compra.

## Regras e limites

- Produtos externos ficam em uma lista de consulta separada do catálogo local. Não são importados automaticamente nem adicionados ao carrinho.
- Preços externos são exibidos como valores numéricos recebidos, sem conversão de moeda ou atribuição automática de R$.
- Uma falha de atualização preserva a última consulta bem-sucedida e informa isso ao usuário. Uma resposta válida vazia substitui a lista anterior por uma lista vazia.
- Dados e consultas ficam apenas em memória e são perdidos ao encerrar.
- O pagamento local continua sendo uma simulação. Use somente dados fictícios. Não existe cobrança real ou integração bancária.
- A quantidade do catálogo local continua informativa, sem controle de estoque.

## Referências

- [Fake Store API](https://fakestoreapi.com/)
- [HttpClient — Microsoft](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient)
- [Desserialização com System.Text.Json — Microsoft](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization)
- [Entrega da Semana 3](https://github.com/giraocaio/Comex/tree/Semana3-CaioGirao)
