using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.Blocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.Tests.Blocks
{
    [TestFixture]
    public class TaskListSummaryActionFilterTests
    {
        private class ExampleViewModel
        {
            public ExampleModelsBuilderModel? Page { get; set; }
        }

        private static OverridableBlockListModel CreateBlockListWithTaskListSummaryAndTaskList()
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[] {
                // Task list summary
                UmbracoBlockListFactory.CreateOverridableBlock(
                  UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.TaskListSummary).Object
                    ),
                // Task list with two tasks
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.TaskList)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.TaskListTasks,
                        CreateBlockListOfTasks())
                    .Object
                    )
            });
            return blockList;
        }

        private static OverridableBlockGridModel CreateBlockGridWithTaskListSummaryAndTaskList()
        {
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(new[] {
                // Task list summary
                UmbracoBlockGridFactory.CreateOverridableBlock(
                  UmbracoBlockGridFactory.CreateContentOrSettings(ElementTypeAliases.TaskListSummary).Object
                    ),
                // Task list with two tasks
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings(ElementTypeAliases.TaskList)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.TaskListTasks,
                        CreateBlockListOfTasks())
                    .Object
                    )
            });
            return blockGrid;
        }

        private static OverridableBlockListModel CreateBlockListOfTasks()
        {
            return UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
                                    {
                            UmbracoBlockListFactory.CreateOverridableBlock(
                                UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Task).Object,
                                UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.TaskSettings)
                                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.TaskListTaskStatus, TaskListTaskStatus.Completed.ToString())
                                .Object
                            ),
                            UmbracoBlockListFactory.CreateOverridableBlock(
                                UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Task).Object,
                                UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.TaskSettings)
                                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.TaskListTaskStatus, TaskListTaskStatus.Incomplete.ToString())
                                .Object
                            )
                        }
                                );
        }

        private static ActionExecutedContext CreateActionExecutedContext(object model)
        {
            var modelState = new ModelStateDictionary();
            var context = new ActionExecutedContext(new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), modelState), new List<IFilterMetadata>(), new { });
            var result = new ViewResult();
            var metadataProvider = new FakeMetadataProvider(typeof(ExampleModelsBuilderModel));
            var viewData = new ViewDataDictionary(metadataProvider, modelState);
            viewData.Model = model;
            result.ViewData = viewData;
            context.Result = result;
            return context;
        }

        [Test]
        public void Null_model_does_not_throw()
        {
            // Arrange
            var actionFilter = new TaskListSummaryActionFilter();
            var context = new ActionExecutedContext(new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()), new List<IFilterMetadata>(), new { });

            // Act
            Assert.DoesNotThrow(() => actionFilter.OnActionExecuted(context));
        }

        [Test]
        public void Task_statuses_added_to_ModelState_from_BlockList_in_PublishedContentModel()
        {
            // Arrange
            var blockList = CreateBlockListWithTaskListSummaryAndTaskList();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);

            var actionFilter = new TaskListSummaryActionFilter();
            var model = new ExampleModelsBuilderModel(content.Object, new NoopPublishedValueFallback());
            var context = CreateActionExecutedContext(model);

            // Act
            actionFilter.OnActionExecuted(context);

            // Assert
            Assert.That(context.ModelState.ContainsKey(blockList[0].Content.Key.ToString()));
            Assert.That(context.ModelState[blockList[0].Content.Key.ToString()]!.AttemptedValue, Is.EqualTo("Completed,Incomplete"));
        }

        [Test]
        public void Task_statuses_added_to_ModelState_from_BlockList_in_PublishedContentModel_as_property_of_viewModel()
        {
            // Arrange
            var blockList = CreateBlockListWithTaskListSummaryAndTaskList();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);

            var actionFilter = new TaskListSummaryActionFilter();
            var model = new ExampleViewModel { Page = new ExampleModelsBuilderModel(content.Object, new NoopPublishedValueFallback()) };
            var context = CreateActionExecutedContext(model);

            // Act
            actionFilter.OnActionExecuted(context);

            // Assert
            Assert.That(context.ModelState.ContainsKey(blockList[0].Content.Key.ToString()));
            Assert.That(context.ModelState[blockList[0].Content.Key.ToString()]!.AttemptedValue, Is.EqualTo("Completed,Incomplete"));
        }

        [Test]
        public void Task_statuses_added_to_ModelState_from_BlockGrid_in_PublishedContentModel()
        {
            // Arrange
            var blockGrid = CreateBlockGridWithTaskListSummaryAndTaskList();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockGridPropertyValue(nameof(ExampleModelsBuilderModel.BlockGrid), blockGrid);

            var actionFilter = new TaskListSummaryActionFilter();
            var model = new ExampleModelsBuilderModel(content.Object, new NoopPublishedValueFallback());
            var context = CreateActionExecutedContext(model);

            // Act
            actionFilter.OnActionExecuted(context);

            // Assert
            Assert.That(context.ModelState.ContainsKey(blockGrid[0].Content.Key.ToString()));
            Assert.That(context.ModelState[blockGrid[0].Content.Key.ToString()]!.AttemptedValue, Is.EqualTo("Completed,Incomplete"));
        }

        [Test]
        public void Task_statuses_added_to_ModelState_from_BlockGrid_in_PublishedContentModel_as_property_of_viewModel()
        {
            // Arrange
            var blockGrid = CreateBlockGridWithTaskListSummaryAndTaskList();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockGridPropertyValue(nameof(ExampleModelsBuilderModel.BlockGrid), blockGrid);

            var actionFilter = new TaskListSummaryActionFilter();
            var model = new ExampleViewModel { Page = new ExampleModelsBuilderModel(content.Object, new NoopPublishedValueFallback()) };
            var context = CreateActionExecutedContext(model);

            // Act
            actionFilter.OnActionExecuted(context);

            // Assert
            Assert.That(context.ModelState.ContainsKey(blockGrid[0].Content.Key.ToString()));
            Assert.That(context.ModelState[blockGrid[0].Content.Key.ToString()]!.AttemptedValue, Is.EqualTo("Completed,Incomplete"));
        }

        [Test]
        public void BlockList_filter_is_applied_for_tasks()
        {
            // Arrange
            var blockList = CreateBlockListWithTaskListSummaryAndTaskList();
            blockList.Filter = x => x.Content.ContentType.Alias != ElementTypeAliases.Task || x.Settings.Value<string>(PropertyAliases.TaskListTaskStatus) == TaskListTaskStatus.Completed.ToString();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);

            var actionFilter = new TaskListSummaryActionFilter();
            var model = new ExampleModelsBuilderModel(content.Object, new NoopPublishedValueFallback());
            var context = CreateActionExecutedContext(model);

            // Act
            actionFilter.OnActionExecuted(context);

            // Assert
            Assert.That(context.ModelState.ContainsKey(blockList[0].Content.Key.ToString()));
            Assert.That(context.ModelState[blockList[0].Content.Key.ToString()]!.AttemptedValue, Is.EqualTo("Completed"));
        }

        [Test]
        public void BlockGrid_filter_is_applied_for_tasks()
        {
            // Arrange
            var blockGrid = CreateBlockGridWithTaskListSummaryAndTaskList();
            blockGrid.Filter = x => x.Content.ContentType.Alias != ElementTypeAliases.Task || x.Settings.Value<string>(PropertyAliases.TaskListTaskStatus) == TaskListTaskStatus.Completed.ToString();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockGridPropertyValue(nameof(ExampleModelsBuilderModel.BlockGrid), blockGrid);

            var actionFilter = new TaskListSummaryActionFilter();
            var model = new ExampleModelsBuilderModel(content.Object, new NoopPublishedValueFallback());
            var context = CreateActionExecutedContext(model);

            // Act
            actionFilter.OnActionExecuted(context);

            // Assert
            Assert.That(context.ModelState.ContainsKey(blockGrid[0].Content.Key.ToString()));
            Assert.That(context.ModelState[blockGrid[0].Content.Key.ToString()]!.AttemptedValue, Is.EqualTo("Completed,Incomplete"));
        }
    }
}
