#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#Set timezone to Moscow
ENV TZ=Europe/Moscow 
RUN cp /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
ARG nuget_path=https://artifactory.rgs.ru/artifactory/api/nuget/nuget-hosted/
COPY ["PrintEngine/PrintEngine.csproj", "PrintEngine/"]
COPY ["PrintEngine.Templates/PrintEngine.Templates.csproj", "PrintEngine.Templates/"]
COPY ["PrintEngine.Core/PrintEngine.Core.csproj", "PrintEngine.Core/"]
COPY ["PrintEngine.Resources/PrintEngine.Resources.csproj", "PrintEngine.Resources/"]
COPY ["PrintEngine.Metadata/PrintEngine.Metadata.csproj", "PrintEngine.Metadata/"]
RUN dotnet restore "PrintEngine/PrintEngine.csproj"
COPY . .
WORKDIR "/src/PrintEngine"
RUN dotnet build "PrintEngine.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "PrintEngine.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrintEngine.dll"]