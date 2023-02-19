# MSSQL_SA_PASSWORD is the database system administrator (userid = 'sa') password used to connect to SQL Server once the container is running.
# Important note: This password needs to include at least 8 characters of at least three of these four categories: uppercase letters,
# lowercase letters, numbers and non-alphanumeric symbols.

docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=someStr0ng#Password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-CU1-ubuntu-20.04