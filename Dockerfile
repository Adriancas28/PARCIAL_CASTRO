# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia el archivo de proyecto y restaura dependencias
COPY *.csproj ./
RUN dotnet restore

# Copia el resto del código y construye
COPY . ./
RUN dotnet publish -c Release -o out

# Etapa de producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia los archivos construidos desde la etapa anterior
COPY --from=build-env /app/out ./

# Cambia el nombre de tu aplicación aquí
ENV APP_NET_CORE PARCIAL_CASTRO.dll 

CMD ["sh", "-c", "ASPNETCORE_URLS=http://*:$PORT dotnet $APP_NET_CORE"]