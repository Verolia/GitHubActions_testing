# Bygg steg
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime steg
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Fikser feilmeding om manglende Kerberos (postgres, .Net forventer uansett) + curl
RUN apt-get update && apt-get install -y \
    libkrb5-3 \
    libgssapi-krb5-2 \
    curl \
 && rm -rf /var/lib/apt/lists/*


COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "api.dll"]