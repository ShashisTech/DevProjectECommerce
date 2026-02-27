FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG DOTNET_USE_POLLING_FILE_WATCH
ENV DOTNET_USE_POLLING_FILE_WATCH=${DOTNET_USE_POLLING_FILE_WATCH}
COPY ["src/CatalogService/CatalogService.csproj", "src/CatalogService/"]
RUN dotnet restore "src/CatalogService/CatalogService.csproj"
COPY . .
WORKDIR "/src/src/CatalogService"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Use a non-root user
RUN useradd -m appuser && chown -R appuser /app
USER appuser
ENTRYPOINT ["dotnet", "CatalogService.dll"]
