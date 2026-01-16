# 🏢 Enterprise Supplier Manager

Solução desenvolvida para o **gerenciamento de relações entre Empresas e Fornecedores**, com foco em **integridade fiscal, organização de dados mestres (MDM)** e **arquitetura escalável**.

---

## 📌 Visão Geral

O **Enterprise Supplier Manager** é uma plataforma de **gestão centralizada de dados mestres (MDM)**, projetada para otimizar o controle de:

- Fornecedores  
- Unidades de negócio (Empresas)

A solução combina uma **interface objetiva e funcional** com uma **arquitetura backend robusta**, garantindo confiabilidade, consistência de dados e conformidade fiscal/tributária.

---

## 🚀 Tecnologias Utilizadas

### 🎨 Frontend

- **Angular (v18/19)**
  - Standalone Components
  - Signals para gerenciamento de estado reativo
  - Novo Control Flow (`@if`, `@for`)
- **Angular Material**
  - Tabelas, inputs, datepickers e snackbars
- **Bootstrap 5**
  - Grid system e utilitários para responsividade
- **Ngx-mask**
  - Máscaras dinâmicas para CPF, CNPJ e CEP

---

### ⚙️ Backend

- **ASP.NET Core (.NET 8/9)**
  - Web APIs baseadas em Clean Architecture
- **C#**
  - Uso de Records, DTOs e validações estritas
- **SQL Server**
  - Banco de dados relacional para dados mestres
- **Entity Framework Core**
  - ORM para mapeamento e persistência de dados

---

## 🏗️ Arquitetura e Padrões

### 🧱 Backend — Clean Architecture

A solução backend é estruturada em camadas bem definidas, garantindo **desacoplamento**, **manutenibilidade** e **testabilidade**:

- **Domain**
  - Entidades de negócio
  - Interfaces
  - Exceções de domínio  
  - Camada central, independente de frameworks

- **Application**
  - Lógica de negócio
  - DTOs (Data Transfer Objects)
  - Mapeamentos e validações

- **Infrastructure**
  - Persistência de dados (`DbContext`, Repositórios)
  - Integrações externas (ex: API ViaCEP)

- **WebAPI**
  - Controllers
  - Injeção de dependência
  - Middlewares (tratamento global de exceções)

---

### ⚡ Frontend — Reactive Architecture

- **Generic Components**
  - Implementação do `GenericTable<T>`
  - Componente agnóstico ao tipo de dado
  - Renderização dinâmica via interface de configuração de colunas

- **Signals State Management**
  - Gerenciamento de estado com Signals
  - Atualizações granulares da UI
  - Eliminação de ciclos pesados de change detection

- **Unified Error Handling**
  - `UiService` centraliza o tratamento de erros da API
  - Parsing de validações complexas do ASP.NET
  - Exibição de erros via `MatSnackBar`

---

## 🧪 Como Testar o Sistema

### 🖥️ Testes de Fluxo (UI)

- **Navegação**
  - Utilize o menu lateral ou as rotas diretas:
    - `/suppliers`
    - `/companies`

#### Cadastro de Fornecedor

- **Pessoa Física**
  - Exibe campos de **RG** e **Data de Nascimento**
- **Pessoa Jurídica**
  - Oculta campos pessoais
- **Consulta de CEP**
  - Informe um CEP válido
  - Ao sair do campo (blur), a integração com a API ViaCEP é acionada
- **Máscaras**
  - CPF: 11 dígitos
  - CNPJ: 14 dígitos

---

### 🔌 Testes de API (Backend)

- **Swagger**
  - Acesse:
    ```
    https://localhost:PORTA/swagger
    ```

- **Validações**
  - Envie um `SupplierRequestDTO` sem o campo `Document`
  - Verifique se o erro retornado pelo .NET é exibido corretamente no `SnackBar` do Angular

---

## 📄 Licença

Projeto desenvolvido para fins educacionais e corporativos internos.
