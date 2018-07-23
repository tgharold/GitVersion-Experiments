
#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
var solutionFile = "./GitVersion-Experiments.sln";

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

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

RunTarget(target);
