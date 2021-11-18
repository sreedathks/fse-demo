FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 6780
EXPOSE 44367

RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_8.x | bash - && \
    apt-get install -y build-essential nodejs

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY src/QuickApp/QuickApp.csproj src/QuickApp/
COPY src/DAL/DAL.csproj src/DAL/
RUN dotnet restore src/QuickApp/QuickApp.csproj
COPY . .
WORKDIR /src/src/QuickApp
RUN dotnet build QuickApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish QuickApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "QuickApp.dll"]