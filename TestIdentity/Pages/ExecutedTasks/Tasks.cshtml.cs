using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestIdentity.Pages.ExecutedTasks
{
    [Authorize]
    public class TasksModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
