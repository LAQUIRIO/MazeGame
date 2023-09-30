@echo off
rem To run requires that OpenCover.Console.exe be in the path https://github.com/OpenCover/opencover
rem Additionally requires the reportgenerator be installed and in the path https://www.nuget.org/packages/dotnet-reportgenerator-globaltool
rem This script is meant to be run from the current folder

set dotnet_path="C:\Program Files\dotnet\dotnet.exe"
set test_args="test MazeTests.csproj"
set output_file="coverage.xml"
set filter="+[Maze*]* -[Maze*Test*]*"
touch %output_file%
C:\Users\laqui\AppData\Local\Apps\OpenCover\OpenCover.Console.exe -target:%dotnet_path% -targetargs:%test_args% -output:%output_file% -filter:%filter% -register:user
reportgenerator -reports:%output_file% -targetdir:coveragereport -reporttypes:Html
touch coveragereport\index.html
start coveragereport\index.html