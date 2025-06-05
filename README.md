# TMB-Challenge

Este projeto é o Back-End do desafio técnico para TMB,
Os detalhes do desafio podem ser vistos no arquivo [POC-TMB](https://github.com/guiriosoficial/TMB-Challenge-Api/blob/main/POC-TMB.pdf)

**FRONT END**
- O Front end deste projeto está disponível em [TMB-Challenge-Web](https://github.com/guiriosoficial/TMB-Challenge-Web)

## Pré-requisitos

Antes de começar, certifique-se de ter os seguintes componentes instalados:

- [Docker](https://www.docker.com/products/docker-desktop/)
- [.NET SDK](https://dotnet.microsoft.com/download) (versão 9.0)
- [Entity Framework Core CLI](https://docs.microsoft.com/ef/core/cli/dotnet) para gerenciar migrações
- Uma conta no [Azure](https://azure.microsoft.com/)

## Configuração do Projeto

1. **Clone o Repositório**:
```bash
git clone git@github.com:guiriosoficial/TMB-Challenge-Api.git
cd TMB-Challenge-Api
```

2. **Endenda a estrutura do projeto**:
```
/Config                 -> Configurações do aplicativo, como configuração de serviços externos (Ex. Azure Service Bus).
/Controllers            -> Controladores que lidam com as requisições HTTP e definem as rotas da API.
/Data                   -> Classes relacionadas ao acesso aos dados, como contextos de banco de dados para Entity Framework.
/Enums                  -> Tipos enumerados que são usados em todo o aplicativo para representar valores constantes.
/Models                 -> Classes de modelo que representam a estrutura dos dados manipulados pelo aplicativo.
/Properties             -> Arquivos de propriedades, como `launchSettings.json`, que configuram o ambiente de execução do projeto.
/Repositorie            -> Interfaces e classes para o padrão de repositório, que abstrai a lógica de acesso a dados.
   /Implementations     -> Implementações específicas das interfaces de repositório.
   /Interfaces          -> Interfaces para repositórios, especificando os métodos que devem ser implementados.
/Services               -> Lógica de negócios do aplicativo, organizada em serviços.
   /Implementations     -> Implementações específicas das interfaces de serviço.
   /Interfaces          -> Interfaces para serviços, especificando os métodos que devem ser implementados.
/Workers                -> Serviços em segundo plano que executam tarefas assíncronas ou contínuas, como o processamento de filas.

Program.cs              -> Ponto de entrada principal do aplicativo.
appsettings.json        -> Armazena configurações do aplicativo, como strings de conexão.
.gitignore              -> Arquivos e diretórios que devem ser ignorados pelo controle de versão Git.
```

### Configuração do Azure Service Bus

1. **Crie um Namespace do Service Bus**:
- Acesse o [Portal Azure](https://portal.azure.com/).
- Crie um novo namespace do Service Bus.

2. **Crie uma Fila**:
- Dentro do namespace do Service Bus, crie uma nova fila.

3. **Obtenha as Credenciais de Acesso**:
- Vá para "Políticas de acesso compartilhado" no namespace do Service Bus.
- Clique em uma das polísicas, como `RootManageSharedAccessKey`.
- Copie o `Cadeia de conexão primária`.

3. **Configure a Cadeia de Conexão no Projeto**:
- Atualize o arquivo `appsettings.json` com a string de conexão da Azure Service Bus e o nome da fila:
   
```json
{
   "AzureServiceBusOptions": {
      "ConnectionString": "<CADEIA_DE_CONEXAO>",
      "QueueName": "<NOME_DA_FILA>"
      }
}
```

- Substitua `<CADEIA_DE_CONEXAO>` pela Cadeia de conexão.
- Substitua `<NOME_DA_FILA>` pelo nome da sua Fila

```bash
# Exemplo de string
"Endpoint=sb://<SEU_NAMESPACE>.servicebus.windows.net/;SharedAccessKeyName=<NOME_DA_CHAVE_DE_ACESSP>;SharedAccessKey=<CHAVE_DE_ACESSO>",
```

## Configuração do Banco de Dados

1. **Inicie o Banco de Dados**:
- Clone uma imagem docker de PostgreSQL

```bash
docker pull postgres
```

- Inicie um container com a imagem baixada:

```bash
docker run --name=<CONTAINER_NAME> -e POSTGRES_USER=<DB_USER> -e POSTGRES_PASSWORD=<DB_PASSWORD> -e POSTGRES_DB=<DB_NAME> -p 5432:5432 -d postgres
```

- Substitua `CONTAINER_NAME` pelo nome que quiser dar ao container.
- Substitua `<DB_USER>` e `<DB_PASSWORD>` pelo nome de usuário e senha do banco de dados
- Substitua `<DB_NAME>` pelo nome que desejar dar ao banco de dados

```bash
# Exemplo
docker run --name=TMB-DB -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin -e POSTGRES_DB=TmbDb -p 5432:5432 -d postgres

# Postgree utiliza por padrão a porta 5432, Se desejar mudar, nao se esqueça de ajudar a Connection String
```

2. **Configure a String de Conexão**:
- Atualize o arquivo `appsettings.json` com a string de conexão do seu banco de dados:

```json
{
   "ConnectionStrings": {
   "DefaultConnection": "<CONNECTION_STRING>"
   },
}
```

- Substitua `<CONNECTION_STRING>` pela string de conexão do seu banco de dados.

```bash
# Exemplo de string
Host=localhost;Port=5432;Database=TmbDb;Username=admin;Password=admin

```

3. **Aplique as Migrações**:
- Certifique-se de que o Entity Framework Core CLI esteja instalado:

```bash
# Caso não esteja, instale com o seguinte comando:
dotnet tool install --global dotnet-ef
```

- No diretório do projeto, execute o seguinte comando para aplicar as migrações:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

- Isso criará as tabelas necessárias no banco de dados.

## Execute o projeto

1. **Restaure as dependências**:
```bash
dotnet restore
```

2. **Compile o projeto**:
```bash
dotnet build
```

3. **Execute o Projeto**:
```bash
dotnet run
```

4. **Abra o Projeto**:
- Por padrão, o projeto será executado na porta 5000
- http://localhost:5000
