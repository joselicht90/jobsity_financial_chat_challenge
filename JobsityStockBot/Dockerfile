FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JobsityCommons/JobsityCommons.csproj", "JobsityCommons/"]
COPY ["JobsityStockBot/JobsityStockBot.csproj", "JobsityStockBot/"]
RUN dotnet restore "JobsityStockBot/JobsityStockBot.csproj"
RUN dotnet restore "JobsityCommons/JobsityCommons.csproj"
COPY . .
WORKDIR "/src/JobsityStockBot"
RUN dotnet build "JobsityStockBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobsityStockBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobsityStockBot.dll"]