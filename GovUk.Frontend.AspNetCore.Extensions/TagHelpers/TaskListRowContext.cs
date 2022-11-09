using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TaskListRowContext
    {
        private readonly List<TaskListRowAction> _actions;

        public TaskListRowContext()
        {
            _actions = new List<TaskListRowAction>();
        }

        public IReadOnlyList<TaskListRowAction> Actions => _actions;

        public AttributeDictionary? ActionsAttributes { get; private set; }

        public (AttributeDictionary Attributes, IHtmlContent Content)? Key { get; private set; }

        public (AttributeDictionary Attributes, IHtmlContent Content)? Value { get; private set; }

        public void AddAction(TaskListRowAction action)
        {
            Guard.ArgumentNotNull(nameof(action), action);

            _actions.Add(action);
        }

        public void SetActionsAttributes(AttributeDictionary attributes)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);

            if (ActionsAttributes != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TaskListRowActionsTagHelper.TagName,
                    TaskListRowTagHelper.TagName);
            }

            if (_actions.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    TaskListRowActionsTagHelper.TagName,
                    TaskListRowActionTagHelper.TagName);
            }

            ActionsAttributes = attributes;
        }

        public void SetKey(AttributeDictionary attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);
            Guard.ArgumentNotNull(nameof(content), content);

            if (Key != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TaskListRowKeyTagHelper.TagName,
                    TaskListRowTagHelper.TagName);
            }

            if (Value != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    TaskListRowKeyTagHelper.TagName,
                    TaskListRowValueTagHelper.TagName);
            }

            if (ActionsAttributes != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    TaskListRowKeyTagHelper.TagName,
                    TaskListRowActionsTagHelper.TagName);
            }

            if (_actions.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    TaskListRowKeyTagHelper.TagName,
                    TaskListRowActionTagHelper.TagName);
            }

            Key = (attributes, content);
        }

        public void SetValue(AttributeDictionary attributes, IHtmlContent content)
        {
            Guard.ArgumentNotNull(nameof(attributes), attributes);
            Guard.ArgumentNotNull(nameof(content), content);

            if (Value != null)
            {
                throw ExceptionHelper.OnlyOneElementIsPermittedIn(
                    TaskListRowValueTagHelper.TagName,
                    TaskListRowTagHelper.TagName);
            }

            if (ActionsAttributes != null)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    TaskListRowValueTagHelper.TagName,
                    TaskListRowActionsTagHelper.TagName);
            }

            if (_actions.Count > 0)
            {
                throw ExceptionHelper.ChildElementMustBeSpecifiedBefore(
                    TaskListRowValueTagHelper.TagName,
                    TaskListRowActionTagHelper.TagName);
            }

            Value = (attributes, content);
        }

        public void ThrowIfIncomplete()
        {
            if (Key == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TaskListRowKeyTagHelper.TagName);
            }

            if (Value == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TaskListRowValueTagHelper.TagName);
            }
        }
    }
}