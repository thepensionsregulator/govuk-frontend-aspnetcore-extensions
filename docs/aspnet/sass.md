# Use SASS for CSS

You can use GOV.UK and TPR SASS styles and utilities in your project when you add the `ThePensionsRegulator.GovUk.Frontend` NuGet package.

1. Install the [AspNetCore.SassCompiler](https://www.nuget.org/packages/AspNetCore.SassCompiler) NuGet package. You must explicitly install this package even though it is already a transative dependency.
2. Create your SASS file in the `Styles` folder of your application. You can add imports at the top of your file to make GOV.UK and TPR styles and utilities available:

   ```sass
   @import '_variables.scss';
   @import 'govuk/base';
   ```

   You can then use GOV.UK functions like `govuk-px-to-rem(10px)` and TPR variables like `$tpr-colour-zambezi`.

3. Compiled CSS will be output in the `wwwroot\css` folder. You should exclude this file from source control.

See the [AspNetCore.SassCompiler Github repo](https://github.com/koenvzeijl/AspNetCore.SassCompiler) and [Dart SASS documentation](https://sass-lang.com/documentation/cli/dart-sass) for details of how to customise this build process.
