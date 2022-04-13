#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/FlexDevSagas.Services.Movies/FlexDevSagas.Services.Movies.csproj", "Services/FlexDevSagas.Services.Movies/"]
RUN dotnet restore "Services/FlexDevSagas.Services.Movies/FlexDevSagas.Services.Movies.csproj"
COPY . .
WORKDIR "/src/Services/FlexDevSagas.Services.Movies"
RUN dotnet build "FlexDevSagas.Services.Movies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlexDevSagas.Services.Movies.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexDevSagas.Services.Movies.dll"]
