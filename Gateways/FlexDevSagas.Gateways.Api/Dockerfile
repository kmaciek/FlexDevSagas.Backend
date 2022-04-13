#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gateways/FlexDevSagas.Gateways.Api/FlexDevSagas.Gateways.Api.csproj", "Gateways/FlexDevSagas.Gateways.Api/"]
RUN dotnet restore "Gateways/FlexDevSagas.Gateways.Api/FlexDevSagas.Gateways.Api.csproj"
COPY . .
WORKDIR "/src/Gateways/FlexDevSagas.Gateways.Api"
RUN dotnet build "FlexDevSagas.Gateways.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlexDevSagas.Gateways.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexDevSagas.Gateways.Api.dll"]
