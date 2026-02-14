FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AspireApp.ApiService/AspireApp.ApiService.csproj", "AspireApp.ApiService/"]
COPY ["AspireApp.ServiceDefaults/AspireApp.ServiceDefaults.csproj", "AspireApp.ServiceDefaults/"]
RUN dotnet restore "AspireApp.ApiService/AspireApp.ApiService.csproj"
COPY . .
WORKDIR "/src/AspireApp.ApiService"
RUN dotnet build "AspireApp.ApiService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspireApp.ApiService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspireApp.ApiService.dll"] 