# GitVersion-Experiments

Experimenting with the various GitVersion modes, plus experimentation with .NET Core and Cake.

# Reference links:

- [GitVersion vs dotnet pack on VSTS](https://tech.trailmax.info/2017/11/gitversion-vs-dotnet-pack-on-vsts/): 
Talks about creating a `SolutionInfo.cs` file in the root of the project (same place as the .sln file) along with adding it as an "ItemGroup Compile Include" in your project's `.csproj` file.
- [Linux bash Cake bootstrap script](https://gist.github.com/SamuelDebruyn/78a6107cc5dbe89f422a0d1c24435dd7): Builds a fake .NET Core project and does a nuget restore to bring down the .NET Core version of Cake, then runs it with "`dotnet run`".

