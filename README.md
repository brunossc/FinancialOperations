# Operações Financeiras:
A Aplicação consiste em 2 serviços, é necessário ter o Docker instalado na máquina:


![image](https://github.com/user-attachments/assets/2c97c94e-823d-4ae4-af7a-82cc47171fcd)


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

Melhor executar com o Visual Studio Community 2022 e selecionar o projeto do docker-compose como inicial.

# Considerações

Projetos das Libs:

 - FinancialOperations.SideCar: Desacoplamento da integração com o serviço de monitoramento (ElasticSearch + Kibana)
 - FinancialOperations.MQ.Events: Masstransit necessita de uma origem única(namespace) de menssagens.

Pontos de observação a serem melhorados:

 - Controle Transacional => https://www.mongodb.com/docs/drivers/csharp/upcoming/fundamentals/transactions/
 - Mapeamento com Mapster
 - FluentValidator
 - Controle de acesso
 - Serviços de redundancia para falhas (Masstransit já aplica as boas praticas para ACK e NACK como por exemplo: fila de erro)
 - Notificação de Falhas
 - Controle de eventos
 - Versionamento
 - Criar uma classe base para o repository para remover a definição do database
