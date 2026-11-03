# Comex.GeneradorRelatorio

Biblioteca .NET 8 da Semana 05 do Comex. O nome `Generador` segue o enunciado.

```csharp
using Comex.GeneradorRelatorio;

IGeradorRelatorio gerador = new GeradorCsv();
string caminho = gerador.Exportar(new[]
{
    new ProdutoRelatorioDto { Id = 1, Nome = "Livro", Preco = 29.90m }
}, "produtos.csv");
Console.WriteLine(caminho);
```

O método retorna o caminho absoluto após gravar o arquivo. A pasta de destino deve existir.
O CSV usa ponto e vírgula, números com vírgula decimal, duas casas, CRLF e UTF-8 com BOM.
Campos de texto são envolvidos em aspas; aspas internas são duplicadas, preservando separadores e quebras de linha.
Textos iniciados por `=`, `+`, `-`, `@`, tabulação ou quebra de linha recebem um apóstrofo para não serem tratados como fórmulas por planilhas.
Uma coleção vazia gera apenas o cabeçalho. Um arquivo existente é substituído.
A biblioteca não conhece a API, os menus nem as entidades de domínio do console.

O build gera automaticamente o pacote `1.0.0` no feed `../nuget-local` e inclui a documentação XML.
