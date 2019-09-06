# SQL Server setup (Linux)

docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=DevPassword1' -p 1433:1433 --name sxbdb -d microsoft/mssql-server-linux

dotnet ef migrations add Initial -o Migrations -c sXb_service.EF.Context

dotnet ef database update Initial -c sXb_service.EF.Context
