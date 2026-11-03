# Comex | Semana 05

Aplicação de console em C# do treinamento Alura / SEFAZ. Esta entrega continua a Semana 4 com **persistência do JSON da API, biblioteca de relatórios, pacote NuGet local e exportação CSV com LINQ e polimorfismo**.

**Autor:** Caio Girão

**Branch:** `Semana05-CaioGirao`, criada a partir de `Semana4-CaioGirao`.

**Link para entrega:** https://github.com/giraocaio/Comex/tree/Semana05-CaioGirao

## Atividades atendidas

| Atividade | Onde conferir |
| --- | --- |
| Continuar a versão final da Semana 4 | Histórico da branch e [VALIDACAO.md](VALIDACAO.md) |
| Criar Class Library `Comex.GeneradorRelatorio` | [Projeto da biblioteca](Comex.GeneradorRelatorio/Comex.GeneradorRelatorio.csproj) |
| Interface com `string Exportar(IEnumerable<ProdutoRelatorioDto> produtos, string caminhoArquivo)` | [IGeradorRelatorio.cs](Comex.GeneradorRelatorio/IGeradorRelatorio.cs) |
| Exportar CSV e retornar `Path.GetFullPath(caminhoArquivo)` | [GeradorCsv.cs](Comex.GeneradorRelatorio/GeradorCsv.cs) |
| Documentação XML e metadados do pacote | Comentários públicos da biblioteca e propriedades do `.csproj` |
| Gerar pacote automaticamente no build | `GeneratePackageOnBuild`, pacote [1.0.0](nuget-local/Comex.GeneradorRelatorio.1.0.0.nupkg) |
| Configurar feed e consumir pacote NuGet | [NuGet.Config](NuGet.Config) e `PackageReference` em [comex.csproj](comex.csproj) |
| Persistir JSON bruto antes da desserialização | [ProdutosApiServico.cs](Servicos/ProdutosApiServico.cs), opções **8** e **14** |
| Menu de relatório herdando de `Menu` | [MenuGerarRelatorioCsv.cs](Menus/MenuGerarRelatorioCsv.cs), opção **18** |
| Aplicar `Where`, `OrderByDescending` e `Select` | Filtro por preço positivo, ordem decrescente e projeção para DTO |
| Usar interface e mostrar o caminho retornado | `IGeradorRelatorio gerador = new GeradorCsv()` e `caminhoSalvo` |
| Registrar o menu no dicionário de `Program.cs` | `menus.Add(18, new MenuGerarRelatorioCsv())` |

O nome **GeneradorRelatorio** segue exatamente o enunciado. A biblioteca é consumida como pacote: não há `ProjectReference` entre o console e a biblioteca.

## Como abrir e executar no computador

Pré-requisito: **SDK do .NET 8**, incluindo as ferramentas de compilação.

1. No GitHub Desktop, escolha o repositório **Comex** e clique em **Fetch origin**.
2. Em **Current branch**, escolha **Semana05-CaioGirao**.
3. Abra a pasta no VS Code ou no terminal. Você deve estar na pasta que contém `comex.csproj` e `NuGet.Config`.
4. Execute:

```sh
dotnet build comex.csproj
dotnet run --project comex.csproj
```

