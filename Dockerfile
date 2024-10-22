# Etapa base para la ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restaurar dependencias con verbosidad detallada

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["CDR-Worship.csproj", "./"]
RUN dotnet restore "./CDR-Worship.csproj"


# Verificar que las dependencias se restauren correctamente
RUN dotnet list package

# Copiar el resto del código fuente y construir el proyecto
COPY . .
RUN dotnet build "CDR-Worship.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "CDR-Worship.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final para la ejecución del contenedor
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CDR-Worship.dll"]