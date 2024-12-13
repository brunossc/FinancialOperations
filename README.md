# Operações Financeiras:
A Aplicação consiste em 2 serviços, é necessário ter o Docker instalado na máquina:

Serviços:

- FinancialOperations.API => pode inserir operações de débito e de crédito, com banco de dados MongoDB.

  Swagger: https://localhost:55028/swagger/index.html

  Endpoints:

  https://localhost:55028/addCredit?value=25

  https://localhost:55028/addDebit?value=35

  Segue Postman Collection na pasta Documents: Teste.postman_collection.json

- FinancialOperations.Consolidator.API(gRPC) => Disponibiliza o consolidado do dia das operações de crédito e débito efetuadas pelo serviço FinancialOperations.API, com banco de dados MongoDB.

  Documento de evicencia do Postman do gRPC(não tem opção para exportar), na pasta Documents: gRPC_Evidence.png

- Arquitetura

  Documento da Arquitetura no modelo C4 (Draw.io), na pasta Documents: OperacoesFinanceiras_Arquitetura.drawio 

Considerando alguns ambientes é melhor executar com o Visual Studio Community 2022 e selecionar o projeto do docker-compose como inicial.
