FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["UrlShortener.Api/UrlShortener.Api.csproj", "UrlShortener.Api/"]
COPY ["UrlShortenerBackend/UrlShortenerBackend.csproj", "UrlShortenerBackend/"]

RUN dotnet restore "UrlShortener.Api/UrlShortener.Api.csproj"

COPY . .

WORKDIR "/src/UrlShortener.Api"
RUN dotnet publish "UrlShortener.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "UrlShortener.Api.dll"]