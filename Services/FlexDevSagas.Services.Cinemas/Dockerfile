#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/FlexDevSagas.Services.Cinemas/FlexDevSagas.Services.Cinemas.csproj", "Services/FlexDevSagas.Services.Cinemas/"]
RUN dotnet restore "Services/FlexDevSagas.Services.Cinemas/FlexDevSagas.Services.Cinemas.csproj"
COPY . .
WORKDIR "/src/Services/FlexDevSagas.Services.Cinemas"
RUN dotnet build "FlexDevSagas.Services.Cinemas.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlexDevSagas.Services.Cinemas.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexDevSagas.Services.Cinemas.dll"]