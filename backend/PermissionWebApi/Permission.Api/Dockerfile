# Usar una imagen base oficial de .NET para el SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el archivo .sln y los archivos .csproj
COPY backend/PermissionWebApi/*.sln ./
COPY backend/PermissionWebApi/Permission.Api/*.csproj backend/PermissionWebApi/Permission.Api/
COPY backend/PermissionWebApi/Permission.Application/*.csproj backend/PermissionWebApi/Permission.Application/

# Restaurar dependencias
RUN dotnet restore backend/PermissionWebApi/Permission.Api/Permission.Api.csproj

# Copiar el resto de los archivos y construir la aplicación
COPY backend/PermissionWebApi/ .
WORKDIR /app/Permission.Api
RUN dotnet publish -c Release -o out

# Usar una imagen base oficial de .NET para el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/Permission.Api/out .

# Exponer el puerto que tu API usa
EXPOSE 80
EXPOSE 443

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Permission.Api.dll"]
