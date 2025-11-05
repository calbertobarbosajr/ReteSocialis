# ReteSocialis – Social Network with ASP.NET Core and Angular
# 🌐 ReteSocialis

Uma rede social moderna desenvolvida com **ASP.NET Core 9** e **Angular**, que permite aos usuários **criar conta, fazer login, adicionar amigos, editar perfil, enviar mensagens em tempo real via SignalR** e **atualizar o avatar de perfil**.

---

## ✨ Funcionalidades

- 🧑‍💻 Cadastro e login de usuários com autenticação JWT  
- 💬 Bate-papo em tempo real usando **SignalR**
- 👥 Envio e recebimento de solicitações de amizade
- ⚙️ Edição de perfil (nome de usuário, e-mail e senha)
- 🖼️ Upload de imagem de avatar
- 🔐 Autenticação com **ASP.NET Identity**
- 🧱 Banco de dados relacional com **Entity Framework Core (SQL Server)**

---

## 🧰 Tecnologias utilizadas

### 🖥️ Backend (API — ASP.NET Core 9)
- `Microsoft.AspNetCore.Authentication.JwtBearer`
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
- `Microsoft.AspNetCore.SignalR`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.EntityFrameworkCore.Design`
- `Swashbuckle.AspNetCore.Swagger`
- `Docker`
- `.NET 9 / C# 12`

### 💻 Frontend (Angular)
- `Angular 18+`
- `RxJS`
- `TypeScript`
- `Bootstrap 5`
- `SignalR Client`

---

## ⚙️ Instalação e Configuração

### 🐳 Pré-requisitos
Antes de começar, certifique-se de ter instalado:
- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)
- [Docker Desktop](https://www.docker.com/)

---

## 🚀 Backend — API (ASP.NET Core)

### 1️⃣ Criar o projeto
```bash
dotnet new webapi -n ReteSocialis.API
cd ReteSocialis.API

2 - Instalar os pacotes necessários
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.SignalR
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore.Swagger

3 - Aplicar as migrações e atualizar o banco de dados
dotnet ef migrations add InitialCreate
dotnet ef database update

4 - Executar a API
dotnet run

A API será iniciada em:
👉 http://localhost:5000

🧭 Frontend — Aplicação Angular

1 - Criar o projeto Angular
npm install -g @angular/cli
ng new ReteSocialis.Web --routing true --style css
cd ReteSocialis.Web

2 - Instalar dependências adicionais
npm install @microsoft/signalr bootstrap

3 - Executar o frontend
ng serve --open

A aplicação será aberta em:
👉 http://localhost:4200

🐳 Docker — Subindo tudo com um comando

Você pode rodar o backend, o frontend e o SQL Server com Docker Compose.

📄 Exemplo completo de docker-compose.yml

Crie um arquivo chamado docker-compose.yml na raiz do projeto com o conteúdo abaixo:

version: '3.9'

services:
  # 🧩 Banco de dados SQL Server
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: rete_sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Your_password123
    ports:
      - "1433:1433"
    networks:
      - rete_network
    volumes:
      - sql_data:/var/opt/mssql

  # ⚙️ Backend ASP.NET API
  backend:
    build:
      context: ./ReteSocialis.API
      dockerfile: Dockerfile
    container_name: rete_api
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=ReteSocialisDB;User Id=sa;Password=Your_password123;TrustServerCertificate=True;
      - JwtSettings__SecretKey=chave_super_secreta_para_o_jwt
      - JwtSettings__Issuer=ReteSocialis.API
      - JwtSettings__Audience=ReteSocialis.Web
    depends_on:
      - sqlserver
    ports:
      - "5000:8080"
    networks:
      - rete_network

  # 💻 Frontend Angular
  frontend:
    build:
      context: ./ReteSocialis.Web
      dockerfile: Dockerfile
    container_name: rete_web
    ports:
      - "4200:80"
    depends_on:
      - backend
    networks:
      - rete_network

networks:
  rete_network:

volumes:
  sql_data:


🚀 Para executar:
docker-compose build
docker-compose up -d

Acesse:
- Frontend → http://localhost:4200
- Backend → http://localhost:5000/swagger
- Banco de dados → localhost,1433

🔍 Verificar se as portas estão em uso
netstat -ano | findstr :5000
netstat -ano | findstr :4200

Se aparecer algo como:
TCP    127.0.0.1:4200    0.0.0.0:0    LISTENING    11448

Finalize o processo:
taskkill /PID 11448 /F

🔐 Configuração do JWT

No arquivo appsettings.json:
"JwtSettings": {
  "SecretKey": "sua_chave_super_segura_aqui",
  "Issuer": "ReteSocialis.API",
  "Audience": "ReteSocialis.Web"
}

🧪 Endpoints principais
| Método | Rota                                  | Descrição                       |
| ------ | ------------------------------------- | ------------------------------- |
| `POST` | `/api/auth/register`                  | Cadastro de novo usuário        |
| `POST` | `/api/auth/login`                     | Login e geração do token JWT    |
| `GET`  | `/api/account/me`                     | Retorna dados do usuário logado |
| `PUT`  | `/api/account/edit`                   | Atualiza nome, e-mail ou senha  |
| `GET`  | `/api/friends`                        | Lista amigos do usuário         |
| `POST` | `/api/friends/invite`                 | Envia convite de amizade        |
| `PUT`  | `/api/friends/accept/{invitationKey}` | Aceita convite                  |
| `Hub`  | `/hubs/feed`, `/hubs/friends`         | Comunicação em tempo real       |

🐞 Observações

⚠️ O projeto ainda está em desenvolvimento e possui alguns bugs conhecidos que serão corrigidos em versões futuras.

🧑‍💼 Autor

ReteSocialis Project
Desenvolvido com ❤️ usando ASP.NET Core + Angular

📄 Licença

Este projeto é distribuído sob a licença MIT.
Você pode usá-lo e modificá-lo livremente, desde que mantenha os créditos originais.