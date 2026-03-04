# Update govuk-frontend-aspnetcore and GOV.UK Frontend

This project builds on [govuk-frontend-aspnetcore](https://github.com/gunndabad/govuk-frontend-aspnetcore) by James Gunn, which in turn builds on [GOV.UK Frontend](https://github.com/alphagov/govuk-frontend).

When you update either you must:

- check the release notes for both projects for changes that we may need to implement
- update `govuk-frontend-aspnetcore` in both the `ThePensionsRegulator.GovUk.Frontend` and `ThePensionsRegulator.Frontend` packages.
- update the GOV.UK Frontend submodule (see below)
- update the GOV.UK Frontend npm package (see below)
- update the version of GOV.UK Frontend in the following files:
  - [README.md](../../README.md)
  - [govuk-js-init.js](../../ThePensionsRegulator.GovUk.Frontend/wwwroot/ThePensionsRegulator.GovUk.Frontend/js/govuk-js-init.js)
  - [tpr-search-results.js](../../ThePensionsRegulator.Frontend/wwwroot/ThePensionsRegulator.Frontend/js/tpr-search-results.js)

This project references GOV.UK Frontend as a submodule, so that we can import and use the SASS code. This reference **must** be kept aligned with the version referenced by `govuk-frontend-aspnetcore`, therefore any time you update the `govuk-frontend-aspnetcore` NuGet package you should check the release notes to see whether you need to update GOV.UK Frontend.

This project also references GOV.UK Frontend as an NPM package, so that we can use the test fixtures for conformance tests. These do not exist in the source repository that we reference as a submodule.

This example shows how to update the reference to GOV.UK Frontend.

```cmd
cd lib\govuk-frontend
git pull
git checkout v4.3.0
cd ..\..
npm install govuk-frontend@4.3.0
git commit -am "Update govuk-frontend to v4.3.0"
```

If you've worked on this project before it's possible that someone else has updated the version of GOV.UK Frontend in the meantime. You should run `git submodule update` and `npm install` periodically to ensure you stay up-to-date.
