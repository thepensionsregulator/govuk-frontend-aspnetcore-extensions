# TypeScript for client-side scripts

Client-side scripts for `ThePensionsRegulator.Frontend` and `ThePensionsRegulator.GovUk.Frontend` can be written in [TypeScript](https://www.typescriptlang.org/) as well as plain JavaScript. This is separate from the Umbraco backoffice TypeScript setup described in [Umbraco backoffice development](/docs/contributing/backoffice-development.md), which uses Vite and is not covered here.

## Where source files live

Source scripts (`.ts` or `.js`) live in a `Scripts` folder at the root of the project, for example `ThePensionsRegulator.Frontend\Scripts`.

`wwwroot\<package-name>\js` is **generated output**. Do not hand-edit or add files directly there, and do not add new files to source control in that folder — the compiler and bundler regenerate it on every build. This mirrors how `wwwroot\<package-name>\css` is generated from SASS files in the `Styles` folder (see [Include client-side files in packages](/docs/contributing/include-client-side-files-in-packages.md)).

## How the build works

Each project has its own `tsconfig.json` which sets `rootDir` to `Scripts` and `outDir` to that project's `wwwroot\<package-name>\js` folder. The `Microsoft.TypeScript.MSBuild` NuGet package runs the TypeScript compiler automatically as part of `dotnet build` (and therefore also `dotnet pack`), compiling every `.ts` and `.js` file in `Scripts` into `wwwroot`.

`BuildBundlerMinifier` (used for minification, see [Include client-side files in packages](/docs/contributing/include-client-side-files-in-packages.md)) runs immediately after the TypeScript compiler as part of the same build, so `.min.js` files are generated from the freshly compiled output. You get both the compiled `.js` and minified `.min.js` from a single `dotnet build` — you don't need to run anything extra.

To add a new script:

1. Add a `.ts` (or `.js`) file to the project's `Scripts` folder.
2. Add an entry to `bundleconfig.json` pointing at the compiled output path in `wwwroot\<package-name>\js`, the same way you would for a hand-written `.js` file.
3. Build the project (`dotnet build`, or build in Visual Studio). The compiled and minified files will appear in `wwwroot`.

## Working with TypeScript in Visual Studio

Visual Studio automatically re-runs the TypeScript compiler in the background (via a design-time build) when it detects certain project changes, such as a file being added to or removed from the project. This can make a deleted `wwwroot` output file reappear immediately — this is expected, because it's regenerated from the source file in `Scripts`. To remove a script, delete or rename its source file in `Scripts`, not the generated file in `wwwroot`.

Editing the *contents* of an existing source file does not automatically trigger a rebuild. To see your changes reflected in `wwwroot`, either:

- Build the project (`Ctrl+Shift+B` in Visual Studio, or `dotnet build`) — this also regenerates `.min.js`, or
- Run `npm run watch:ts:frontend` or `npm run watch:ts:govuk-frontend` from the root of the repo while you work. This recompiles `Scripts` to `wwwroot` as you save, but does **not** run the minifier, so use this for quick local iteration and do a full `dotnet build` before committing or testing minified output.

## Type-checking and tests

`npm install` at the root of the repo installs TypeScript, [ts-jest](https://kulshekhar.github.io/ts-jest/) and associated type declarations, so Jest can run `.ts` test files under `__tests__` alongside existing `.js` tests (see [Run tests](/docs/contributing/run-tests.md)).

The root `tsconfig.json` is a type-checking-only configuration (it doesn't emit any output) covering both projects' `Scripts` and `__tests__` folders together. Use it to type-check the whole repo, including tests, without needing a full `dotnet build`:

```cmd
npx tsc -p tsconfig.json --noEmit
```

## Available npm scripts

| Script | Description |
| --- | --- |
| `npm run build:ts` | Compiles TypeScript for both projects (no minification — use `dotnet build` for that). |
| `npm run build:ts:frontend` | Compiles TypeScript for `ThePensionsRegulator.Frontend` only. |
| `npm run build:ts:govuk-frontend` | Compiles TypeScript for `ThePensionsRegulator.GovUk.Frontend` only. |
| `npm run watch:ts:frontend` | Watches and recompiles `ThePensionsRegulator.Frontend`'s `Scripts` folder as you edit. |
| `npm run watch:ts:govuk-frontend` | Watches and recompiles `ThePensionsRegulator.GovUk.Frontend`'s `Scripts` folder as you edit. |
