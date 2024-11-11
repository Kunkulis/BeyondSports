# BeyondSports

## Set Up the Database

### 1. Navigate to the Project Directory
Open a terminal in the directory containing `BeyondSports.csproj`. The path should be `..\BeyondSports\BeyondSports`. To open the terminal:
   - Use **File Explorer**: Navigate to the folder, type `cmd` in the address bar.
   - Or use **Visual Studio**: Open the project, then open the terminal with `Ctrl + ``. 

If you initially only see one `BeyondSports` folder, navigate one level deeper: `cd .\BeyondSports\`.

### 2. Add Migrations
Run the following command to create the initial migration files:
`dotnet ef migrations add InitialCreate`

### 3. Run the API
After setting up migrations, you can start the API:

- **In Visual Studio**: Press F5 to run.
- **In the terminal**: Run dotnet run to start the API. The terminal will display the URL the API is listening to.

### 4. Access the API Interface
Append `/swagger` to the API’s URL to use the Swagger UI for testing the API.
