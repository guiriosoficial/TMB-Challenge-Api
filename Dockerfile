FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /app

COPY TMB-Challenge.csproj ./ 

RUN dotnet restore

RUN dotnet tool install --global dotnet-ef --version 9.0

ENV PATH="${PATH}:/root/.dotnet/tools"

COPY . .

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "TMB-Challenge.dll"]