# Validação da Semana 2

Validação realizada em **13/10/2026**.

## Compilação e execução

Todos os arquivos C# foram compilados com o compilador Roslyn do SDK .NET **8.0.415**, usando as referências do .NET **8.0.21**, nullable habilitado e avisos tratados como erros. A compilação terminou sem erros ou avisos. O executável foi rodado com o runtime .NET 8.0.21.

O comando `dotnet build` não pôde ser concluído neste ambiente: a inicialização da CLI falha ao consultar informações do processo (`System.Diagnostics.Process.GetStat`). A compilação direta com o compilador do próprio SDK e a execução foram concluídas; isso não substitui a verificação do comando completo de build na máquina de entrega.

Para reproduzir localmente com o SDK instalado:

```sh
dotnet build comex.csproj
dotnet run --project comex.csproj -- --testes
dotnet run --project comex.csproj
```

## Verificações das classes

As **18 verificações abaixo passaram**. Estão incluídas em `Testes/TestesDoDominio.cs` e são executadas com a opção de linha de comando `--testes`.

1. Produto não possui construtor sem nome.
2. Produto rejeita nome vazio.
3. Produto rejeita nome composto apenas de espaços.
4. Produto rejeita nome nulo.
5. Produto guarda nome, descrição, preço unitário e quantidade.
6. Preço negativo é rejeitado sem modificar o valor anterior.
7. Quantidade negativa é rejeitada sem modificar o valor anterior.
8. Cliente guarda seus dados e a referência ao objeto Endereco associado.
9. Endereco guarda suas seis propriedades.
10. Cliente rejeita endereço nulo.
11. Endereco rejeita número negativo.
12. Carrinho vazio tem total zero.
13. Carrinho rejeita produto nulo.
14. Duas unidades somam os preços corretamente.
15. Carrinhos de clientes distintos são independentes.
16. Ajuste de preço no objeto Produto é refletido no total do carrinho.
17. A coleção exposta do carrinho não permite alteração externa.
18. Esvaziar o carrinho não altera os dados do produto no catálogo.

## Cenários de console

Foram executados **27 cenários aprovados**, alimentando entradas no executável e verificando mensagens de saída e encerramento. O roteiro manual correspondente está no README; o harness de execução usado no ambiente de preparação não integra o projeto C#.

1. Demonstração de Produto, Cliente e Endereco pela opção 8, sem incluir os exemplos nos cadastros.
2. Menu inválido e listagens vazias.
3. Cadastro e listagem de cliente com todos os dados.
4. CPF duplicado e normalização da pontuação.
5. CPF inválido e nome vazio.
6. Cadastro de cliente incompleto não é salvo.
7. Complemento opcional e endereço com número zero.
8. Número de endereço negativo.
9. Cadastro de produto completo e rejeição de duplicidade.
10. Nome e descrição de produto vazios.
11. Preço negativo.
12. Preço zero.
13. Preço não numérico.
14. Preço com três casas decimais significativas.
15. Preço com separador de milhar.
16. Preço acima do limite.
17. Quantidade negativa.
18. Quantidade fracionária.
19. Quantidade não numérica.
20. Quantidade fora da faixa de int.
21. Preço com ponto decimal e quantidade zero.
22. Buscas de cliente e produto inexistentes.
23. Carrinho vazio.
24. Compra de duas unidades, reajuste posterior para R$ 25,50, total de R$ 51,00 e esvaziamento após pagamento simulado.
25. Cancelamento e cartão inválido preservam o carrinho para nova tentativa.
26. Carrinhos independentes de dois clientes.
27. Fim da entrada durante um cadastro sem travar ou entrar em loop infinito.

O cenário de compra também confirmou que a quantidade cadastrada permanece informativa: não há baixa de estoque nesta etapa.
