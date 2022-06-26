FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 7000
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JobsityFinancialChat/JobsityFinancialChat.csproj", "JobsityFinancialChat/"]
COPY ["JobsityCommons/JobsityCommons.csproj", "JobsityCommons/"]
RUN dotnet restore "JobsityFinancialChat/JobsityFinancialChat.csproj"
RUN dotnet restore "JobsityCommons/JobsityCommons.csproj"
COPY . .
WORKDIR "/src/JobsityFinancialChat"
RUN dotnet build "JobsityFinancialChat.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobsityFinancialChat.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobsityFinancialChat.dll"]