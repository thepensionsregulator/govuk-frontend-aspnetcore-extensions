using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
	public partial class ComponentGenerator
	{
		internal const string TaskListElement = "dl";
		internal const string TaskListRowActionElement = "a";
		internal const string TaskListRowActionsElement = "dd";
		internal const string TaskListRowElement = "div";
		internal const string TaskListRowKeyElement = "dt";
		internal const string TaskListRowValueElement = "dd";

		public TagBuilder GenerateTaskList(
			AttributeDictionary? attributes,
			IEnumerable<TaskListRow> rows)
		{
			Guard.ArgumentNotNull(nameof(rows), rows);

			var anyRowHasActions = rows.Any(r => r.Actions?.Items.Any() == true);

			var tagBuilder = new TagBuilder(TaskListElement);
			tagBuilder.MergeAttributes(attributes);
			tagBuilder.MergeCssClass("govuk-task-list");

			var index = 0;
			foreach (var row in rows)
			{
				Guard.ArgumentValidNotNull(
					nameof(rows),
					$"Row {index} is not valid; {nameof(TaskListRow.Key)} cannot be null.",
					row.Key,
					row.Key != null);

				Guard.ArgumentValidNotNull(
					nameof(rows),
					$"Row {index} is not valid; {nameof(TaskListRow.Key)}.{nameof(TaskListRow.Key.Content)} cannot be null.",
					row.Key.Content,
					row.Key.Content != null);

				Guard.ArgumentValidNotNull(
					nameof(rows),
					$"Row {index} is not valid; {nameof(TaskListRow.Value)} cannot be null.",
					row.Value,
					row.Value != null);

				Guard.ArgumentValidNotNull(
					nameof(rows),
					$"Row {index} is not valid; {nameof(TaskListRow.Value)}.{nameof(TaskListRow.Value.Content)} cannot be null.",
					row.Value.Content,
					row.Value.Content != null);

				var thisRowHasActions = row.Actions?.Items.Any() == true;

				var rowTagBuilder = new TagBuilder(TaskListRowElement);
				rowTagBuilder.MergeAttributes(row.Attributes);
				rowTagBuilder.MergeCssClass("govuk-task-list__row");

				if (anyRowHasActions && !thisRowHasActions)
				{
					rowTagBuilder.MergeCssClass("govuk-task-list__row--no-actions");
				}

				var dt = new TagBuilder(TaskListRowKeyElement);
				dt.MergeAttributes(row.Key.Attributes);
				dt.MergeCssClass("govuk-task-list__key");
				dt.InnerHtml.AppendHtml(row.Key.Content);
				rowTagBuilder.InnerHtml.AppendHtml(dt);

				var dd = new TagBuilder(TaskListRowValueElement);
				dd.MergeAttributes(row.Value.Attributes);
				dd.MergeCssClass("govuk-task-list__value");
				dd.InnerHtml.AppendHtml(row.Value.Content);
				rowTagBuilder.InnerHtml.AppendHtml(dd);

				if (thisRowHasActions)
				{
					var actionsDd = new TagBuilder(TaskListRowActionsElement);
					actionsDd.MergeAttributes(row.Actions!.Attributes);
					actionsDd.MergeCssClass("govuk-task-list__actions");

					if (row.Actions.Items.Count() == 1)
					{
						actionsDd.InnerHtml.AppendHtml(GenerateLink(row.Actions.Items.Single()));
					}
					else
					{
						var ul = new TagBuilder("ul");
						ul.MergeCssClass("govuk-task-list__actions-list");

						foreach (var action in row.Actions.Items!)
						{
							var li = new TagBuilder("li");
							li.MergeCssClass("govuk-task-list__actions-list-item");
							li.InnerHtml.AppendHtml(GenerateLink(action));

							ul.InnerHtml.AppendHtml(li);
						}

						actionsDd.InnerHtml.AppendHtml(ul);
					}

					rowTagBuilder.InnerHtml.AppendHtml(actionsDd);
				}

				tagBuilder.InnerHtml.AppendHtml(rowTagBuilder);

				index++;
			}

			return tagBuilder;

			static TagBuilder GenerateLink(TaskListRowAction action)
			{
				var anchor = new TagBuilder(TaskListRowActionElement);
				anchor.MergeAttributes(action.Attributes);
				anchor.MergeCssClass("govuk-link");
				anchor.InnerHtml.AppendHtml(action.Content);

				if (action.VisuallyHiddenText != null)
				{
					var vht = new TagBuilder("span");
					vht.MergeCssClass("govuk-visually-hidden");
					vht.InnerHtml.Append(action.VisuallyHiddenText);
					anchor.InnerHtml.AppendHtml(vht);
				}

				return anchor;
			}
		}
	}
}