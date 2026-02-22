# Sistema de Controle de Gastos Residenciais

## Visão geral
Este sistema implementa um controle de gastos residenciais com:
- **Cadastro de Pessoas** (criar, editar, excluir, listar)
- **Cadastro de Categorias** (criar, listar)
- **Cadastro de Transações** (criar, listar)
- **Relatório de Totais por Pessoa** (receitas, despesas e saldo, com total geral)

A solução é separada em:
- **Back-end**: ASP.NET Core Web API (.NET) + EF Core + SQLite
- **Front-end**: React + TypeScript consumindo a API via HTTP

O banco **SQLite** foi escolhido para garantir persistência local (arquivo `.db`) sem depender de infraestrutura externa, mantendo os dados mesmo após reiniciar a aplicação.

---

## Arquitetura (Clean Architecture)

A solução é organizada em 4 camadas/projetos:

### 1) `Household.Domain`
Contém o núcleo do negócio:
- Entidades (`Person`, `Category`, `Transaction`)
- Enums (`TransactionType`, `CategoryPurpose`)
- Regras “estruturais” (ex.: limites de tamanho via DataAnnotations)

### 2) `Household.Application`
Contém a aplicação/fluxos de negócio:
- DTOs (contratos de entrada/saída)
- Interfaces de repositório (`IPersonRepository`, `ICategoryRepository`, `ITransactionRepository`)
- Serviços de caso de uso (ex.: `TransactionService`)
- Exceções de regra de negócio (`BusinessRuleException`)

### 3) `Household.Infrastructure`
Contém detalhes de implementação:
- `AppDbContext` (EF Core)
- Mapeamentos (ex.: cascade delete)
- Repositórios concretos (implementações das interfaces usando EF/SQLite)

### 4) `Household.Api`
Camada de apresentação HTTP:
- Controllers
- Injeção de dependência (DI)
- Swagger
- Configurações (ConnectionString, JSON)

---

## Persistência e banco (SQLite + EF Core)

### Por que SQLite?
- Persistência garantida com um arquivo local (`household.db`)
- Não exige servidor externo.
- Migrações EF tornam o schema versionado e reproduzível.

### Migrações
A criação/atualização do banco é feita com:

```bash
dotnet ef migrations add InitialCreate --project Household.Infrastructure --startup-project Household.Api
dotnet ef database update --project Household.Infrastructure --startup-project Household.Api
```

---

# Regras de negócio implementadas

## 1) Ao excluir uma pessoa, excluir transações dessa pessoa

Implementado via **Cascade Delete** no **EF Core**.

**Configuração no `OnModelCreating`:**
- `Person` (1) : (N) `Transaction`
- `OnDelete(DeleteBehavior.Cascade)`

**Por que cascade no banco?**
- Garante consistência mesmo se alguém apagar a pessoa por outro caminho
- Evita código manual repetitivo para deletar transações

---

## 2) Menor de idade (< 18) só pode lançar despesa

Implementado no `TransactionService` (camada **Application**):
- Busca a pessoa
- Se `Age < 18` e `Type != Despesa` → bloqueia com erro de regra (`BusinessRuleException`)

**Por que no Service (Application) e não no Controller?**
- Controller é HTTP; regra é negócio.
- A regra precisa consultar dados (idade da pessoa), então não deve ficar “presa” ao UI/HTTP.

---

## 3) Categoria compatível com tipo de transação

**Regra:**
- Se transação é **Despesa**, categoria deve ser **Despesa** ou **Ambas**
- Se transação é **Receita**, categoria deve ser **Receita** ou **Ambas**

Implementado no `TransactionService`:
- Busca categoria
- Valida compatibilidade antes de salvar

**Por que assim?**
- Evita inconsistência (transação despesa em categoria receita)
- Mantém coerência do relatório/agrupamentos

---

## 4) Valor da transação deve ser positivo

Implementado com `Range` e validação no DTO/entidade:
- `Value > 0`

**Por que DataAnnotations?**
- Validação automática com `[ApiController]` (`ModelState`)
- Evita persistir valores inválidos

---

# Endpoints (API)

## Pessoas (`/api/pessoas`)
- `GET /api/pessoas` — Lista pessoas cadastradas.
- `GET /api/pessoas/{id}` — Retorna pessoa por id.
- `POST /api/pessoas` — Cria pessoa.
- `PUT /api/pessoas/{id}` — Edita pessoa.
- `DELETE /api/pessoas/{id}` — Exclui pessoa (e transações via cascade).

## Categorias (`/api/categorias`)
- `GET /api/categorias` — Lista categorias.
- `POST /api/categorias` — Cria categoria.

## Transações (`/api/transacoes`)
- `GET /api/transacoes` — Lista transações (com dados de pessoa e categoria).
- `POST /api/transacoes` — Cria transação aplicando todas as regras de negócio.

## Relatórios (`/api/relatorios`)
- `GET /api/relatorios/totais-por-pessoa` — Lista todas as pessoas com:
  - total de receitas
  - total de despesas
  - saldo (receita - despesa)  
  E ao final traz total geral.

---

# Tratamento de erros e mensagens

- Erros de regra de negócio retornam **400**
- Recurso não encontrado retorna **404**

---

# Front-end (React + TypeScript)

## Organização
- `src/api`: cliente HTTP (`fetch`) e endpoints
- `src/types`: tipagens (DTOs) para garantir consistência com a API
- `src/pages`: telas (Pessoas, Categorias, Transações, Relatório)

---

## Proxy no Vite

Configuração para evitar CORS:
- chamadas para `/api/*` são encaminhadas para a API local
---
