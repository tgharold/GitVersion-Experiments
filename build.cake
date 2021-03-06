
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=GitVersion.CommandLine"

var solutionFile = "./GitVersion-Experiments.sln";
var solutionDir = new FilePath(solutionFile).GetDirectory();
var shouldBuildNugetSourcePackage = true;

// assumptions that rarely change
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildDir = Directory("./artifacts") + Directory(configuration);
var projectsPattern = "**/*.csproj";
var testProjectsPattern = "**/*Tests.csproj";

// Versioning
var buildVersion = "0.0.0";
var buildAssemblyVersion = buildVersion;
var buildFileVersion = buildVersion;
var buildAssemblyInformationalVersion = buildVersion;

// basic sanity checks
if (!FileExists(solutionFile)) throw new Exception($"File not found: {solutionFile}");

Task("Default")
    .IsDependentOn("Package");

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

        Debug($"solutionDir: {solutionDir}");
        Debug($"testProjectsPattern: {testProjectsPattern}");
        var projects = GetFiles(solutionDir + testProjectsPattern);
        foreach(var project in projects)
        {
            Debug($"Project: {project}");
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

Task("GitVersion")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    var result = GitVersion(new GitVersionSettings {
        // UpdateAssemblyInfo = true
    });
    buildVersion = result.NuGetVersionV2;
    buildAssemblyVersion = result.NuGetVersionV2;
    buildFileVersion = result.NuGetVersionV2;
    buildAssemblyInformationalVersion = result.InformationalVersion;
});

Task("Package")
    .IsDependentOn("GitVersion")
    .Does(() =>
    {
        Debug($"solutionDir: {solutionDir}");
        Debug($"projectsPattern: {projectsPattern}");
        Debug($"testProjectsPattern: {testProjectsPattern}");

        var projects = GetFiles(solutionDir + projectsPattern)
            - GetFiles(solutionDir + testProjectsPattern);

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
            Debug($"Project: {project}");
            DotNetCorePack(project.ToString(), settings);
        }
    });

RunTarget(target);
