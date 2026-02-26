
# get the .NET run time image to run the app 

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# installing curl for healht checks using root user
USER root
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

# create a new user so that we don't use root user 
USER $APP_UID

# create a folder app where we are going to copy our app
WORKDIR /app

# expose ports so that we can communitcate
EXPOSE 8080
EXPOSE 8081


# get the .net sdk to build the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# declaring variable 
ARG BUILD_CONFIGURATION=Release

# creating folder with name src and moving to src folder
WORKDIR /src

# copy the .csproj file from host machine to docker image
COPY ["OrderService.csproj", "."]

# resotring all dependencies 
RUN dotnet restore "./OrderService.csproj"

# copying all other files to src folder
COPY . .

# moving to src folder
WORKDIR "/src/."

# building the app and copyingt the output to /app/build folder
RUN dotnet build "./OrderService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# taking the pervious image
FROM build AS publish

# declaring variable
ARG BUILD_CONFIGURATION=Release

# publish the app using dotnet publish , output will go to /app/publish folder
RUN dotnet publish "./OrderService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# running the published app
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.dll"]