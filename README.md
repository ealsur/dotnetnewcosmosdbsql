# dotnet new for Azure Cosmos DB

This repo holds a template for `dotnet new` to create ASP.NET Core applications with Azure Cosmos DB SQL accounts.

## How do I install it?

Just type: `dotnet new -i CosmosDBSQLTemplate.CSharp` it will pull the latest version from [Nuget](https://www.nuget.org/packages/CosmosDBSQLTemplate.CSharp).

## How do I use it?

Type `dotnet new cosmosdbsql --help` for the complete help page or `dotnet new cosmosdb-sql -ac yourAccount -k yourKey -d MyDatabase -c MyCollection` where `yourAccount` is your Account Name, `yourKey` is your Account Key, `MyDatabase` is the name of your database and `MyCollection` the name of the collection.

Please use the [Issues](https://github.com/ealsur/dotnetnewcosmosdbsql/issues) section to report any problem you might encounter.
