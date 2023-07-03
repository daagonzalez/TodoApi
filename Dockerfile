# Start with the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code and build the project
COPY . ./
RUN dotnet publish -c Release -o out

# Create the final image with the built application
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port on which the API will listen
EXPOSE 7033

# Define the entry point for the container
ENTRYPOINT ["dotnet", "TodoApi.dll"]
