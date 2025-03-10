# Configuration Control API

## Getting Started

Install entity framework:<br>
`dotnet tool install --global dotnet-ef`

Install packages:<br>
`dotnet restore`

Go to ./src/ folder and add migration after create an model:<br>
`dotnet ef migrations add <NameOfModel>v1 --output-dir Infra/Database/Migrations`

> The migrations are executed on project build/run.

## Build and Test

> MySql database on docker-compose file

Run docker-compose:<br>
`docker-compose up -d`

Build project:<br>
`dotnet build`

Development:<br>
`dotnet watch`

Run tests:<br>
`dotnet test`
