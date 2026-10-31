Enhanzer Full Stack Developer Trainee Assessment
A two-page web application developed as part of the Enhanzer Full Stack
Developer Trainee technical assessment.

The application uses Angular, TypeScript, ASP.NET Core Web API
(.NET 8), C#, and SQL Server.
Important: Update this README to match your actual implementation
before submitting. Replace every [PLACEHOLDER], remove features you
did not implement, and do not include real passwords, API secrets, or
personal credentials in this file or in GitHub.

1. Assignment overview
The assessment requires the following:
1. A login page that authenticates users through the supplied Enhanzer
   API.
2. Saving the authenticated user's locations in a SQL Server table
   named Location_Details.
3. A protected Purchase Bill page.
4. An item autocomplete containing Mango, Apple, Banana, Orange,
   Grapes, Kiwi, and Strawberry.
5. A Batch dropdown populated using saved location names.
6. Purchase bill calculations, an item table, and an item summary.
2. Technologies
  Area              Technology
  Frontend          Angular, TypeScript, HTML, CSS
Backend           ASP.NET Core Web API, C#
Framework         .NET 8
Database          Microsoft SQL Server
Version control   Git and GitHub
3. Features
Login page
- Email and password inputs.
- Client-side input validation.
- Authentication request sent to the Enhanzer API through the backend.
- Success and failure feedback.
- Authenticated session handling.
- Retrieval of the User_Locations collection after successful
  authentication.
- Saving location codes and names to SQL Server.
Authentication endpoint supplied in the assignment:

https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke

The request payload and response handling should follow the assignment
PDF. Do not commit credentials or tokens to source control.
Purchase Bill page
- Accessible only to an authenticated user.
- Item autocomplete with the seven specified fruit items.
- Batch dropdown populated from Location_Details.
- Quantity, Standard Cost, Standard Price, and Discount inputs.
- Automatic Total Cost and Total Selling calculations.
- Add action to insert an item into the displayed table.
- Summary showing total number of rows/items and total quantity.
Calculation rules
Based on the example in the assignment:
- Total Cost = (Standard Cost × Quantity) − (Standard Cost × Quantity
  × Discount Percentage / 100)
- Total Selling = Standard Price × Quantity
- Total Items = number of rows in the item table
- Total Quantity = sum of the quantities in the item table
Example:

  Input              Value
  Standard Cost        100
Standard Price       150
Quantity               5
Discount             20%
Total Cost           400
Total Selling        750

The assignment does not specify a source for real standard cost and
standard price values. Describe the source or sample values used by this
implementation here: \(EXPLAIN YOUR PRICE/COST DATA SOURCE\).
4. Project structure
Update this example to match the folders and filenames in your
repository.
enhanzer-assignment/
├── frontend/                 # Angular application
│   ├── src/
│   │   └── app/
│   │       ├── core/         # Services, guards, shared core logic
│   │       ├── login/        # Login page
│   │       ├── purchase-bill/# Purchase Bill page
│   │       └── ...
│   └── package.json
├── backend/                  # ASP.NET Core Web API
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   └── ...
├── database/
│   └── Location_Details.sql  # SQL database/table script
└── README.md
5. Prerequisites
Install the following software:
- Node.js (LTS) and npm.
- Angular CLI compatible with the Angular version used by this
  project.
- .NET 8 SDK.
- SQL Server (Express, Developer, or another compatible SQL Server
  edition).
- SQL Server Management Studio (SSMS), or another SQL Server client.
- Git.
Check the installations:
node --version
npm --version
ng version
dotnet --list-sdks
git --version
Confirm that a SQL Server instance is installed and running before
starting the backend.
6. Database setup
1. Start your SQL Server instance.
2. Open SQL Server Management Studio and connect to your local
   instance.
3. Open and execute the SQL script in database/ (update this path if
   your script is elsewhere).
4. Confirm that the Location_Details table has been created.
The required table stores the location code and location name returned
by the login API. The exact column types and constraints should match
the SQL script included in this repository.
Database connection
Configure the backend connection string using your local SQL Server
instance.

For example, use a configuration entry similar to the following in your
local development settings:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
Replace the server and database names with your actual values. This is
an example only; use the connection-string format appropriate for your
SQL Server authentication setup.

Do not commit production credentials or sensitive connection strings.
Use user secrets or environment variables for sensitive settings.
7. Configuration
Configure the Enhanzer API endpoint and any other environment-specific
settings in the backend configuration.
- API endpoint:
  https://ez-staging-api.azurewebsites.net/api/External_Api/POS_Api/Invoke
- SQL Server connection string: [CONFIGURE LOCALLY]
- Frontend API base URL: [CONFIGURE LOCALLY]
Do not place API passwords, user credentials, access tokens, or private
keys in the Angular source code or public repository.
8. Run the application
The commands below assume the example frontend/ and backend/ folder
names. Change them if your repository uses different names.
Start the backend
cd backend
dotnet restore
dotnet build
dotnet run
Note the local HTTP/HTTPS URL printed by the application. Configure the
Angular frontend to use the correct backend API URL.
Start the frontend
Open another terminal:
cd frontend
npm install
ng serve
Open the local URL shown by Angular in the terminal (commonly
http://localhost:4200).
9. How to use
1. Start SQL Server.
2. Run the ASP.NET Core backend.
3. Run the Angular frontend.
4. Open the frontend in a browser.
5. Enter valid login credentials provided for assessment/testing.
6. After successful authentication, confirm that the returned locations
   are saved in Location_Details.
7. Open the Purchase Bill page.
8. Select an item and batch/location, enter the required values, and
   check the calculated totals.
9. Click Add and verify that the item appears in the table.
10. Confirm that Total Items and Total Quantity update correctly.
11. Test that an unauthenticated user cannot access the Purchase Bill
    page.
10. Validation and error handling
Test and document the behavior implemented for:
- Empty email and password fields.
- Invalid login credentials.
- Authentication API errors or network failure.
- Missing or invalid form values.
- Invalid quantity, cost, price, or discount values.
- Attempts to access the Purchase Bill page without authentication.
- Database connection or save failures.
11. Testing checklist
- [ ] Frontend starts successfully.
- [ ] Backend starts successfully.
- [ ] Database connection works.
- [ ] Login form validates required fields.
- [ ] Successful login is handled correctly.
- [ ] Failed login displays a useful error.
- [ ] User_Locations are saved to Location_Details.
- [ ] Batch dropdown displays saved location names.
- [ ] Item autocomplete contains all seven required items.
- [ ] Total Cost calculation is correct.
- [ ] Total Selling calculation is correct.
- [ ] Add inserts an item into the table.
- [ ] Total Items and Total Quantity update correctly.
- [ ] Purchase Bill page is protected from unauthorized access.
