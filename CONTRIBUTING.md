# Contributing to govuk-frontend-aspnetcore-extensions

## Updating the version of GOV.UK Frontend

This project builds on [ASP.NET Core MVC tag helpers for GOV.UK Design System](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, which in turn builds on [GOV.UK Frontend](https://github.com/alphagov/govuk-frontend).

This project references GOV.UK Frontend as a submodule, so that we can import and use the SASS code. This reference **must** be kept aligned with the version referenced by govuk-frontend-aspnetcore, therefore any time you update the govuk-frontend-aspnetcore NuGet package you should check the release notes to see whether you need to update GOV.UK Frontend.

This example shows how to update the reference to GOV.UK Frontend:

```cmd
cd lib\govuk-frontend
git pull
git checkout v4.3.0
cd ..\..
git commit -am "Update govuk-frontend to v4.3.0"
```
