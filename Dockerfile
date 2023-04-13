#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -sL https://deb.nodesource.com/setup_lts.x | bash -
RUN apt-get install -y nodejs	
WORKDIR /src
COPY ["YCNBot/YCNBot.csproj", "YCNBot/"]
COPY ["YCNBot.Core/YCNBot.Core.csproj", "YCNBot.Core/"]
COPY ["YCNBot.Data/YCNBot.Data.csproj", "YCNBot.Data/"]
COPY ["YCNBot.Services/YCNBot.Services.csproj", "YCNBot.Services/"]
RUN dotnet restore "YCNBot/YCNBot.csproj"
COPY . .
WORKDIR "/src/YCNBot"
RUN dotnet build "YCNBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YCNBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YCNBot.dll"]
