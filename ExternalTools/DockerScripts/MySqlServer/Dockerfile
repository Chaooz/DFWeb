FROM mysql/mysql-server AS mysqldev
WORKDIR /app

# Copy database file
COPY ../../Data/darkfactorweb.sql ./

# Start mysql server
RUN --name=mysqldev -d mysqldev 

# Show auto generated password
LOGS mysqldev 2>&1 | grep GENERATED

EXEC -it mysqldev mysql -uroot -p --connect-expired-password -e "ALTER USER 'root'@'localhost' IDENTIFIED BY 'netr00t'";

# Build runtime image
#FROM microsoft/aspnetcore:2.0
#WORKDIR /app
#COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "DarkFactorCoreNet.dll"]
