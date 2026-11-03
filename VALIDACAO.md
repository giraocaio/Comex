# Validação da Semana 05

Data: **03/11/2026**. SDK **.NET 8.0.415**, runtime **8.0.21**, Linux.

## Base da entrega

- Origem: branch `Semana4-CaioGirao`, commit `939e5fd63874210aca9036e8f2d391dd510e9abe`.
- Nova branch: `Semana05-CaioGirao`.
- Antes das implementações, a base executou os 87 testes existentes e compilou com zero erros/avisos.

## Compilação e consumo NuGet

Comandos executados com o SDK local:

```sh
dotnet build Comex.GeneradorRelatorio/Comex.GeneradorRelatorio.csproj -c Release -p:TreatWarningsAsErrors=true
dotnet build comex.csproj -c Release -p:TreatWarningsAsErrors=true
dotnet run --project comex.csproj -c Release --no-build -- --testes
```

Resultados:

- Biblioteca: **0 erros, 0 avisos**; `.nupkg` gerado automaticamente no build.
- Console: **0 erros, 0 avisos**; referência resolvida como pacote NuGet, sem `ProjectReference`.
- Arquivo `obj/project.assets.json`: dependência `Comex.GeneradorRelatorio/1.0.0`, tipo `package`.
- Pacote inspecionado: DLL e XML em `lib/net8.0`, README e metadados com ID, título, versão, descrição e autor.
- Uma cópia limpa, sem `bin`, `obj` ou cache `.nuget`, restaurou o pacote a partir do feed versionado e compilou com **0 erros/avisos**.

## Testes automatizados

**105 verificações aprovadas:** 18 de domínio, 19 da Semana 3, 20 de console, 30 da Semana 4 e 18 da Semana 5.

Os 18 cenários novos verificam:

1. Persistência do texto JSON sem reserialização e desserialização preservada.
2. Atualização do arquivo sem concatenar a resposta anterior.
3. Gravação de um JSON inválido de HTTP 200 antes de tentar desserializar.
4. HTTP 500 sem sobrescrever o arquivo existente.
5. Falha de gravação do JSON tratada sem falso sucesso.
6. Caminho absoluto, cabeçalho, acentos, BOM e biblioteca em assembly separado.
7. Escape de aspas, ponto e vírgula e quebras de linha no CSV.
8. Formatação numérica pt-BR mesmo com cultura de execução en-US.
9. Proteção de texto com aparência de fórmula.
10. Coleção vazia e substituição de arquivo CSV anterior.
11. Rejeição de coleção nula e caminho vazio.
12. Ausência do JSON com orientação para consultar a API.
13. Leitura do disco, filtro, ordenação decrescente, projeção e exibição do caminho.
14. Nenhum produto elegível: CSV com cabeçalho e mensagem clara.
15. Rejeição de JSON malformado, raiz nula, item nulo e preço negativo; relatório anterior preservado.
16. Falha ao gravar CSV com retorno ao menu.
17. Integração via dicionário: opção 8 → opção 18 → saída.
18. Demonstração offline sem sobrescrever o JSON real.

A execução completa dos testes usa pastas temporárias, removidas ao terminar. As respostas HTTP são simuladas deliberadamente. O teste anterior da demonstração de herança foi atualizado da opção 8 para a 19; a funcionalidade continua sendo verificada.

## Execução real do programa

Entrada: `8`, `18`, `-1`, em uma pasta sem JSON prévio.

```text
Não foi possível consultar a API: HTTP 403.
Arquivo produtos.json não encontrado. Consulte a API pela opção 8 ou 14 primeiro.
COMEX FINALIZADO!
```

Processo encerrado com código **0**. A API externa não retornou produtos neste ambiente. O tratamento de erro foi verificado; o sucesso da persistência de uma resposta HTTP foi validado nos testes com `HttpMessageHandler` simulado.

## Execução offline do programa completo

O exemplo `Exemplos/produtos.simulados.json` foi copiado para uma pasta de teste como `produtos.json`. O executável real recebeu `18` e `-1`.

Resultado: **3 produtos exportados**, caminho absoluto exibido, processo encerrado com código **0**. Conteúdo do CSV (aqui sem mostrar o BOM):

```csv
Id;Nome;Descricao;Preco
3;"Monitor";"Produto fictício";850,00
4;"Mouse";"Modelo ""especial""";120,50
2;"Livro de C#";"Dados fictícios; demonstração";29,90
```

O Caderno de preço zero foi filtrado. Essa demonstração comprova o fluxo de leitura de disco e exportação usando o pacote local, sem depender da resposta da API real.
