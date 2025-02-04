using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;

namespace WebApplication1.Pages.Client.Orders
{
    [Authorize(Roles = "client")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;
        public List<WebApplication1.Models.Order> orders = new List<WebApplication1.Models.Order>();

        //Pagination
        public int currentPage = 1;
        public int totalPages = 0;
        private readonly int pageSize = 5;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int indexPage)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if(currentUser == null)
            {
                return RedirectToPage("/index");
            }


            IQueryable<WebApplication1.Models.Order> query = context.Orders.OrderByDescending(o => o.Id)
                .Include(o => o.Client).Include(o => o.Items).Where(o => o.ClientId == currentUser.Id);

            //Pagination functionality
            if (indexPage < 1)
            {
                indexPage = 1;
            }

            currentPage = indexPage;

            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((indexPage - 1) * pageSize).Take(pageSize);

            orders = query.ToList();

            return Page();
        }
    }
}
