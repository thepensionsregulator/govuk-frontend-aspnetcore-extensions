﻿using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TaskListContext
    {
        private readonly List<TaskListSection> _sections;

        public (AttributeDictionary Attributes, HtmlString? Content) Name { get; internal set; }

        public TaskListContext()
        {
            _sections = new List<TaskListSection>();
        }

        public IReadOnlyList<TaskListSection> Sections => _sections;

        public void AddSection(TaskListSection section)
        {
            Guard.ArgumentNotNull(nameof(section), section);

            _sections.Add(section);
        }

        public void ThrowIfIncomplete()
        {
            if (Sections.Count < 1)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TaskListSectionTagHelper.TagName);
            }
        }
    }   
}