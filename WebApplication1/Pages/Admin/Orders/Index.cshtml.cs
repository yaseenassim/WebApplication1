using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using sib_api_v3_sdk.Model;
using WebApplication1.Services;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages.Admin.Orders
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<WebApplication1.Models.Order> orders = new List<WebApplication1.Models.Order>();

        //Pagination
        public int currentPage = 1;
        public int totalPages  = 0;
        private readonly int pageSize = 5;

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet(int indexPage)
        {
            IQueryable<WebApplication1.Models.Order> query = context.Orders.OrderByDescending(o => o.Id)
                .Include(o => o.Client).Include(o => o.Items);

            //Pagination functionality
            if(indexPage < 1)
            {
                indexPage = 1;
            }

            currentPage = indexPage;

            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((indexPage - 1) * pageSize).Take(pageSize);

            orders = query.ToList();
        }
    }
}
