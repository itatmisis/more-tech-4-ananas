#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["More_Tech.Parser/More_Tech.Parser.csproj", "More_Tech.Parser/"]
COPY ["More_Tech.Data/More_Tech.Data.csproj", "More_Tech.Data/"]
RUN dotnet restore "More_Tech.Parser/More_Tech.Parser.csproj"
COPY . .
WORKDIR "/src/More_Tech.Parser"
RUN dotnet build "More_Tech.Parser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "More_Tech.Parser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "More_Tech.Parser.dll"]