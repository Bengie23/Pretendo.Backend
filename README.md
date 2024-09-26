# What is Pretendo?
Pretendo is a local mock server intended to help developers accelerate their testing, improve code simplicty and, in the future, facilitate testing during CI/CD stages.[More Info](https://github.com/Bengie23/Pretendo_Frontend)
# What is Pretendo.Backend?
Since Pretendo is a **local** mock server, all custom endpoints will be caught and satisfied at a local level, hence Pretendo.Backend is required to be running only locally.
# Features

![image](https://github.com/user-attachments/assets/8105c1b5-74ec-4359-ac8d-a03a61f8000a)
> ## Endpoints
>> ### Create Pretendo
>> Creates a new Pretendo, if the domain is new, it creates it as well.
>> ### Get All Pretendos
>> Returns a list of pretendos for a given domain.
>> ### Get All Domains
>> Returns a list of all domains.



> ## Powershell scripts
>> ### Register custom domain in the local hosts file
# Setup
## With Visual Studio
The solution includes an app manifest that states that the solution requires privileged access, attempting to run the servier using visual studio will prompt for elevated rights.
## With dotnet run command
In the same, way, the app manifest that states that the solution requires privileged access, attempting to execute `dotnet run` will raise an error, since elevated rights is required. Run a terminal with elevated rights first, then run `dotnet build` and `dotnet run`.
## Default Port
Pretendo.Backend is supposed to always be running behind the default port:8080, in VS, this is already configured, if using  `dotnet run` runs the server in a different port, use `SET ASPNETCORE_URLS=https://*:8080 && dotnet run`.
