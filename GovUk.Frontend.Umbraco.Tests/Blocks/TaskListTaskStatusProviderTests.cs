using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.Blocks;
using NUnit.Framework;
using System;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace GovUk.Frontend.Umbraco.Tests.Blocks
{
    [TestFixture]
    public class TaskListTaskStatusProviderTests
    {
        private static OverridableBlockListModel CreateBlockListWithTaskListSummaryAndTaskList(OverridableBlockListModel blockListOfTasks)
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
                        blockListOfTasks)
                    .Object
                    )
            });
            return blockList;
        }

        private static OverridableBlockGridModel CreateBlockGridWithTaskListSummaryAndTaskList(OverridableBlockListModel blockListOfTasks)
        {
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(new[] {
                // Task list summary
                UmbracoBlockGridFactory.CreateOverridableBlock(
                  UmbracoBlockGridFactory.CreateContentOrSettings(ElementTypeAliases.TaskListSummary).Object
                    ),
                // Task list with block list of tasks
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings(ElementTypeAliases.TaskList)
                    .SetupUmbracoBlockListPropertyValue(PropertyAliases.TaskListTasks,
                        blockListOfTasks)
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

        [Test]
        public void Null_content_throws_ArgumentNullException()
        {
            // Arrange
            var provider = new TaskListTaskStatusProvider();

            // Act
#nullable disable
            Assert.Throws<ArgumentNullException>(() => provider.FindTaskStatuses(null));
#nullable enable
        }

        [Test]
        public void Task_statuses_returned_from_BlockList()
        {
            // Arrange
            var blockList = CreateBlockListWithTaskListSummaryAndTaskList(CreateBlockListOfTasks());

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);

            var provider = new TaskListTaskStatusProvider();

            // Act
            var result = provider.FindTaskStatuses(content.Object).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Contains(TaskListTaskStatus.Completed));
            Assert.That(result.Contains(TaskListTaskStatus.Incomplete));
        }

        [Test]
        public void Task_statuses_returned_from_BlockGrid()
        {
            // Arrange
            var blockGrid = CreateBlockGridWithTaskListSummaryAndTaskList(CreateBlockListOfTasks());

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockGridPropertyValue(nameof(ExampleModelsBuilderModel.BlockGrid), blockGrid);

            var provider = new TaskListTaskStatusProvider();

            // Act
            var result = provider.FindTaskStatuses(content.Object).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Contains(TaskListTaskStatus.Completed));
            Assert.That(result.Contains(TaskListTaskStatus.Incomplete));
        }

        [Test]
        public void BlockList_filter_is_applied_for_tasks()
        {
            // Arrange
            var blockList = CreateBlockListWithTaskListSummaryAndTaskList(CreateBlockListOfTasks());
            blockList.Filter = x => x.Content.ContentType.Alias != ElementTypeAliases.Task || x.Settings.Value<string>(PropertyAliases.TaskListTaskStatus) == TaskListTaskStatus.Completed.ToString();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);

            var provider = new TaskListTaskStatusProvider();

            // Act
            var result = provider.FindTaskStatuses(content.Object).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Contains(TaskListTaskStatus.Completed));
        }

        [Test]
        public void BlockGrid_filter_is_not_applied_for_tasks_which_are_block_list_items()
        {
            // Arrange
            var blockGrid = CreateBlockGridWithTaskListSummaryAndTaskList(CreateBlockListOfTasks());
            blockGrid.Filter = x => x.Content.ContentType.Alias != ElementTypeAliases.Task || x.Settings.Value<string>(PropertyAliases.TaskListTaskStatus) == TaskListTaskStatus.Completed.ToString();

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockGridPropertyValue(nameof(ExampleModelsBuilderModel.BlockGrid), blockGrid);

            var provider = new TaskListTaskStatusProvider();

            // Act
            var result = provider.FindTaskStatuses(content.Object).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Contains(TaskListTaskStatus.Completed));
            Assert.That(result.Contains(TaskListTaskStatus.Incomplete));
        }

        [Test]
        public void Overridden_status_is_applied_for_tasks()
        {
            // Arrange
            var blockListOfTasks = UmbracoBlockListFactory.CreateOverridableBlockListModel(new[]
            {
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.Task).Object,
                    // Must use a non-mocked OverridablePublishedElement for this test
                    new OverridablePublishedElement(UmbracoContentFactory.CreateContent<IPublishedElement>(ElementTypeAliases.TaskSettings)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.TaskListTaskStatus, TaskListTaskStatus.Completed.ToString())
                    .Object)
                )
            });
            blockListOfTasks[0].Settings.OverrideValue(PropertyAliases.TaskListTaskStatus, TaskListTaskStatus.NotStarted.ToString());

            var blockList = CreateBlockListWithTaskListSummaryAndTaskList(blockListOfTasks);

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>()
                .SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);

            var provider = new TaskListTaskStatusProvider();

            // Act
            var result = provider.FindTaskStatuses(content.Object).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.Contains(TaskListTaskStatus.NotStarted));
        }
    }
}
