@echo off
rem Build the projects
echo Building projects...

cd Floxel
echo Building Floxel...
dotnet build -c Release
cd ..

cd Floxel.Generator
echo Building Floxel.Generator...
dotnet build -c Release
cd ..

rem Pack NuGet package
echo Packing NuGet package...
nuget pack floxel.nuspec -OutputDirectory .\builds