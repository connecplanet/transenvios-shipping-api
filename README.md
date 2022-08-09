# transenvios-shipping-api
RESTful de integración de Transporte para Transenvios

### Features

- User management
- User authentication

### References

* [HTTP response status codes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status)
* [HTTP headers | User-Agent](https://www.geeksforgeeks.org/http-headers-user-agent/#:~:text=The%20HTTP%20headers%20User-Agent%20is%20a%20request%20header,user%20agent%20to%20every%20website%20you%20connect%20to.)
* [How to send emails from C#/.NET - The definitive tutorial](https://blog.elmah.io/how-to-send-emails-from-csharp-net-the-definitive-tutorial/)
* [Angular 8 - Basic HTTP Authentication Tutorial & Example](https://jasonwatmore.com/post/2019/06/26/angular-8-basic-http-authentication-tutorial-example)
* [.NET 6.0 - User Registration and Login Tutorial with Example API](https://jasonwatmore.com/post/2022/01/07/net-6-user-registration-and-login-tutorial-with-example-api)
* [EF Dependency Injection](https://www.c-sharpcorner.com/article/clean-architecture-with-net-6-using-entity-framework/)
* [Flutter vs React Native](https://www.tabnine.com/blog/flutter-vs-react-native/?utm_source=rss&utm_medium=rss&utm_campaign=flutter-vs-react-native)
* [Readme.md editor](https://pandao.github.io/editor.md/en.html)
* [Tutorial: Create a web API with ASP.NET Core](https://docs-microsoft-com.translate.goog/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio-code&_x_tr_sl=en&_x_tr_tl=es&_x_tr_hl=es&_x_tr_pto=op%2Cwapp)
* [EF cli](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
* [EF cli tutorial](https://www.entityframeworktutorial.net/efcore/cli-commands-for-ef-core-migration.aspx)
* [The Fluent API IsRequired Method](https://www.learnentityframeworkcore.com/configuration/fluent-api/isrequired-method)

### EF

Installing the tools
```
dotnet tool install --global dotnet-ef
```

Verify installation
```
dotnet ef
```

### Migrations

SQLite EF Core Migrations (Windows/MacOS):
```
dotnet ef migrations add InitialCreate --context SqliteDataContext --output-dir Migrations/SqliteMigrations
```

SQL Server EF Core Migrations (Windows Command):
```
set ASPNETCORE_ENVIRONMENT=Production
dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqlServerMigrations
```

SQL Server EF Core Migrations (Windows PowerShell):
```powershell
$env:ASPNETCORE_ENVIRONMENT="Production"
dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqlServerMigrations
```

SQL Server EF Core Migrations (MacOS):
```
ASPNETCORE_ENVIRONMENT=Production dotnet ef migrations add InitialCreate --context DataContext --output-dir Migrations/SqlServerMigrations
```

### Update EF

Powershell
```
Add-Migration DBInit6
Update-Database
```
Console
```
dotnet ef migrations add MyFirstMigration --context SqliteDataContext
dotnet ef database update InitialCreate --context SqliteDataContext
```
Console 2022-06-27
```
dotnet ef migrations add US3-register-shipping-requester --context MySqlDataContext  --output-dir Migrations/MySqlMigrations
dotnet ef database update --context MySqlDataContext
```

``` $ dotnet ef migrations add US5-add-cities-and-routes --context MySqlDataContext --output-dir Migrations/MySqlMigrations
```
```$ dotnet ef database update --context MySqlDataContext
```
### Free MySQL Database

[free mysql hosting](https://www.freemysqlhosting.net/)