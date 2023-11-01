#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ShopNet.API/ShopNet.API.csproj", "ShopNet.API/"]
COPY ["ShopNet.BLL/ShopNet.BLL.csproj", "ShopNet.BLL/"]
COPY ["ShopNet.Common/ShopNet.Common.csproj", "ShopNet.Common/"]
COPY ["ShopNet.DAL/ShopNet.DAL.csproj", "ShopNet.DAL/"]
RUN dotnet restore "ShopNet.API/ShopNet.API.csproj"
COPY . .
WORKDIR "/src/ShopNet.API"
RUN dotnet build "ShopNet.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShopNet.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShopNet.API.dll"]