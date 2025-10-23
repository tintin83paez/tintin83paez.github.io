@echo off
REM - In the following the commands to create a solution along with the projects of AppForMovies are presented.


REM - First, CLONE YOUR REPO, preferably in c:/repos
REM - Second, copy this file to the folder where your repo has been cloned 
REM - Third, replace TeamProject in the next line with the name of the product you are going to develop WITHOUT WHITESPACES

SET PROJECT_NAME=AppForDevices
if PROJECT_NAME=="" SET PROJECT_NAME=AppForDevices

REM - Fourth, run commands.bat

SET NETCORE_VERSION=net8.0
SET WEB_CODE_GENERATION_LIB_VERSION=8.0.5
SET NETCORE_LIB_VERSION=8.0.8
SET SWASHBUCKLE_VERSION=6.6.2
SET DATA_ANNOTATIONS_LIB_VERSION=3.2.0-rc1.20223.4
SET XUNIT_VERSION=2.9.0
SET XUNIT_RUNNER_VERSION=2.8.2
SET MOQ_VERSION=4.20.72
SET SELENIUM_VERSION=4.24.0
SET CHROME_DRIVER_VERSION=128.0.6613.11900
SET COVERLET_VERSION=6.0.2




@echo *********************************************************
@echo.                                                               
@echo       PROJECT: %PROJECT_NAME%   
@echo. 
@echo *********************************************************

REM - Create your solution
dotnet new sln --name "%PROJECT_NAME%"

REM - Create the folder for the source code and create the projects
md src
cd src

REM ----------------------------SRC------------------------------------------------

REM -------- create a library to share classes definition between both projects --------
dotnet new  classlib -f %NETCORE_VERSION% -n "%PROJECT_NAME%".Shared

REM -----------------------  add the package for annotations to validate the properties in runtime
cd "%PROJECT_NAME%".Shared
dotnet add package Microsoft.AspNetCore.Components.DataAnnotations.Validation --version %DATA_ANNOTATIONS_LIB_VERSION%
cd ..

REM -------- create the API project without authorization using framework 8.0 --------
dotnet new  webapi -au none -f %NETCORE_VERSION% -n "%PROJECT_NAME%".API

REM -----------------------  add reference to the AppForMovies.Shared project
cd "%PROJECT_NAME%".API
dotnet add reference ../"%PROJECT_NAME%".Shared/"%PROJECT_NAME%".Shared.csproj

REM -----------------------  add packages to the project
dotnet add package Microsoft.EntityFrameworkCore.Design --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Tools --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version %WEB_CODE_GENERATION_LIB_VERSION%


REM -----------------------  add a file for the design of the Class Diagram
echo ^<?xml version="1.0" encoding="utf-8"?^>^<ClassDiagram^>^</ClassDiagram^> >  ClassDiagram.cd

cd ..

REM -------- create the Web Project with authorization using framework 8.0 ------------
dotnet new  blazor -au Individual -f %NETCORE_VERSION% -n "%PROJECT_NAME%".Web

REM -----------------------  add referebce to the AppForMovies.Shared project
cd "%PROJECT_NAME%".Web
dotnet add reference ../"%PROJECT_NAME%".Shared/"%PROJECT_NAME%".Shared.csproj
cd ..

REM ----------------------- add packages to the project 
cd "%PROJECT_NAME%".Web
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design --version %WEB_CODE_GENERATION_LIB_VERSION%
cd ..

REM - add the created projects to the solution
cd..
dotnet sln "%PROJECT_NAME%".sln add src/"%PROJECT_NAME%".API
dotnet sln "%PROJECT_NAME%".sln add src/"%PROJECT_NAME%".Web
dotnet sln "%PROJECT_NAME%".sln add src/"%PROJECT_NAME%".Shared

REM -------------------------------TEST-----------------------------------------------------------------------
REM - Create the folder for the testing code and create the projects
md test
cd test

REM -------- create the project for the unit test
dotnet new xunit -f %NETCORE_VERSION% -n "%PROJECT_NAME%".UT

REM ----------------------- add the reference to the project to be tested
cd "%PROJECT_NAME%".UT
dotnet add reference ../../src/"%PROJECT_NAME%".API/"%PROJECT_NAME%".API.csproj
dotnet add reference ../../src/"%PROJECT_NAME%".Shared/"%PROJECT_NAME%".Shared.csproj

REM -----------------------  add packages
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version %NETCORE_LIB_VERSION%
dotnet add package Microsoft.EntityFrameworkCore.Tools --version %NETCORE_LIB_VERSION%
dotnet add package xunit --version %XUNIT_VERSION%
dotnet add package coverlet.collector --version %COVERLET_VERSION%
dotnet add package Moq --version %MOQ_VERSION%
cd ..

REM -------- create the project for the functional test
dotnet new xunit -f %NETCORE_VERSION% -n "%PROJECT_NAME%".UIT

REM -----------------------  add packages
cd "%PROJECT_NAME%".UIT
dotnet add package Selenium.Support --version %SELENIUM_VERSION%
dotnet add package Selenium.WebDriver --version %SELENIUM_VERSION%
dotnet add package Selenium.WebDriver.ChromeDriver --version %CHROME_DRIVER_VERSION%
dotnet add package xunit --version %XUNIT_VERSION%
dotnet add package xunit.runner.visualstudio --version %XUNIT_VERSION%
dotnet add package coverlet.collector --version %COVERLET_VERSION%
cd..

REM - add the created projects to the solution
cd..
dotnet sln "%PROJECT_NAME%".sln add test/"%PROJECT_NAME%".UT
dotnet sln "%PROJECT_NAME%".sln add test/"%PROJECT_NAME%".UIT

@echo.
@echo [END] Projects created for %PROJECT_NAME%.


REM -------------------------------DESIGN-----------------------------------
REM - create the design project
REM md design
REM cd design
REM dotnet new classlib -f net8.0 -n "%PROJECT_NAME%".Design
REM cd ..

REM - add the project to the solution
REM dotnet sln "%PROJECT_NAME%".sln add design/"%PROJECT_NAME%".Design
