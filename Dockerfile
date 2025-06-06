# Use a imagem oficial do .NET SDK para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copie os arquivos de projeto e restaure as dependências
COPY *.csproj ./
RUN dotnet restore

# Copie o restante do código e compile a aplicação
COPY . ./
RUN dotnet publish -c Release -o out

# Use a imagem do .NET Runtime para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .

# Defina variáveis de ambiente para o appsettings
ENV ConnectionStrings__DefaultConnection="Host=db;Port=5432;Database=TmbDb;Username=admin;Password=admin"
ENV AzureServiceBusOptions__ConnectionString="<CONNECTION_SCRING>"
ENV AzureServiceBusOptions__QueueName="<QUEUE_NAME>"

# Exponha a porta que a aplicação irá utilizar
EXPOSE 5000

# Comando de entrada para aplicar migrações e iniciar a aplicação
ENTRYPOINT ["sh", "-c", "dotnet ef migrations add InitialCreate && dotnet ef database update && dotnet ef database update && dotnet TMB-Challenge-Api.dll"]