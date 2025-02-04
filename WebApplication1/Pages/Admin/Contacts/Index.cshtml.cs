using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Contacts
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<Contact> ContactList { get; set; } = new List<Contact>();

        //Pagination functionality
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 5;

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        } 
        public void OnGet(int? pageIndex)
        {
            IQueryable<Contact> query = context.Contacts
                .OrderByDescending(c => c.Id)
                .Include(c => c.Attachments);

            //Pagination functionality
            if(pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }
            this.pageIndex = (int)pageIndex;

            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);

            query = query.Skip((this.pageIndex - 1) * pageSize).Take(pageSize);

            ContactList = query.ToList();
        }
    }
}
