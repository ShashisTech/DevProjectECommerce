FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG DOTNET_USE_POLLING_FILE_WATCH
ENV DOTNET_USE_POLLING_FILE_WATCH=${DOTNET_USE_POLLING_FILE_WATCH}
COPY ["src/OrderService/OrderService.csproj", "src/OrderService/"]
RUN dotnet restore "src/OrderService/OrderService.csproj"
COPY . .
WORKDIR "/src/src/OrderService"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN useradd -m appuser && chown -R appuser /app
USER appuser
ENTRYPOINT ["dotnet", "OrderService.dll"]
