#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["recipeList.csproj", "."]
RUN dotnet restore "./recipeList.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "recipeList.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "recipeList.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "recipeList.dll"]

# Add the migration steps here
WORKDIR /app
COPY ["recipeList.csproj", "."]
RUN dotnet ef migrations add InitialMigration --output-dir Migrations
RUN dotnet ef migrations script --output init-script.sql --idempotent
RUN dotnet ef database update
