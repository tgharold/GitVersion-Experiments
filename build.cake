
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
        settings.SetConfiguration(configuration))
            .WithProperty("TreatWarningsAsErrors", "True")
            .WithProperty("DeployOnBuild", "True")
            .SetVerbosity(Verbosity.Minimal)
            .AddFileLogger());
    }
    else
    {
      // Use XBuild
      XBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration))
            .WithProperty("TreatWarningsAsErrors", "True")
            .WithProperty("DeployOnBuild", "True")
            .SetVerbosity(Verbosity.Minimal)
            .AddFileLogger());
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./src/**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

RunTarget(target);
