# Validação da Semana 3

Verificação realizada em **20/10/2026**.

## Resultado

Todos os arquivos C# foram compilados diretamente pelo **Roslyn do SDK .NET 8.0.415**, usando as referências do .NET 8.0.21, nullable habilitado e avisos tratados como erros. A compilação terminou **sem erros ou avisos**.

O executável foi executado com o runtime **.NET 8.0.21**. Passaram **57 verificações**:

| Grupo | Quantidade | Código |
| --- | --- | --- |
| Regras herdadas da Semana 2 | 18 | `Testes/TestesDoDominio.cs` |
| Herança, interface e catálogo com LINQ | 19 | `Testes/TestesSemana3.cs` |
| Interação com o menu refatorado | 20 | `Testes/TestesDoConsole.cs` |

## O que foi verificado

As regras anteriores cobrem construtor obrigatório, campos do produto, associação de endereço, valores negativos, carrinhos independentes, cálculo de total, reajuste e proteção da coleção do carrinho.

As 19 verificações da Semana 3 cobrem:

1. Dados herdados e específicos de Eletronico.
2. Dados herdados e específicos de Livro.
3. Identificar cliente por nome e CPF por meio da interface.
4. Identificar livro por nome e ISBN por meio da interface.
5. Nome obrigatório nos dois subtipos.
6. Rejeição de ISBN vazio ou nulo.
7. Rejeição de páginas não positivas.
8. Rejeição de voltagem não positiva.
9. Rejeição de potência não positiva.
10. Carrinho com os dois subtipos e reajuste posterior.
11. Duplicidade entre tipos sem substituição do objeto original.
12. Busca ignorando caixa e espaços externos.
13. Busca inexistente.
14. Rejeição de produto nulo no catálogo.
15. Ordenação por nome, com acento e diferenças de caixa, segundo pt-BR.
16. Ordenação numérica por preço e desempate por nome.
17. Preservação do catálogo original ao ordenar.
18. Ordenação considerando preço atualizado.
19. Ordenação de catálogo vazio.

Os 20 cenários de console executam o menu real com `Console.SetIn` e `Console.SetOut`, restaurando os fluxos ao final de cada cenário. Cobrem menu inválido, consultas vazias, cadastro completo do cliente, CPF duplicado/inválido, listagem polimórfica, identificação por interface, demonstração isolada, as duas ordenações, duplicidade entre tipos, compra com carrinho misto, cancelamento, cartão inválido, consultas inexistentes, campos específicos inválidos, preço/quantidade inválidos e fim da entrada durante cadastro.

## Reproduzir localmente

Com o SDK do .NET 8 instalado, na pasta que contém `comex.csproj`:

```sh
dotnet build comex.csproj
dotnet run --project comex.csproj -- --testes
dotnet run --project comex.csproj
```

A opção 8 demonstra os conceitos sem alterar os cadastros. O README contém um roteiro completo para cadastro, ordenação, identificação e compra.

## Limite da verificação do ambiente

A CLI do .NET não inicializa normalmente no ambiente de preparação: ocorre uma falha na consulta de informações do processo (`System.Diagnostics.Process.GetStat`). A tentativa de executar o MSBuild diretamente também falhou ao consultar o nome do processo.

Por isso, não se afirma que `dotnet build` foi concluído aqui. Foram concluídas a compilação direta pelo compilador do SDK e a execução dos testes. O comando completo de build deve ser confirmado no computador de execução ou em um ambiente .NET convencional.
