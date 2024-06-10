Dies ist das SQS-Projekt von Simon Göttsberger für die Vorlesung

Software Qualitätssicherung


Benötigte Tests:

- Integration Test
- Unit Test
- E2E Test
- Lasttest 
- Akzeptanztest
- Dependency Security Checks (Github Action)


ConnectionString für MS LocalDB:
    "WebAppContext": "Server=(localdb)\\mssqllocaldb;Database=WebAppContext-aa547cdf-dffb-4433-af90-ecd6f6d5ac75;Trusted_Connection=True;MultipleActiveResultSets=true"

Run Docker Postgres Image
    docker run --name postgresDB -e POSTGRES_PASSWORD=mypassword -p 5432:5432 -d postgres

Create Migration
    dotnet ef migrations add MigrationName

Update Database
    dotnet ef Update-Database