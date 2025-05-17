# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código fuente
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Configurar variables de entorno para la aplicación
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Exponer el puerto 80
EXPOSE 80

# Iniciar la aplicación
ENTRYPOINT ["dotnet", "StringManager_API.dll"]