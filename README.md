# TMB-Challenge

Este projeto é um serviço em segundo plano que processa mensagens de uma fila do Azure Service Bus para gerenciar pedidos.

## Pré-requisitos

Antes de começar, certifique-se de ter os seguintes componentes instalados:

- [.NET SDK](https://dotnet.microsoft.com/download) (versão 9.0)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli) (opcional, mas recomendado para gerenciar recursos do Azure)
- Uma conta no [Azure](https://azure.microsoft.com/)
- Um servidor de banco de dados compatível (por exemplo, SQL Server, PostgreSQL)
- [Entity Framework Core CLI](https://docs.microsoft.com/ef/core/cli/dotnet) para gerenciar migrações

## Configuração do Azure Service Bus

1. **Crie um Namespace do Service Bus**:
   - Acesse o [Portal do Azure](https://portal.azure.com/).
   - Crie um novo namespace do Service Bus.

2. **Crie uma Fila**:
   - Dentro do namespace do Service Bus, crie uma nova fila onde as mensagens de pedido serão enviadas.

3. **Obtenha as Credenciais de Acesso**:
   - Vá para "Políticas de acesso compartilhado" no namespace do Service Bus.
   - Copie o `Cadeia de conexão primária` de uma das políticas, como `RootManageSharedAccessKey`.

## Configuração do Banco de Dados

1. **Configurar o Banco de Dados**:
   - Crie um banco de dados no seu servidor de banco de dados.

2. **Configurar a String de Conexão**:
   - Atualize o arquivo `appsettings.json` com a string de conexão do seu banco de dados:

     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "<SUA_STRING_DE_CONEXAO>"
       },
       "AzureServiceBusOptions": {
         "ConnectionString": "<SUA_CONNECTION_STRING>",
         "QueueName": "<NOME_DA_FILA>"
       }
     }
     ```

   - Substitua `<SUA_STRING_DE_CONEXAO>` pela string de conexão do seu banco de dados.

3. **Aplicar Migrações**:
   - Certifique-se de que o Entity Framework Core CLI esteja instalado.
   - No diretório do projeto, execute o seguinte comando para aplicar as migrações:

     ```bash
     dotnet ef database update
     ```

   - Isso criará as tabelas necessárias no banco de dados.

## Configuração do Projeto

1. **Clone o Repositório**:
   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd <NOME_DO_PROJETO>
















Adicionar a ferramenta de migração:

bash


dotnet tool install --global dotnet-ef
Adicionar a primeira migração e atualizar o banco de dados:

bash


dotnet ef migrations add InitialCreate
dotnet ef database update