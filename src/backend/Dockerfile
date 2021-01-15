FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
COPY . .
RUN dotnet restore "SyllabusManager.API/SyllabusManager.API.csproj"
RUN dotnet build "SyllabusManager.API/SyllabusManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SyllabusManager.API/SyllabusManager.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "SyllabusManager.API.dll"]
