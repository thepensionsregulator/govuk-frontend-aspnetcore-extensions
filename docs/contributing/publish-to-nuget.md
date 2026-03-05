# Publish a new version to nuget.org

Update the version of our packages in `Directory.Build.props` at the root of this repo. Name the version using [semantic versioning](https://semver.org/). For example, if you are making a breaking change to `v1.0.0`, your version should be `v2.0.0`. A non-breaking change introducing a new feature should be `v1.1.0`, and a non-breaking bug fix should be `v1.0.1`.

All of our packages are versioned together even when not all of them change, so that installing or upgrading `ThePensionsRegulator.GovUk.Frontend.Umbraco` or `ThePensionsRegulator.Frontend.Umbraco` will always bring in the latest packages as transitive dependencies.

Tag the commit you want to publish from. The tag must be the same as your version number, including the letter `v`.

```cmd
git tag v2.0.0
git push origin develop --tags
```

For anything other than an alpha release, add details of the release to the [Releases](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/releases) section on Github.

To publish to nuget.org, run the `govuk-frontend-aspnetcore-extensions` pipeline in Azure DevOps. In the 'Run pipeline' dialog for the pipeline specify your tag in the format `refs/tags/<tag-name>`.

![Specify a tag in the run pipeline dialog](/docs/images/run-pipeline-from-tag.png)

## Updating the API key

Publishing uses an API key generated on nuget.org by an individual developer. This has an expiry date. If the pipeline breaks due to an expired API key, generate a new one on nuget.org using the following settings:

- Scope: `Push new packages and package versions`
- Package owner: `ThePensionsRegulator`
- Glob pattern: `ThePensionsRegulator.*`

In Azure DevOps go to Project Settings > Service Connections and edit the `NUGET` service connection which is named in our pipeline. Enter the new API key.
