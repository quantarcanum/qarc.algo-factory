#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Qarc.AlgoFactory.Adapter.Api/Qarc.AlgoFactory.Adapter.Api.csproj", "Qarc.AlgoFactory.Adapter.Api/"]
COPY ["Qarc.AlgoFactory.Adapter.Mongo/Qarc.AlgoFactory.Adapter.Mongo.csproj", "Qarc.AlgoFactory.Adapter.Mongo/"]
COPY ["Qarc.AlgoFactory.Core.Application/Qarc.AlgoFactory.Core.Application.csproj", "Qarc.AlgoFactory.Core.Application/"]
COPY ["Qarc.AlgoFactory.Core.Domain/Qarc.AlgoFactory.Core.Domain.csproj", "Qarc.AlgoFactory.Core.Domain/"]
RUN dotnet restore "Qarc.AlgoFactory.Adapter.Api/Qarc.AlgoFactory.Adapter.Api.csproj"
COPY . .
WORKDIR "/src/Qarc.AlgoFactory.Adapter.Api"
RUN dotnet build "Qarc.AlgoFactory.Adapter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Qarc.AlgoFactory.Adapter.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Qarc.AlgoFactory.Adapter.Api.dll"]