# Стадия сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY ShutafimService/ShutafimService.csproj ./ShutafimService/
WORKDIR /app/ShutafimService
RUN dotnet restore

# Копируем остальные файлы и публикуем
COPY . /app/
RUN dotnet publish -c Release -o /app/publish

# Runtime образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

# Открываем порты
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "ShutafimService.dll"]
