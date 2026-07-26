FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Formulaar1.sln ./
COPY Formulaar1/Formulaar1.csproj Formulaar1/
COPY Formulaar1.Tests/Formulaar1.Tests.csproj Formulaar1.Tests/
RUN dotnet restore

COPY . .
RUN dotnet publish Formulaar1/Formulaar1.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Formulaar1.dll"]
