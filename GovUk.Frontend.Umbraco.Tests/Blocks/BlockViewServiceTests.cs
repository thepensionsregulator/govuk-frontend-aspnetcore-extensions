using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace GovUk.Frontend.Umbraco.Tests.Blocks
{
    [TestFixture]
    public class BlockViewServiceTests
    {
# nullable disable
        private Mock<IGovUkGridClassBuilder> _gridClassBuilder;
        private Mock<IGovUkFieldsetErrorFinder> _fieldsetErrorFinder;
        private BlockViewService _blockViewService;
#nullable enable

        [SetUp]
        public void SetupMocks()
        {
            _gridClassBuilder = new();
            _fieldsetErrorFinder = new();

            _blockViewService = new(_gridClassBuilder.Object, _fieldsetErrorFinder.Object);
        }

        [Test]
        public void OverridableBlockGridModel_applies_filter()
        {
            // Arrange
            const string ALLOWED = "allowed";
            const string NOT_ALLOWED = "notAllowed";

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock(ALLOWED),
                UmbracoBlockGridFactory.CreateOverridableBlock(NOT_ALLOWED)
                ]);
            model.Filter = block => block.Content.ContentType.Alias == ALLOWED;

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Block.Content.ContentType.Alias, Is.EqualTo(ALLOWED));
        }

        [Test]
        public void OverridableBlockAreaModel_applies_filter()
        {
            // Arrange
            const string ALLOWED = "allowed";
            const string NOT_ALLOWED = "notAllowed";

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([
                UmbracoBlockGridFactory.CreateOverridableBlock(ALLOWED),
                UmbracoBlockGridFactory.CreateOverridableBlock(NOT_ALLOWED)
                ], "area");
            model.Filter = block => block.Content.ContentType.Alias == ALLOWED;

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Block.Content.ContentType.Alias, Is.EqualTo(ALLOWED));
        }

        [Test]
        public void No_blocks_returns_empty_list()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([]);

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result, Is.Empty);
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        public void Fieldset_error_classes_are_applied_if_there_are_fieldset_errors_and_legend_is_not_page_heading(bool hasErrors, bool legendIsPageHeading, bool expectClasses)
        {
            // Arrange
            const string FIELDSET_ERROR_CLASS = "govuk-form-group govuk-form-group--error";

            var modelState = new ModelStateDictionary();
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoBooleanPropertyValue(PropertyAliases.FieldsetLegendIsPageHeading, legendIsPageHeading)
                        .Object
                    )
                ]);
            var errors = hasErrors ? [UmbracoBlockGridFactory.CreateOverridableBlock(ElementTypeAliases.ErrorMessage)] : Array.Empty<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            _ = _fieldsetErrorFinder.Setup(x => x.FindErrors(model.First(), modelState)).Returns(errors);

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, modelState);

            // Assert
            if (expectClasses)
            {
                Assert.That(result.First().FieldsetErrorClasses, Is.EqualTo(FIELDSET_ERROR_CLASS));
            }
            else
            {
                Assert.That(result.First().FieldsetErrorClasses, Is.Null);
            }
        }

        [Test]
        public void Row_and_column_class_are_set_from_GovUkGridClassBuilder()
        {
            // Arrange
            const string ROW_CLASS = "example-row";
            const string COLUMN_CLASS = "example-column";

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                ]);
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(null)).Returns(ROW_CLASS);
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, null, "alias", false)).Returns(COLUMN_CLASS);

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().RowClasses, Is.EqualTo(ROW_CLASS));
            Assert.That(result.First().ColumnClasses, Is.EqualTo(COLUMN_CLASS));
        }

        [Test]
        public void HasGridAreas_is_set_from_Areas()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                    .AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area")),
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                 ]);

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().HasGridAreas, Is.True);
            Assert.That(result.Last().HasGridAreas, Is.False);
        }

        // true if everything the same with default row classes
        [TestCase(true, true, "", "", "", "", true)]
        [TestCase(false, false, "", "", "", "", true)]

        // false if grid areas different
        [TestCase(true, false, "", "", "", "", false)]
        [TestCase(false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, "custom", "", "", "", false)]
        [TestCase(false, false, "", "custom", "", "", false)]
        [TestCase(false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, "", "", "custom", "", false)]
        [TestCase(false, false, "", "", "", "custom", false)]
        [TestCase(false, false, "", "", "custom1", "custom2", false)]
        public void SameAsNext_matches_on_grid_areas_and_row_class_and_column_class(
            bool currentBlockHasAreas,
            bool nextBlockHasAreas,
            string currentBlockRowClass,
            string nextBlockRowClass,
            string currentBlockColumnClass,
            string nextBlockColumnClass,
            bool expectSameAsNext
            )
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, currentBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, currentBlockColumnClass)
                        .Object
                     ),
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, nextBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, nextBlockColumnClass)
                        .Object
                    )
                 ]);

            if (currentBlockHasAreas)
            {
                model[0].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }
            if (nextBlockHasAreas)
            {
                model[1].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(currentBlockRowClass)).Returns($"{HtmlClassNames.Row} {currentBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(nextBlockRowClass)).Returns($"{HtmlClassNames.Row} {nextBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, currentBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {currentBlockColumnClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, nextBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {nextBlockColumnClass}");

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsSameAsNext, Is.EqualTo(expectSameAsNext));
            Assert.That(result.Last().IsSameAsNext, Is.False); // last one should always be false
        }

        // true if everything the same with default row classes
        [TestCase(true, true, "", "", "", "", true)]
        [TestCase(false, false, "", "", "", "", true)]

        // false if grid areas different
        [TestCase(true, false, "", "", "", "", false)]
        [TestCase(false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, "custom", "", "", "", false)]
        [TestCase(false, false, "", "custom", "", "", false)]
        [TestCase(false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, "", "", "custom", "", false)]
        [TestCase(false, false, "", "", "", "custom", false)]
        [TestCase(false, false, "", "", "custom1", "custom2", false)]
        public void SameAsPrevious_matches_on_grid_areas_and_row_class_and_column_class(
            bool previousBlockHasAreas,
            bool currentBlockHasAreas,
            string previousBlockRowClass,
            string currentBlockRowClass,
            string previousBlockColumnClass,
            string currentBlockColumnClass,
            bool expectSameAsPrevious
            )
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, previousBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, previousBlockColumnClass)
                        .Object
                     ),
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, currentBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, currentBlockColumnClass)
                        .Object
                    )
                 ]);

            if (previousBlockHasAreas)
            {
                model[0].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }
            if (currentBlockHasAreas)
            {
                model[1].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(previousBlockRowClass)).Returns($"{HtmlClassNames.Row} {previousBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(currentBlockRowClass)).Returns($"{HtmlClassNames.Row} {currentBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, previousBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {previousBlockColumnClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, currentBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {currentBlockColumnClass}");

            // Act
            var result = _blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsSameAsPrevious, Is.False); // first one should always be false
            Assert.That(result.Last().IsSameAsPrevious, Is.EqualTo(expectSameAsPrevious));
        }
    }
}
