#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["SyllabusManager.API/SyllabusManager.API.csproj", "SyllabusManager.API/"]
RUN dotnet restore "SyllabusManager.API/SyllabusManager.API.csproj"
COPY . .
WORKDIR "/src/SyllabusManager.API"
RUN dotnet build "SyllabusManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SyllabusManager.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SyllabusManager.API.dll"]