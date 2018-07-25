
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
var solutionFile = "./GitVersion-Experiments.sln";
var solutionDir = new FilePath(solutionFile).GetDirectory();
var shouldBuildNugetSourcePackage = true;

// assumptions that rarely change
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildDir = Directory("./artifacts") + Directory(configuration);
var projectsPattern = "/**/*.csproj";
var testsPattern = "/**/*.Tests.csproj";

// Versioning
var buildVersion = EnvironmentVariable("GitVersion_NuGetVersionV2");
var buildAssemblyVersion = EnvironmentVariable("GitVersion_NuGetVersionV2");
var buildFileVersion = EnvironmentVariable("GitVersion_NuGetVersionV2");
var buildAssemblyInformationalVersion = EnvironmentVariable("GitVersion_InformationalVersion");

Task("Clean")
    .Does(() =>
    {
        DotNetCoreClean(solutionFile);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(solutionFile);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        DotNetCoreBuild(solutionFile, 
            new DotNetCoreBuildSettings()
            {
                Configuration = configuration
            }
        );
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        /* This should build the command:
         * $ dotnet test --no-build --configuration=Release .\tests\ExampleProject.Tests\
         */

        var projects = GetFiles(solutionDir + testsPattern);
        foreach(var project in projects)
        {
            DotNetCoreTest(
                project.FullPath,
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true
                }
            );
        }
    });    

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
    {
        var projects = GetFiles(solutionDir + projectsPattern)
            - GetFiles(solutionDir + testsPattern);

        var settings = new DotNetCorePackSettings {
            NoBuild = true,
            Configuration = configuration,
            OutputDirectory = buildDir,
            ArgumentCustomization = (args) => {
                if (shouldBuildNugetSourcePackage)
                {
                    args.Append("--include-source");
                }
                return args
                    .Append("/p:Version={0}", buildVersion)
                    .Append("/p:AssemblyVersion={0}", buildAssemblyVersion)
                    .Append("/p:FileVersion={0}", buildFileVersion)
                    .Append("/p:AssemblyInformationalVersion={0}", buildAssemblyInformationalVersion);
            }
        };

        foreach (var project in projects)
        {
            DotNetCorePack(project.ToString(), settings);
        }
    });

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

RunTarget(target);
