FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY . .
RUN dotnet restore "./TodoApi.sln" --disable-parallel
RUN dotnet publish "./TodoApi.sln" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=build-env /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "TodoApi.Presentation.dll"]
