# GitHubActions_testing

Every time someone makes a new pull request to main (wants to pull/merge their branch into main), Github Actions workflow will run.
If any of these fail, the pull request will not go through and you can not push the code to main. 
You get feedback on what the problem is, where it failed.  

A virual machine is built, and this machine runs the different commands on our code: 

##  Backend checks: 
- Checkout repository: actions/checkout@v2
- Set up .NET: actions/setup-dotnet@v1
    - dotnet-version: 10.0.x
- Restore dependencies: 'dotnet restore'
- Build project: dotnet build --no-restore --configuration Release
- Run our tests: dotnet test --no-build --configuration Release 

## Frontend checks: 
- Checkout repository: actions/checkout@v2
- Set up Node.js: actions/setup-node@v2
    - node-version: 18.x
- Install frontend dependencies: npm install
- Build frontend: npm run build

## Integration checks: 
To ensure that different parts of the project work together, not just individually. For our workflow, this includes the backend connecting to the database and running integration tests against it.

- Start temporary Postgres database
    - A PostgreSQL container is launched using GitHub Actions services.
    - The database is configured with a username, password, and database name.
    - Ports are exposed so the backend code can connect.
- Set up backend environment
    - Checkout repository and set up .NET (same as backend checks).
    - Restore dependencies and build the backend project.
- Run integration tests
    - Environment variable ConnectionStrings__MainDbConnection points to the temporary database.
    - Tests verify that MainDbContext can connect, migrations are correct, and database queries work.
    - After tests finish, the database container is removed automatically.

Note: this is not exactly the same as running Docker-Compose Up --build, because the GitHub Actions workflow doesn’t start all services at once like the local docker-compose setup will. Each job or service runs in isolation unless explicitly linked.
The database created for integration tests is temporary and only exists for the duration of the workflow. It will be removed automatically after tests finish. The workflow focuses on build, tests, and basic integration, not on fully running the app stack. 