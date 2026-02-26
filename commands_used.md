## Creating new solution
dotnet new sln -n MicroservicesDemo

## Creating new project
dotnet new webapi -n <ProjectName> --use-controllers

## Adding project to solution file
dotnet sln add <csproj file>

## Adding entity framework core to project
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

## Add migration
dotnet ef migrations add InitialCreate

## Running SQL Server inside docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd2026" -p 1433:1433 --name sqlserver  -d mcr.microsoft.com/mssql/server:2022-latest

## Re-deploying containers with scaling
docker compose up --build --scale userservice=3 --scale orderservice=2


## Orders endpoint
http://localhost:5000/orders/api/orders

## Users enpoint
http://localhost:5000/users/api/users

## Bash into a container for debugging 
docker exec -it 73a16ee0ecee bash
