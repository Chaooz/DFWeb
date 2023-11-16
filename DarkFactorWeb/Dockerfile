# Use .Net Core 3.1 image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./DarkFactorWeb/*.csproj ./
RUN dotnet restore

# Copy everything else
COPY ./DarkFactorWeb/ ./

# Build solution
RUN dotnet publish DarkFactorCoreNet.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DarkFactorCoreNet.dll"]

EXPOSE 5000

#docker build -t dotnetcoredarkfactorweb .
#docker run -d -p 8080:80 --name dotnetcoredarkfactorweb dotnetcoredarkfactorweb
