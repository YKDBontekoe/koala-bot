FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MessageConsumerService/MessageConsumerService.csproj", "MessageConsumerService/"]
COPY ["Infrastructure.Messaging/Infrastructure.Messaging.csproj", "Infrastructure.Messaging/"]
COPY ["Infrastructure.Common/Infrastructure.Common.csproj", "Infrastructure.Common/"]
RUN dotnet restore "MessageConsumerService/MessageConsumerService.csproj"
COPY . .
WORKDIR "/src/MessageConsumerService"
RUN dotnet build "MessageConsumerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageConsumerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Koala.MessageConsumerService.dll"]
