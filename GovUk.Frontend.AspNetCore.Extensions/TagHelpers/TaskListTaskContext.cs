using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TaskListTaskContext
    {
        public TaskName Name { get; set; } = new();
        public Hint? Hint { get; set; }
        public TaskStatus Status { get; set; } = new();

        public void ThrowIfIncomplete()
        {
            if (Name.Attributes == null)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TaskListTaskNameTagHelper.TagName);
            }
        }
    }
}