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

## 🛠️ Detalhes da Arquitetura
1. Componentização Genérica
O sistema foi projetado com componentes de alto nível para máxima reutilização:

GenericTable<T>: Tabela abstrata que renderiza dados dinamicamente com base em configurações de colunas.

PageLayout: Estrutura de cabeçalho e container de vidro padronizada para todas as páginas de CRUD.

UiService: Centralização do tratamento de erros do backend .NET, transformando objetos de validação em notificações amigáveis.

2. Integração Reativa
A comunicação entre o frontend e a API utiliza Signals, garantindo que a interface se atualize instantaneamente após operações de Delete, Create ou Update sem a necessidade de recarregar a página.

## 🧪 Como Testar o Sistema
Testes de Fluxo (UI)
Navegação: Utilize o menu lateral (ou rotas diretas /suppliers e /companies) para alternar entre os módulos.

Cadastro de Fornecedor:

Selecione Pessoa Física: O formulário deve exibir campos de RG e Data de Nascimento.

Selecione Pessoa Jurídica: O formulário deve ocultar campos pessoais.

Consulta de CEP: Digite um CEP válido e saia do campo (blur) para verificar a integração com a API ViaCEP.

Máscaras: Insira documentos para validar a formatação automática de CPF (11 dígitos) e CNPJ (14 dígitos).

Testes de API (Backend)
Swagger: Acesse https://localhost:PORTA/swagger para visualizar todos os endpoints.

Validações: Tente enviar um SupplierRequestDTO sem o campo Document e verifique se o SnackBar do Angular exibe o erro retornado pelo .NET.
