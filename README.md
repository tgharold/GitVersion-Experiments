# GitVersion-Experiments

Experimenting with the various GitVersion modes

# Setup

I'm using .NET Core, but running GitVersion 4.0.0-beta.12 on Windows.  Assuming that you could get GitVersion 4.x on another environment, you could probably test there as well.

# Reference links:

- [GitVersion vs dotnet pack on VSTS](https://tech.trailmax.info/2017/11/gitversion-vs-dotnet-pack-on-vsts/): 
Talks about creating a `SolutionInfo.cs` file in the root of the project (same place as the .sln file) along with adding it as an "ItemGroup Compile Include" in your project's `.csproj` file.

