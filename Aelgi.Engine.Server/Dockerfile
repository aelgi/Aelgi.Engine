FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 7707

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Aelgi.Engine.Server/Aelgi.Engine.Server.fsproj", "Aelgi.Engine.Server/"]
COPY ["Aelgi.Engine.Core/Aelgi.Engine.Core.fsproj", "Aelgi.Engine.Core/"]
RUN dotnet restore "Aelgi.Engine.Server/Aelgi.Engine.Server.fsproj"
COPY . .
WORKDIR "/src/Aelgi.Engine.Server"
RUN dotnet build "Aelgi.Engine.Server.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aelgi.Engine.Server.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Aelgi.Engine.Server/appsettings.json .
COPY Aelgi.Engine.Server/appsettings.Development.json .
COPY Aelgi.Engine.Server/appsettings.Production.json .
ENV DOTNET_ENVIRONMENT Production

ENTRYPOINT ["dotnet", "Aelgi.Engine.Server.dll"]