# Digital Bank - Sistema de Contas e Transferências

Este projeto representa um sistema bancário digital modular, focado em operações de conta corrente, transferências e tarifas. Ele é construído com base nos princípios da Clean Architecture e Domain-Driven Design (DDD), utilizando .NET 8 e uma abordagem orientada a eventos com Kafka.

## 🚀 Visão Geral

O "Digital Bank" é um sistema distribuído modular, que gerencia as principais funcionalidades de um banco digital. Ele é dividido em domínios de negócio claros, cada um com suas próprias responsabilidades e APIs.

### Domínios Principais:

*   **ContaCorrente**: Gerencia o cadastro de contas, saldos e movimentações (depósitos/saques).
*   **Transferencia**: Orquestra e processa transferências de fundos entre contas.
*   **Tarifa**: Aplica e gerencia tarifas sobre operações bancárias.

### Arquitetura:

A solução segue a **Clean Architecture**, promovendo uma forte separação de responsabilidades, baixo acoplamento e alta testabilidade. As dependências fluem das camadas externas (API) para as internas (Domínio), garantindo que as regras de negócio permaneçam independentes da infraestrutura.

## 🛠️ Tecnologias Utilizadas

*   **Linguagem**: C# 12
*   **Framework**: .NET 8 (ASP.NET Core)
*   **Padrões de Desi  gn**: Clean Architecture, CQRS (Command Query Responsibility Segregation), DDD (Domain-Driven Design), Result Pattern
*   **Comunicação Interna**: MediatR (para orquestração de comandos e queries dentro de cada domínio)
*   **Comunicação Assíncrona/Eventos**: KafkaFlow
*   **Banco de Dados**: SQLite (para simplicidade em desenvolvimento, facilmente substituível)
*   **ORM/Acesso a Dados**: Dapper
*   **Autenticação**: JWT (JSON Web Tokens)
*   **Controle de Acesso**: Middleware de Idempotência
*   **Containerização**: Docker
*   **Documentação API**: Swagger/OpenAPI

## 📂 Estrutura da Solução

A solução `digital-bank.sln` é organizada de forma modular, refletindo uma arquitetura de microsserviços bem-definido. Cada domínio de negócio (`ContaCorrente`, `Transferencia`, `Tarifa`) possui sua própria estrutura de camadas, aderindo aos princípios da Clean Architecture. Além disso, há projetos compartilhados para abstrações e infraestrutura comuns.

### Visão Geral das Camadas (Clean Architecture)

A hierarquia de dependências segue rigorosamente o padrão: `Api -> Infrastructure -> Application -> Domain -> Core`.

### Detalhamento dos Projetos:

1.  **Projetos Compartilhados (Core)**
    *   **`Core`**:
        *   **Propósito**: Contém abstrações genéricas, utilitários, tipos base (`ApiResult`, `ErrorDetails`), exceções, extensões e interfaces que são independentes de qualquer tecnologia de infraestrutura.
        *   **Características**: É a camada mais interna e fundamental, sem dependências externas, garantindo que as regras de negócio e tipos comuns sejam puros.
        *   *Dependências*: Nenhuma.
    *   **`Core.Infrastructure`**:
        *   **Propósito**: Agrupa implementações de infraestrutura que são comuns e reutilizáveis entre os domínios.
        *   **Conteúdo**: Inclui middlewares (ex: Idempotência, Exception Handling), serviços de segurança (JWT, criptografia), extensões para configuração de DI e abstrações para acesso a banco de dados genérico.
        *   *Dependências*: `Core`.

2.  **Módulo `ContaCorrente`**
    *   **`ContaCorrente.Api`**:
        *   **Propósito**: A camada de apresentação para o domínio de Conta Corrente.
        *   **Conteúdo**: Expõe endpoints RESTful para operações como cadastro, inativação e movimentação de contas, além de login. É o ponto de entrada HTTP para o domínio.
        *   *Dependências*: `ContaCorrente.Application`, `ContaCorrente.Infrastructure`, `Core.Infrastructure`.
    *   **`ContaCorrente.Application`**:
        *   **Propósito**: Define os casos de uso do domínio de Conta Corrente.
        *   **Conteúdo**: Contém Commands e Queries (MediatR), DTOs (Request/Response), validadores e interfaces para gateways externos. Orquestra a lógica de negócio.
        *   *Dependências*: `ContaCorrente.Domain`, `Core`.
    *   **`ContaCorrente.Domain`**:
        *   **Propósito**: O coração do domínio de Conta Corrente, contendo as regras de negócio essenciais.
        *   **Conteúdo**: Entidades de negócio (`ContaCorrenteEntity`, `MovimentoEntity`), value objects e interfaces de repositório específicas para Conta Corrente.
        *   *Dependências*: `Core`.
    *   **`ContaCorrente.Infrastructure`**:
        *   **Propósito**: Implementa as interfaces de repositório e serviços de infraestrutura para o domínio de Conta Corrente.
        *   **Conteúdo**: Utiliza Dapper para persistência de dados (SQLite) e KafkaFlow para mensageria. Também configura a injeção de dependência específica para este domínio.
        *   *Dependências*: `ContaCorrente.Application`, `ContaCorrente.Domain`, `Core.Infrastructure`.

