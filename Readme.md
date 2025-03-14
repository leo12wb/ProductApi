# Product API - ASP.NET Core

Este repositório contém uma API simples de gerenciamento de produtos, construída com ASP.NET Core. A API permite realizar operações básicas como:

Listar todos os produtos
Consultar um produto específico por ID
Criar um novo produto
Atualizar um produto existente
Excluir um produto
A API utiliza rotas RESTful e está configurada para rodar tanto em HTTP quanto HTTPS. Além disso, a documentação da API é gerada automaticamente com Swagger/OpenAPI para facilitar o teste e a exploração dos endpoints.

## Tecnologias utilizadas:

#### ASP.NET Core 6+
#### OpenAPI (Swagger)
#### C#

##

Como rodar:

Clone o repositório
Execute o comando dotnet run para iniciar a aplicação
Acesse os endpoints via http://localhost:5007/api/products ou https://localhost:7226/api/products

```bash
dotnet clean
dotnet build
dotnet run
```

Adicionar a migração:
```bash
dotnet ef migrations add InitialCreate
```
Atualizar o banco de dados:
```bash
dotnet ef database update
```
