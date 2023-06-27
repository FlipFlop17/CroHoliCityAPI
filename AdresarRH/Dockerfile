# Set the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

# Copy the published app from build image
COPY --from=build /app/out ./

# Expose the required port
EXPOSE $PORT

# Set the entry point
ENTRYPOINT ["dotnet", "CroHoliCityAPI.dll"]
