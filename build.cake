
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
var solutionFile = "./GitVersion-Experiments.sln";
var solutionDir = new FilePath(solutionFile).GetDirectory();

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var buildDir = Directory("./artifacts") + Directory(configuration);

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
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
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration)
            .WithProperty("TreatWarningsAsErrors", "True")
            .WithProperty("DeployOnBuild", "True")
            .SetVerbosity(Verbosity.Minimal)
            );
    }
    else
    {
      // Use XBuild
      XBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration)
            .WithProperty("TreatWarningsAsErrors", "True")
            .WithProperty("DeployOnBuild", "True")
            .SetVerbosity(Verbosity.Minimal)
            );
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    XUnit2("./test/**/bin/" + configuration + "/*.Tests.dll", new XUnit2Settings()
    {
        Parallelism = ParallelismOption.All,
        HtmlReport = true,
        OutputDirectory = buildDir
    });
});

Task("Package")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    var projects = GetFiles(solutionDir + "/**/*.csproj")
        - GetFiles(solutionDir + "/**/*.Tests.csproj");

    var buildVersion = EnvironmentVariable("GitVersion_NuGetVersionV2");
    var buildAssemblyVersion = EnvironmentVariable("GitVersion_NuGetVersionV2");
    var buildFileVersion = EnvironmentVariable("GitVersion_NuGetVersionV2");
    var buildAssemblyInformationalVersion = EnvironmentVariable("GitVersion_InformationalVersion");

    var settings = new DotNetCorePackSettings {
        NoBuild = true,
        Configuration = configuration,
        OutputDirectory = buildDir,
        ArgumentCustomization = (args) => {
            //if (BuildParameters.ShouldBuildNugetSourcePackage)
            //{
            //    args.Append("--include-source");
            //}
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
