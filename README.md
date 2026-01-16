# 🏢 Enterprise Supplier Manager

Plataforma **full-stack** para **gestão centralizada de Empresas e Fornecedores (MDM)**, desenvolvida com foco em **consistência de dados**, **integridade fiscal**, **testabilidade** e **arquitetura escalável**.

O projeto simula um cenário corporativo real, aplicando **Clean Architecture**, **validações robustas**, **testes automatizados** e **boas práticas modernas** tanto no backend quanto no frontend.

---

## 📌 Visão Geral

O **Enterprise Supplier Manager** resolve o problema de **cadastro, manutenção e governança de dados mestres** de fornecedores e empresas, oferecendo:

- Modelo de dados consistente e validado
- Separação clara de responsabilidades
- Preparação para crescimento, auditoria e integração com outros sistemas

A solução foi pensada desde o início para **ambientes corporativos**, onde confiabilidade e evolução contínua são essenciais.

---

## 📦 Instalação e Configuração

Esta seção descreve os passos necessários para configurar o ambiente de desenvolvimento e executar a solução completa localmente.

---

### 1️⃣ Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas em sua estação de trabalho:

- Docker e Docker Compose
- Node.js (v18+) e Angular CLI
- .NET SDK (v8.0 ou v9.0)
- Entity Framework Core Tools

Instalação do EF Core Tools (caso necessário):

```bash
dotnet tool install --global dotnet-ef
```

### 2️⃣ Configuração do Banco de Dados e API (Docker)

A solução utiliza Docker Compose para orquestrar o banco de dados SQL Server 2022 e o serviço de API.

Na raiz do projeto, execute o comando abaixo para subir os containers:

```
  docker-compose up -d
```
### 🔐 Nota de Segurança

- Usuário do SQL Server: sa
- Senha do SQL Server:
- Os dados do banco de dados são persistidos no volume Docker:

```sql_data```

### 3️⃣ Migrations e Base de Dados
```
dotnet ef database update --project src/backend/EnterpriseSupplierManager.Infrastructure
```
Com o container do banco de dados ativo, aplique as Migrations do Entity Framework Core para criar o schema do banco de dados DbEnterpriseSupplier.

### 4️⃣ Execução do Frontend (Angular)

O frontend deve ser executado localmente para facilitar o ciclo de desenvolvimento e depuração.

```bash
# Navegue até a pasta do frontend
cd src/frontend/EnterpriseSupplierManager-UI

# Instale as dependências
npm install

# Inicie o servidor de desenvolvimento
ng serve
````

Acesse a aplicação em:
http://localhost:4200

## 🛠️ Detalhes de Conectividade

A tabela abaixo resume os serviços expostos pela solução, suas portas padrão e finalidades durante o desenvolvimento local.

| Serviço               | Porta | Descrição |
|-----------------------|-------|-----------|
| SQL Server            | 1433  | Banco de dados relacional (Instância: `sql-server-dev-bh`) |
| Web API (HTTP)        | 8080  | Endpoint principal para consumo do frontend |
| Web API (HTTPS)       | 8081  | Endpoint seguro para desenvolvimento |
| Swagger UI            | `/swagger` | Documentação interativa dos contratos de Empresas e Fornecedores |

### Observações

- O acesso ao **Swagger UI** é feito através da URL base da API, por exemplo:
- As portas podem ser ajustadas no arquivo `docker-compose.yml`, caso necessário.
- O frontend Angular consome a API via HTTP/HTTPS conforme configuração do ambiente.

## ✨ Principais Funcionalidades

### 📦 Gestão de Fornecedores (Supplier MDM)
- Cadastro e manutenção de fornecedores
- Suporte a **Pessoa Física** e **Pessoa Jurídica**
- Validação rigorosa de documentos (CPF / CNPJ)
- **Soft Delete** para preservação histórica dos dados

### 🏢 Gestão de Empresas
- Cadastro de empresas (unidades de negócio)
- Relacionamento **N:N (Many-to-Many)** entre Empresas e Fornecedores
- Estrutura preparada para expansão organizacional

### 📍 Integração de Endereços
- Consulta automática de endereço via **API ViaCEP**
- Infraestrutura desacoplada para serviços externos

### 🛡️ Validações e Confiabilidade
- **FluentValidation** aplicado na camada de Application
- Regras de integridade de domínio bem definidas
- Padronização de erros para consumo pelo frontend

### ⚠️ Tratamento Global de Erros
- Middleware de **Global Exception Handling**
- Contratos de erro consistentes entre API e UI
- Melhor experiência para o usuário final

---

## 🧪 Qualidade e Testes

- **Testes unitários abrangentes** nas camadas:
  - Domain
  - Application
- Garantia de regras de negócio isoladas de infraestrutura
- Projeto orientado à **alta testabilidade**

---

## 🚀 Tecnologias Utilizadas

### 🎨 Frontend

- **Angular (v18/19)**
  - Standalone Components
  - Signals para estado reativo
  - Novo Control Flow (`@if`, `@for`)
- **Angular Material**
  - Tabelas, formulários, datepickers e snackbars
- **Bootstrap 5**
  - Grid system e responsividade
- **Ngx-mask**
  - Máscaras dinâmicas para CPF, CNPJ e CEP

---

### ⚙️ Backend

- **ASP.NET Core (.NET 8/9)**
  - APIs seguindo Clean Architecture
- **C#**
  - Records, DTOs e tipagem forte
- **Entity Framework Core**
  - ORM e migrations
- **SQL Server**
  - Persistência de dados mestres
- **FluentValidation**
  - Validações declarativas e reutilizáveis

---

## 🏗️ Arquitetura

### Backend — Clean Architecture

Estrutura dividida para máximo desacoplamento:

- **Domain**
  - Entidades (`Company`, `Supplier`)
  - Regras de negócio
  - Contratos e exceções

- **Application**
  - Casos de uso
  - DTOs e validações
  - Orquestração da lógica de negócio

- **Infrastructure**
  - EF Core
  - Repositórios
  - Integração ViaCEP
  - Configuração de Soft Delete

- **WebAPI**
  - Controllers
  - Dependency Injection
  - Middlewares (Exception Handling)

---

### Frontend — Arquitetura Reativa

- **GenericTable<T>**
  - Componente genérico reutilizável
  - Renderização dinâmica via configuração de colunas

- **State Management com Signals**
  - Atualizações granulares
  - Menor acoplamento e melhor performance

- **Tratamento Unificado de Erros**
  - Parsing de erros vindos do ASP.NET
  - Exibição padronizada via `MatSnackBar`

---

## 🐳 Infraestrutura

- **Docker**
  - Ambiente isolado para backend e banco de dados
- **Migrations automatizadas**
  - Inicialização rápida do ambiente

---

## 🧪 Como Testar

### UI
- Rotas:
  - `/suppliers`
  - `/companies`
- Testar:
  - Alternância entre PF / PJ
  - Consulta de CEP
  - Máscaras e validações

### API
- Swagger disponível em:
- Testar envio de requests inválidos para validar o tratamento de erros

---

## 🎯 Objetivo do Projeto

Este projeto foi desenvolvido com foco em:

- Simular **cenários reais de software corporativo**
- Demonstrar domínio de **Clean Architecture**
- Aplicar **boas práticas modernas** de frontend e backend
- Servir como **base escalável** para sistemas MDM

---

## 📄 Licença

Projeto de uso educacional e demonstrativo.
