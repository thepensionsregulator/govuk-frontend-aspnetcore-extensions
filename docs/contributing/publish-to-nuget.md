# Publish a new version to nuget.org

Tag the commit you want to publish from. Name your tag using semantic versioning. For example, if you are upgrading all packages from v1.0.0 to v2.0.0, your tag should be `v2.0.0`. If you are upgrading only one package then append the package name - for example `v2.0.0-ThePensionsRegulator.GovUk.Frontend`.

```cmd
git tag v2.0.0
git push origin develop --tags
```

For anything other than an alpha release, add details of the release to the [Releases](https://github.com/thepensionsregulator/govuk-frontend-aspnetcore-extensions/releases) section on Github.

To publish to nuget.org, run `azure-pipelines.yml` in Azure DevOps. In the 'Run pipeline' dialog for the pipeline specify your tag in the format `refs/tags/<tag-name>`. Tick the boxes for the packages you want to publish.

![Specify a tag in the run pipeline dialog](/docs/images/run-pipeline-from-tag.png)
