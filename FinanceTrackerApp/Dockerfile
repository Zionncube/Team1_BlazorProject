FROM mcr.microsoft.com/dotnet/aspnet:10.0 as base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
COPY . /src
RUN ls
RUN dotnet build "FinanceTrackerApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinanceTrackerApp.csproj" -c Release -n /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinanceTrackerApp.dll"]