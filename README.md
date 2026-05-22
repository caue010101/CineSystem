# CineList API

API RESTful para gerenciamento de filmes favoritos, desenvolvida com ASP.NET Core 8, Clean Architecture e integração com a API do TMDB.

## 🚀 Tecnologias

- **C# / ASP.NET Core 8**
- **Dapper** — micro ORM para acesso a dados
- **PostgreSQL** — banco de dados relacional
- **JWT** — autenticação e autorização
- **BCrypt** — hash de senhas
- **Serilog** — logging estruturado
- **xUnit + Moq + FluentAssertions** — testes unitários
- **TMDB API** — integração com catálogo de filmes

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture**, dividido em 4 camadas:

```
CineList/
├── CineList.API/           # Controllers, configuração, middlewares
├── CineList.Application/   # Use Cases, Interfaces, DTOs, Services
├── CineList.Domain/        # Entidades, Interfaces de Repositório
├── CineList.Infrastructure/# Repositórios, UoW, TmdbService, JwtService
└── CineList.Tests/         # Testes unitários
```

## 📐 Padrões e Decisões Técnicas

- **Unit of Work** — garante atomicidade nas operações que envolvem múltiplas tabelas (ex: salvar filme + registrar favorito)
- **Repository Pattern** — abstração do acesso a dados via interfaces
- **Global Exception Handler** — tratamento centralizado de erros com mapeamento de status codes
- **JWT via Claims** — o `userId` é extraído diretamente do token, impedindo acesso a dados de outros usuários
- **Cache de banco** — antes de chamar a API do TMDB, verifica se o filme já existe localmente

## 🗄️ Banco de Dados

```sql
users           — cadastro de usuários
movies          — filmes obtidos via TMDB
user_favorites  — relacionamento usuário ↔ filme
```

## 📡 Endpoints

### Auth
| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/auth/login` | Autenticação e geração de token JWT |

### Users
| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| POST | `/api/user` | Cadastro de usuário | ❌ |
| GET | `/api/user/me` | Dados do usuário autenticado | ✅ |
| PUT | `/api/user` | Atualização de dados | ✅ |
| DELETE | `/api/user` | Exclusão de conta | ✅ |

### Movies
| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| GET | `/api/movie/search?movie={title}` | Busca filmes por título | ✅ |

### Favorites
| Método | Rota | Descrição | Auth |
|--------|------|-----------|------|
| POST | `/api/userfavorites/{tmdbId}` | Favoritar um filme | ✅ |
| GET | `/api/userfavorites` | Listar filmes favoritos | ✅ |
| DELETE | `/api/userfavorites/{tmdbId}` | Desfavoritar um filme | ✅ |

## ⚙️ Como Rodar

### Pré-requisitos

- .NET 8 SDK
- PostgreSQL
- Conta na [TMDB API](https://www.themoviedb.org/settings/api)

### Configuração

1. Clone o repositório:
```bash
git clone https://github.com/caue010101/CineSystem.git
cd CineSystem
```

2. Configure o `appsettings.json` na camada `CineList.API`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=cinelist;Username=postgres;Password=suasenha"
  },
  "Jwt": {
    "SecretKey": "sua-chave-secreta-minimo-32-caracteres",
    "Issuer": "CineList",
    "Audience": "CineList"
  },
  "Tmdb": {
    "ApiKey": "sua-api-key-do-tmdb",
    "BaseUrl": "https://api.themoviedb.org/3/"
  }
}
```

3. Crie as tabelas no PostgreSQL:
```sql
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL DEFAULT '',
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE movies (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    tmdb_id INTEGER NOT NULL UNIQUE,
    title VARCHAR(200) NOT NULL,
    overview TEXT,
    poster_path VARCHAR(300),
    popularity NUMERIC(10,3) NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE user_favorites (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES users(id),
    movie_id UUID NOT NULL REFERENCES movies(id),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    UNIQUE(user_id, movie_id)
);
```

4. Execute o projeto:
```bash
cd CineList.API
dotnet run
```

5. Acesse o Swagger em `http://localhost:5180/swagger`

### Testes

```bash
cd CineList.Tests
dotnet test
```

## 🔒 Segurança

- Senhas armazenadas com hash BCrypt
- Autenticação via JWT (HS256)
- `userId` extraído das claims do token — impossível acessar dados de outros usuários
- Erros internos não expostos em produção
