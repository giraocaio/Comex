# Validação da Semana 4

Verificação realizada em **28/10/2026**.

## Build completo

Executado com **SDK .NET 8.0.415**, tendo como alvo `net8.0`:

```sh
dotnet build comex.csproj --configuration Release -p:TreatWarningsAsErrors=true --nologo
```

Resultado: **Build succeeded, 0 Warning(s), 0 Error(s)**. A restauração do projeto também foi concluída. Nesta entrega, a CLI funcionou e foi possível validar o build completo.

## Testes automatizados

Executado:

```sh
dotnet run --project comex.csproj --configuration Release --no-build -- --testes
```

Resultado: saída com código **0**, com **87 verificações aprovadas**:

| Grupo | Quantidade | Código |
| --- | --- | --- |
| Regras de domínio da Semana 2 | 18 | `Testes/TestesDoDominio.cs` |
| Herança, interface e LINQ da Semana 3 | 19 | `Testes/TestesSemana3.cs` |
| Fluxos anteriores do menu | 20 | `Testes/TestesDoConsole.cs` |
| HTTP, JSON, LINQ e menu da Semana 4 | 30 | `Testes/TestesSemana4.cs` |

### Os 30 testes novos

1. HttpClient envia GET para `https://fakestoreapi.com/products`.
2. Desserialização mapeia os campos e ignora informações extras.
3. Preço decimal é preservado e descrição ausente recebe texto vazio.
4. Lista vazia é aceita.
5. JSON malformado é rejeitado.
6. Corpo vazio é rejeitado.
7. Raiz JSON nula é rejeitada.
8. Objeto JSON em vez de lista é rejeitado.
9. Item nulo na lista é rejeitado.
10. Nome obrigatório ausente é rejeitado.
11. Preço obrigatório ausente é rejeitado.
12. Identificador obrigatório ausente é rejeitado.
13. Nome vazio é rejeitado.
14. Preço negativo é rejeitado.
15. Identificador não positivo é rejeitado.
16. Preço com tipo incompatível é rejeitado.
17. Ordenações por nome e preço, desempates e preservação da origem.
18. Ordenação numérica de preços 2, 10 e 100.
19. HTTP 403 é tratado sem anunciar sucesso.
20. HTTP 500 é tratado sem anunciar sucesso.
21. Falha de rede é tratada.
22. Timeout real do HttpClient é tratado, usando um handler que aguarda o cancelamento.
23. Cancelamento solicitado é propagado pelo serviço.
24. JSON inválido gera mensagem adequada no menu.
25. Ordenação antes da primeira consulta orienta o usuário.
26. Falha de atualização preserva a consulta anterior.
27. Atualização válida vazia substitui os dados antigos.
28. Menu principal consulta e ordena sem alterar o catálogo local; ordenar não dispara novas requisições.
29. Erro HTTP devolve o controle ao menu principal.
30. Demonstração offline é identificada e não preenche a lista da consulta real.

Os testes usam `HttpClient` com um `HttpMessageHandler` simulado. Isso permite reproduzir respostas e falhas sem depender da internet e não é evidência de sucesso da API externa real.

## Consulta real à API

A aplicação compilada foi executada com a sequência de entradas **14, 15, -1**. A opção 14 enviou a consulta real. O resultado observado foi:

```text
Iniciando consulta de produtos...
Não foi possível consultar a API: HTTP 403.
Consulte a API pela opção 14 antes de ordenar os produtos externos.
COMEX FINALIZADO!
```

A execução terminou com código **0**. O erro HTTP foi tratado, nenhuma lista fictícia foi carregada como resposta real e o menu permaneceu utilizável. Uma consulta independente ao mesmo endereço também retornou HTTP 403 no ambiente de preparação.

A consulta real bem-sucedida depende de a API permitir o acesso na rede de execução. A opção **17** demonstra o fluxo com dados fictícios e indica explicitamente que a resposta HTTP é simulada, sem acesso externo.

## Reproduzir no computador

Na pasta que contém `comex.csproj`, com o SDK do .NET 8 instalado:

```sh
dotnet build comex.csproj
dotnet run --project comex.csproj -- --testes
dotnet run --project comex.csproj
```

Use 14 para consulta real, 15/16 para ordenar uma consulta bem-sucedida e 17 para a demonstração offline. O README traz o roteiro completo para a monitoria.