Outra opção é abrir o [link da branch](https://github.com/giraocaio/Comex/tree/Semana05-CaioGirao), selecionar **Code > Download ZIP**, extrair tudo e abrir a pasta extraída. Mantenha a pasta `nuget-local`: ela contém a dependência do console.

Para clonar pelo terminal:

```sh
git clone --branch Semana05-CaioGirao https://github.com/giraocaio/Comex.git
cd Comex
dotnet run --project comex.csproj
```

O pacote `Comex.GeneradorRelatorio.1.0.0.nupkg` está incluído no repositório para facilitar a avaliação. O restore encontra esse pacote no feed local automaticamente, sem publicação no nuget.org. O SDK precisa estar instalado, mas não é necessário acessar a internet para restaurar esse pacote ou rodar os testes.

## Roteiro principal para testar/apresentar

1. Execute a aplicação na pasta do projeto.
2. Selecione **8 — Consultar produtos da API e salvar JSON**.
3. Se a API responder com sucesso, confira os produtos exibidos e o caminho de `produtos.json` informado no console.
4. Abra esse arquivo: o texto é o JSON recebido, sem reserialização. Ele conserva campos externos que não são usados pelo DTO.
5. Selecione **18 — Gerar relatório CSV do JSON salvo**.
6. Confira a quantidade exportada e o caminho absoluto retornado pela biblioteca.
7. Abra `relatorio-produtos.csv` em um editor ou em uma planilha. Ao importar manualmente, selecione **UTF-8**, separador **ponto e vírgula** e números em **português do Brasil**.
8. Confira o cabeçalho `Id;Nome;Descricao;Preco`, os produtos com preço maior que zero e a ordem decrescente de preço.
9. Encerre com **-1**, execute novamente e escolha **18**: o relatório pode ser gerado sem nova consulta, pois o JSON está no disco.

Os caminhos padrão são relativos à **pasta de trabalho atual**, isto é, a pasta a partir da qual o programa foi iniciado. Nos comandos acima, é a raiz do projeto. Os caminhos absolutos são mostrados no console.

Uma nova consulta bem-sucedida atualiza `produtos.json`. Uma nova exportação substitui `relatorio-produtos.csv`. Esses dois arquivos de execução não são versionados.

## Teste offline, caso a API não esteja disponível

Na consulta real em **03/11/2026**, a API respondeu **HTTP 403** neste ambiente. O erro foi tratado e a aplicação voltou ao menu. O sucesso da persistência HTTP foi verificado com respostas simuladas, sem afirmar que houve uma resposta real com produtos.

Para demonstrar a leitura de disco e a exportação sem depender da API, use o arquivo **explicitamente fictício** [Exemplos/produtos.simulados.json](Exemplos/produtos.simulados.json). Feche o programa antes de copiar. O comando abaixo substitui o JSON atual; se desejar preservá-lo, renomeie-o antes.

No PowerShell, dentro da pasta do projeto:

```powershell
Copy-Item Exemplos/produtos.simulados.json produtos.json
dotnet run --project comex.csproj
```

No Linux/macOS:

```sh
cp Exemplos/produtos.simulados.json produtos.json
dotnet run --project comex.csproj
```

Escolha **18**. O resultado esperado é:

| Ordem | Produto | Preço |
| --- | --- | --- |
| 1 | Monitor | 850,00 |
| 2 | Mouse | 120,50 |
| 3 | Livro de C# | 29,90 |

O Caderno tem preço zero e fica fora do relatório, demonstrando o filtro `Where`. A descrição do Mouse contém aspas; a do Livro contém ponto e vírgula, demonstrando o escape correto no CSV.

A opção **17** continua demonstrando uma resposta HTTP simulada da Semana 4. Ela usa um JSON temporário que é removido ao terminar e **não substitui** o `produtos.json` real nem alimenta a opção 18.

## O que explicar ao professor

- **Persistência:** após `EnsureSuccessStatusCode()` e a leitura da resposta, `File.WriteAllTextAsync` salva o JSON bruto. Só depois ocorre a desserialização. Uma resposta HTTP de erro não substitui o arquivo anterior. Um HTTP 200 com JSON inválido é salvo para inspeção, mas a desserialização informa o erro e o relatório recusa esse conteúdo.
- **DTO:** `ProdutoApi` representa o formato externo. `ProdutoRelatorioDto` representa apenas o contrato necessário para exportar; a biblioteca não depende de `ProdutoApi`, de `Produto` nem dos menus.
- **LINQ:** `Where` seleciona preços maiores que zero; `OrderByDescending` ordena pelo valor numérico; `ThenBy` desempata por ID; `Select` cria os DTOs do relatório. O filtro é uma escolha desta implementação porque o enunciado não define um critério específico.
- **Polimorfismo:** o relatório chama `Exportar` por uma variável `IGeradorRelatorio`. Outro formato pode implementar essa mesma interface sem alterar o contrato existente. A escolha da implementação pode então ser trocada no ponto de composição.
- **Herança:** `MenuGerarRelatorioCsv` herda de `Menu` e sobrescreve `ExecutarAsync`. `MenuPrincipal` executa as opções pelo tipo base. `MenuAcao` adapta as funções anteriores para o mesmo contrato.
- **NuGet:** o console usa `PackageReference` para a versão 1.0.0. `NuGet.Config` configura o feed `nuget-local`; a biblioteca gera o `.nupkg` no build. O pacote contém DLL, XML de documentação e README.
- **Retorno útil:** a biblioteca retorna `Path.GetFullPath(caminhoArquivo)` depois de gravar. O menu guarda esse retorno em `caminhoSalvo` e o exibe.
- **Tratamento de falhas:** ausência do JSON, JSON inválido, falhas de leitura/gravação e falta de permissão têm mensagens próprias. O usuário volta ao menu.

## Gerar novamente a biblioteca e o pacote

Para demonstrar a criação automática do `.nupkg`, execute na raiz do projeto:

```sh
dotnet build Comex.GeneradorRelatorio/Comex.GeneradorRelatorio.csproj --configuration Release
```

Confira:

- `nuget-local/Comex.GeneradorRelatorio.1.0.0.nupkg`;
- `Comex.GeneradorRelatorio/bin/Release/net8.0/Comex.GeneradorRelatorio.xml`.

O `.csproj` contém ID, título, versão, descrição, autores, `GenerateDocumentationFile`, `GeneratePackageOnBuild` e `PackageOutputPath`.

O arquivo `NuGet.Config` configura a fonte apenas neste repositório. No Visual Studio, a fonte aparece como **ComexLocal** ao abrir o projeto. Se precisar cadastrá-la manualmente, use **Ferramentas > Gerenciador de Pacotes NuGet > Configurações > Fontes de pacote**, adicionando o caminho absoluto de `nuget-local`.

Se modificar a biblioteca, incremente a versão no projeto dela, gere o pacote e atualize a versão do `PackageReference` no console. Isso evita reutilizar a versão antiga do cache. O console não recompila automaticamente o código da biblioteca, pois depende do pacote.

## Testes automatizados

```sh
dotnet run --project comex.csproj -- --testes
```

Resultado esperado: **105 verificações aprovadas**:

```text
18 testes de domínio aprovados.
19 testes da Semana 3 aprovados.
20 cenários de console aprovados.
30 testes da Semana 4 aprovados.
18 testes da Semana 5 aprovados.
```

Os testes usam pastas temporárias e respostas HTTP simuladas. Não alteram os arquivos de uso normal e não precisam da internet. Veja [VALIDACAO.md](VALIDACAO.md) para os cenários e resultados registrados.

## Numeração do menu e funcionalidades anteriores

| Opção | Ação |
| --- | --- |
| 1 / 2 | Cadastrar e listar clientes |
| 3 / 4 | Cadastrar produto comum e listar o catálogo local |
| 5 | Ajustar preço de produto local |
| 6 / 7 | Adicionar ao carrinho e simular pagamento |
| **8** | **Consultar a API e salvar JSON, conforme o enunciado da Semana 5** |
| 9 / 10 | Cadastrar eletrônico e livro |
| 11 | Identificar clientes e livros |
| 12 / 13 | Ordenar produtos locais por nome e preço |
| 14 | Mesmo fluxo da opção 8, mantendo o atalho da Semana 4 |
| 15 / 16 | Ordenar a última consulta externa em memória por nome/preço crescente |
| 17 | Demonstração HTTP offline da Semana 4 |
| **18** | **Gerar relatório CSV a partir do JSON em disco** |
| 19 | Demonstração de herança/interfaces, que antes estava na opção 8 |
| -1 | Encerrar |

O catálogo local, clientes e carrinhos continuam somente em memória; o pagamento é uma simulação com dados fictícios. Os produtos externos não são importados automaticamente para o catálogo/carrinho. Os preços externos não recebem conversão de moeda nem indicação automática de reais.

## Referências

- [Feed NuGet local — Microsoft](https://learn.microsoft.com/en-us/nuget/hosting-packages/local-feeds)
- [Criação de pacote com MSBuild — Microsoft](https://learn.microsoft.com/en-us/nuget/create-packages/creating-a-package-msbuild)
- [Entrega da Semana 4](https://github.com/giraocaio/Comex/tree/Semana4-CaioGirao)
