# Feed NuGet local

Esta pasta é a fonte `ComexLocal` configurada em `../NuGet.Config`.

O pacote `Comex.GeneradorRelatorio.1.0.0.nupkg` foi gerado pelo build da biblioteca e é versionado intencionalmente para que um clone ou ZIP consiga restaurar o console imediatamente.

Para gerar o pacote a partir do código, execute na raiz do repositório:

```sh
dotnet build Comex.GeneradorRelatorio/Comex.GeneradorRelatorio.csproj -c Release
```

Para alterações futuras, incremente a versão da biblioteca e do `PackageReference` do console. Não é necessário publicar no nuget.org nem modificar as fontes NuGet globais do computador.
