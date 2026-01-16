# enterprise-supplier-manager
Solução desenvolvida para o gerenciamento de relações entre Empresas e Fornecedores.

## Enterprise Supplier Manager
O Enterprise Supplier Manager é uma plataforma de gestão centralizada de dados mestres (MDM) desenvolvida para otimizar o controle de fornecedores e unidades de negócio (empresas). O sistema utiliza uma estética direta integrada a uma arquitetura robusta no backend para garantir integridade fiscal e tributária.

## 🚀 Tecnologias Utilizadas
### Frontend
Angular (v18/19): Utilização de Standalone Components, Signals para gestão de estado reativo e o novo Control Flow (@if, @for).

Angular Material: Componentes de UI como tabelas, inputs, datepickers e snacks de notificação.

Bootstrap 5: Sistema de Grid e utilitários para responsividade acelerada.

Ngx-mask: Máscaras dinâmicas para CPF, CNPJ e CEP.

### Backend
ASP.NET Core (.NET 8/9): Web APIs construídas sob os princípios da Clean Architecture.

C#: Linguagem principal utilizando Records, DTOs e validações estritas.

SQL Server: Banco de dados relacional para armazenamento de dados mestres.

Entity Framework Core: ORM para mapeamento e persistência de dados.

## 🏗️ Arquitetura e Padrões
### Backend (Clean Architecture)
A solução backend é dividida em camadas lógicas para garantir o desacoplamento e a testabilidade:

Domain: Contém as entidades de negócio, interfaces e exceções de domínio. É o núcleo do sistema, independente de frameworks externos.

Application: Implementa a lógica de negócio, DTOs (Data Transfer Objects) para contratos de entrada/saída e mapeamentos de dados.

Infrastructure: Responsável pela persistência (DbContext, Repositórios) e integrações externas (como a consulta ao ViaCEP).

WebAPI: Camada de entrada que gerencia os Controllers, injeção de dependência e Middlewares para tratamento global de exceções.

### Frontend (Reactive Architecture)
Generic Components: Implementação do GenericTable<T>, um componente agnóstico ao tipo de dado que renderiza colunas dinamicamente através de uma interface de configuração de colunas.

Signals State Management: O estado da aplicação (como listas de fornecedores e empresas) é gerenciado via Signals, permitindo atualizações granulares da UI sem a necessidade de ciclos de detecção de mudança pesados.

Unified Error Handling: O UiService centraliza o tratamento de erros da API, realizando o parsing de objetos de validação complexos vindos do ASP.NET e apresentando-os via MatSnackBar.

## 🧪 Como Testar o Sistema
### Testes de Fluxo (UI)
Navegação: Utilize o menu lateral (ou rotas diretas /suppliers e /companies) para alternar entre os módulos.

Cadastro de Fornecedor:

  Selecione Pessoa Física: O formulário deve exibir campos de RG e Data de Nascimento.
  
  Selecione Pessoa Jurídica: O formulário deve ocultar campos pessoais.
  
  Consulta de CEP: Digite um CEP válido e saia do campo (blur) para verificar a integração com a API ViaCEP.
  
  Máscaras: Insira documentos para validar a formatação automática de CPF (11 dígitos) e CNPJ (14 dígitos).

### Testes de API (Backend)
Swagger: Acesse https://localhost:PORTA/swagger para visualizar todos os endpoints.

Validações: Tente enviar um SupplierRequestDTO sem o campo Document e verifique se o SnackBar do Angular exibe o erro retornado pelo .NET.
