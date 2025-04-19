# 🏍️ MotoLocadora

Sistema de locação de motos desenvolvido com foco em arquitetura limpa, boas práticas e escalabilidade. Este projeto foi inicialmente criado como parte de um teste técnico e posteriormente aprimorado para servir como portfólio técnico.

## 🚀 Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Entity Framework Core](https://learn.microsoft.com/pt-br/ef/core/)
- [RabbitMQ](https://www.rabbitmq.com/)
- [Serilog](https://serilog.net/)
- [Docker](https://www.docker.com/)
- [JWT](https://jwt.io/)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [Redis](https://redis.io/) *(planejado)*
- [Elasticsearch](https://www.elastic.co/elasticsearch/) *(planejado)*
- [GitHub Actions](https://github.com/features/actions) *(planejado)*

## 🧱 Padrões e Arquitetura

- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Mensageria assíncrona com RabbitMQ
- Autenticação e autorização via JWT
- Validações com FluentValidation
- Logging estruturado com Serilog

## 📦 Como Executar Localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)

### Passos

1. Clone o repositório:

   ```bash
   git clone https://github.com/davidsou/MotoLocadora.git
   cd MotoLocadora
   ```

2. Execute os containers com Docker:

   ```bash
   docker-compose up -d
   ```

3. Acesse a API em `http://localhost:5000`

## 🧪 Testes

*(Em desenvolvimento)*

- Testes unitários com xUnit
- Cobertura de código com Coverlet
- Integração contínua com GitHub Actions

## 📌 Roadmap

- [ ] Integração com Elasticsearch para logs
- [ ] Implementação de cache híbrido (MemoryCache + Redis)
- [ ] Testes unitários e de integração
- [ ] Pipeline CI/CD com GitHub Actions

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

## 👤 Autor

- **David Soares** – [GitHub](https://github.com/davidsou) | [LinkedIn](https://www.linkedin.com/in/david-soares-a4296317)

