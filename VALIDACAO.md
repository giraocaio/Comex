# Validação da Semana 1

Validação realizada em 08/10/2026.

O `Program.cs` foi compilado pelo compilador Roslyn do SDK .NET 8.0.415, usando as referências do .NET 8.0.21, nullable habilitado e avisos tratados como erros. A compilação terminou sem erros ou avisos.

O executável foi rodado com o runtime .NET 8.0.21. Foram aprovados 17 cenários com entradas de console e verificações das mensagens de saída:

1. Menu com letras, opção inexistente e saída com -1.
2. Listagens de clientes e produtos vazias.
3. Cadastro de cliente, remoção da pontuação do CPF e rejeição de duplicidade.
4. CPF incompleto e nomes vazios.
5. Produto duplicado com outra capitalização e preço com ponto decimal.
6. Preço negativo.
7. Preço zero.
8. Preço não numérico.
9. Preço com três casas decimais significativas.
10. Preço com separador de milhar.
11. Preço acima do limite informado.
12. Consultas de cliente e produto inexistentes.
13. Fechamento de carrinho vazio.
14. Duas unidades, ajuste posterior do preço, total de R$ 51,00 e esvaziamento após a compra.
15. Cancelamento e cartão inválido preservando os itens para nova tentativa.
16. Carrinhos independentes para dois clientes.
17. Fim da entrada durante um cadastro sem provocar loop infinito.

## Limite da verificação

O comando da CLI `dotnet build` não pôde ser concluído neste ambiente: a inicialização da CLI falhou ao consultar informações do processo (`System.Diagnostics.Process.GetStat`). A compilação direta com o compilador do próprio SDK e a execução do programa foram concluídas. O projeto deve ser aberto e compilado também no Visual Studio ou pelo comando `dotnet build comex.csproj` na máquina de entrega.
