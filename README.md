# ProductCrud
This is REST API project created using Dotnet, MySql, Entityframework, Nunit, Moq for creating and managing products

## Prerequisite:
1) Dotnet(net8.0)
2) MySql

## Steps To Run:
1) Clone The Repository (git clone https://github.com/kris99manjunath/ProductCrud.git)
2) Create a local mysql db and update the connection string in appsettings.json present in ".\ProductsCrudApp\ProductsCrudApp\appsettings.json" [Sample string already provided].
3) Restore package(dotnet restore), build(dotnet build) from command prompt or do the same via visual studio.
4) In Visual Studio, open package manager cmd and run "update-database" to create the tables.[install dotnet ef tool if not installed : dotnet tool install --global dotnet-ef]
5) Above command should have created an empty product table in mysql[verify].
6) Run the code.
7) If launched via Visual studio it should automatically open the swagger page.(https://localhost:7255/swagger/index.html).

## Overview of implementation

<ul> 
  <li> It exposes 7 end points[create,put ,getbyId ,getall, delete, addStocks, decreaseStocks]. </li>
  <li> There is middleware to handle all the exceptions globally and return 500 Bad response in any such case. It returns the actual error if dev env else logs the error. </li>
  <li> Uses Multiple DataAnnotations Validators and Custom made validators to  validate data[InventoryOverflowValidator,InventoryUnderflowValidator, KeyNotFoundValidatior, PositiveNumberAttributeValidator]. </li>
  <li> Validation is done for the following: Name[required], StockAvailable and should be >= 0, price should be greater than 0.01, description is optional </li>
  <li> DTO class is used to processing the incoming product requests and also sending the error response in a particular format. </li>
  <li> KeyNotFoundValidatio,InventoryOverflowValidatorr and InventoryUnderflowValidator are used so that no check is needed during performing the operation and can be made generic and extended if needed for other Controllers. Since KeyNotFoundValidatior has to call getProductById() the data is stored to avoid redundant calls. </li>
  <li> PositiveNumberAttributeValidator is done so that when incrementing or decrementing the value is intact and can be reused on all types. </li>
  <li> Uses Extension Menthods to add functionality for existing classes which helps in writing cleaner code also to avoid repetition of code. </li>
  <li> Uses a ErrorResponse Object to send errors/validation issue in a particular format. </li>
  <li> Uses Dependency Injection and Repository pattern is used. </li>
  <li> Uses SeriLog to log so that can also be configured for database logging in future. </li>
  <li> Has a test project which has unit tests for the controller as well as the SqlRepository. </li>
  <li> To generate 6-digit collision free Id, it is generated in mysql using id auto increment and the start value is set to 100000 so it is 6 digits. </li>
</ul>
