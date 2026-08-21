using ThePensionsRegulator.GovUk.Frontend.HtmlGeneration;
using TaskStatus = ThePensionsRegulator.GovUk.Frontend.HtmlGeneration.TaskStatus;

namespace ThePensionsRegulator.GovUk.Frontend.TagHelpers
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