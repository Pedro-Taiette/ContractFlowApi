# ContractFlow API

Uma API simples em .NET 8 para gerenciamento de contratos, fornecedores, unidades organizacionais, obrigações, entregáveis e alertas.

O backend é construído com:
- ASP.NET Core 8 (C#)
- Entity Framework Core (Code-First)
- SQL Server
- Swagger (para documentação da API)

---

## 🚀 Início Rápido

### 0. Rodar tudo com Docker (recomendado para o MVP)

Se não quiser instalar SDKs localmente, basta ter **Docker** e **Docker Compose**. Na raiz do repositório execute:

```bash
docker compose up --build
```

O backend sobe na porta `8080` (http://localhost:8080/swagger) e já inclui um SQL Server pronto para uso. Os uploads ficam persistidos no volume nomeado `uploads`.

> A string de conexão e demais variáveis já estão configuradas no `docker-compose.yml`. Ajuste as portas se alguma estiver ocupada.

---

### 1. Pré-requisitos

Certifique-se de ter instalado:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Azure Data Studio](https://azure.microsoft.com/en-us/products/data-studio) ou [SSMS](https://aka.ms/ssms) (opcional, para visualizar o banco)

---

### 2. Configurar o Banco de Dados

Edite a connection string no arquivo:

```

appsettings.json

````

Exemplo (autenticação integrada):
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ContractFlowDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
````

Exemplo (autenticação com usuário e senha):

```json
"Server=localhost;Database=ContractFlowDB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True;"
```

---

### 3. Configuração do Entity Framework Core

Instale o CLI do Entity Framework (somente uma vez por máquina):

```bash
dotnet tool install --global dotnet-ef
```

Garanta que o projeto principal tenha os pacotes NuGet necessários:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
```

---

### 4. Migrations e Atualização do Banco

Para aplicar o schema atual no banco de dados:

```bash
dotnet ef database update
```

Para recriar o banco do zero (caso esteja bagunçado):

```bash
dotnet ef database drop -f
dotnet ef database update
```

Para criar uma nova migration quando alterar modelos:

```bash
dotnet ef migrations add NomeDaMigration
```

Para listar todas as migrations:

```bash
dotnet ef migrations list
```

Para gerar o script SQL manualmente (útil para DBAs):

```bash
dotnet ef migrations script -o update.sql
```

---

### 5. Executar a Aplicação

A partir da pasta principal da API:

```bash
dotnet run
```

Depois acesse o Swagger UI:

```
https://localhost:51624/swagger
```

---

### 6. Seed de Dados

A API cria automaticamente dados de demonstração quando o banco está vazio:

* 1 fornecedor de exemplo
* 1 unidade organizacional de exemplo
* 1 contrato com uma obrigação e um entregável

Essa lógica está em:
`Data/SeedData.cs`

---

### 7. Erros Comuns

**Erro:** `Invalid object name 'Alerts'`
→ Solução:

```bash
dotnet ef database update
```
**Erro:** `Your startup project doesn't reference Microsoft.EntityFrameworkCore.Design`
→ Solução:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

---

### 8. Estrutura de Pastas

```
ContractFlowApi/
├── Controllers/       → Endpoints da API
├── Models/            → Entidades e Value Objects
├── Data/              → DbContext e SeedData
├── Migrations/        → Arquivos gerados pelo EF Core
├── Properties/
├── appsettings.json
└── Program.cs
```

---

### 9. Comandos Úteis do EF Core

| Ação                   | Comando                                                   |
| ---------------------- | --------------------------------------------------------- |
| Criar migration        | `dotnet ef migrations add NomeDaMigration`                |
| Aplicar migrations     | `dotnet ef database update`                               |
| Dropar e recriar banco | `dotnet ef database drop -f && dotnet ef database update` |
| Listar migrations      | `dotnet ef migrations list`                               |
| Gerar script SQL       | `dotnet ef migrations script -o update.sql`               |
| Executar aplicação     | `dotnet run`                                              |

---

### 10. Dicas Importantes para o Time

* **Não edite migrations manualmente.**
* Sempre rode `dotnet build` antes de subir código.
* Use `drop` e `update` com frequência para garantir um banco limpo durante o desenvolvimento.
* Teste os endpoints sempre pelo Swagger.
* **CORS liberado**: a API aceita requisições de qualquer origem (útil para o time de frontend testar rapidamente).
* Em caso de erro, leia o console — geralmente o problema é de migration ou conexão.

---

**Autor:** Equipe ContractFlow
**Linguagem:** C# (.NET 8)
**Banco de Dados:** SQL Server
