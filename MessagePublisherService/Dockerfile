FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["DiscordMessageService/DiscordMessageService.csproj", "MessageService/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
RUN dotnet restore "MessageService/MessagePublisherService.csproj"

COPY . .
WORKDIR "/src/DiscordMessageService"
RUN dotnet build "DiscordMessageService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessagePublisherService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Koala.MessagePublisherService.dll"]
