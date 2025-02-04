# Use the .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . .
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /out

# Use a lightweight .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /out .

#set environment variable for dabatabse credentials
ENV DB_HOST=mysql
ENV DB_PORT=3306
ENV DB_NAME=fleetdb
ENV DB_USER=fleetuser
ENV DB_PASSOWRD=fleetPassword 

# Expose the correct port #Make sure it matches your application port
EXPOSE 5222  

# Run the application
ENTRYPOINT ["dotnet", "fleetmanagement.dll"]
