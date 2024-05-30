# .NET SDK kullanarak uygulamayı derliyoruz
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "LibraryApp.sln"
RUN dotnet build "LibraryApp.sln" -c Release -o /app/build
RUN dotnet publish "LibraryApp.sln" -c Release -o /app/publish

# Runtime imajı kullanarak uygulamayı çalıştırıyoruz
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Portu expose ediyoruz
EXPOSE 5062

# Çevre değişkenini ayarlıyoruz
ENV ASPNETCORE_URLS=http://+:5062

# Uygulamayı başlatıyoruz
ENTRYPOINT ["dotnet", "Katmanli.API.dll"]