3.  **Módulo `Transferencia`**
    *   **`Transferencia.Api`**:
        *   **Propósito**: A camada de apresentação para o domínio de Transferência.
        *   **Conteúdo**: Expõe endpoints RESTful para iniciar e consultar transferências.
        *   *Dependências*: `Transferencia.Application`, `Transferencia.Infrastructure`, `Core.Infrastructure`.
    *   **`Transferencia.Application`**:
        *   **Propósito**: Define os casos de uso para o domínio de Transferência.
        *   **Conteúdo**: Inclui Commands, Queries, DTOs, validadores e interfaces para comunicação com outros domínios (ex: `IContaCorrenteApiGateway`).
        *   *Dependências*: `Transferencia.Domain`, `Core`.
    *   **`Transferencia.Domain`**:
        *   **Propósito**: Contém as entidades de negócio e regras para o domínio de Transferência.
        *   **Conteúdo**: Entidades (`TransferenciaEntity`), value objects e interfaces de repositório.
        *   *Dependências*: `Core`.
    *   **`Transferencia.Infrastructure`**:
        *   **Propósito**: Implementa os repositórios, gateways HTTP e consumidores/produtores KafkaFlow para o domínio de Transferência.
        *   **Conteúdo**: Lida com a persistência de dados e a comunicação com a API de Conta Corrente e o barramento de mensagens.
        *   *Dependências*: `Transferencia.Application`, `Transferencia.Domain`, `Core.Infrastructure`.

4.  **Módulo `Tarifa`**
    *   **`Tarifa.Api`**:
        *   **Propósito**: A camada de apresentação para o domínio de Tarifa.
        *   **Conteúdo**: Expõe endpoints RESTful relacionados à aplicação e consulta de tarifas.
        *   *Dependências*: `Tarifa.Application`, `Tarifa.Infrastructure`, `Core.Infrastructure`.
    *   **`Tarifa.Application`**:
        *   **Propósito**: Define os casos de uso para o domínio de Tarifa.
        *   **Conteúdo**: Inclui Commands, Queries, DTOs e validadores.
        *   *Dependências*: `Tarifa.Domain`, `Core`.
    *   **`Tarifa.Domain`**:
        *   **Propósito**: Contém as entidades de negócio e regras para o domínio de Tarifa.
        *   **Conteúdo**: Entidades (`TarifaEntity`), value objects e interfaces de repositório.
        *   *Dependências*: `Core`.
    *   **`Tarifa.Infrastructure`**:
        *   **Propósito**: Implementa os repositórios e consumidores/produtores KafkaFlow para o domínio de Tarifa.
        *   **Conteúdo**: Lida com a persistência de dados e o processamento de mensagens de eventos de tarifa.
        *   *Dependências*: `Tarifa.Application`, `Tarifa.Domain`, `Core.Infrastructure`.

## 🚀 Como Executar

### Pré-requisitos

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Docker Desktop](https://www.docker.com/products/docker-desktop/) (necessário para Kafka e para rodar a aplicação em containers)

### Executando com Docker Compose

A forma mais fácil de iniciar todos os serviços (Kafka, Zookeeper e as APIs) é utilizando Docker Compose.

0. **Criar network Docker (caso não existir)**:

    ```bash
    docker network create digitalbank_network
    ```
1.  **Inicie os serviços**:
* Kafka:

    ```bash
    docker-compose -f kafka.yml up -d
* Demais serviços:

    ```bash
    docker-compose -f contacorrente.yml -f transferencia.yml -f tarifa.yml up --build -d
    ```
    *   `--build`: Garante que as imagens Docker das APIs sejam reconstruídas com as últimas alterações do código.
    *   `-d`: Executa os containers em modo detached (em segundo plano).

2.  **Verifique o status dos containers**:
    ```bash
    docker-compose -f kafka.yml -f contacorrente.yml -f transferencia.yml -f tarifa.yml ps
    ```

3.  **Acesse as APIs**:
    Após a inicialização, as APIs estarão disponíveis nos seguintes endereços (padrão do Docker Compose):
    *   **ContaCorrente API**: `http://localhost:7771`
    *   **Transferencia API**: `http://localhost:7772`
    *   **Tarifa API**: `http://localhost:7773`

### Parando os Serviços

Para parar e remover todos os containers e redes criadas pelo Docker Compose:

```bash
docker-compose -f kafka.yml -f contacorrente.yml -f transferencia.yml -f tarifa.yml down
