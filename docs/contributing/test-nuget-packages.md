# Test pre-release NuGet packages in a consuming application

When you need to test the NuGet packages published by this solution in a consuming application before submitting your code in a pull request, follow these steps:

1. Open the consuming application in Visual Studio.
2. In 'Tools > NuGet Package Manager > Package Manager Settings' add one or more NuGet package sources pointing at the `bin\Debug` folder(s) of the package(s) you want to test. For example, to test a new TPR component in a non-Umbraco application, add a package source pointing at `ThePensionsRegulator.Frontend\bin\Debug`.
3. Open this solution in a separate instance of Visual Studio.
4. Add a pre-release tag to the version in `Solution Items/Directory.Build.props`. For example, `<Version>7.0.0</Version>` becomes `<Version>7.0.0-beta001</Version>`. Using three digits with a leading zero allows you plenty of iterations while helping Visual Studio to know which is the latest.
5. Ensure you build configuration is set to 'Debug', and build the project(s) you want to test. This will generate an updated NuGet package in the `bin\Debug` folder of each project.
6. Return to the consuming application and open the 'Manage NuGet Packages' dialog. Go to the 'Installed' tab and ensure that the 'Include prerelease' box is ticked, and the package source is set to either 'All' or the new package source you set up. Enter 'ThePensionsRegulator' in the search box. Your test version should appear as an available upgrade.
7. For each iteration of your testing, increment the number in your pre-release tag and re-build the project to make the new package version available to the consuming application.
