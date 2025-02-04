using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public Product Product { get; set; } = new Product();

        public DetailsModel(ApplicationDbContext context)
        {
            this.context = context;
        }


        public void OnGet(int id)
        {
            var product = context.products.Find(id);

            if (product == null)
            {
                Response.Redirect("/Index");
                return;
            }

            Product = product;
        }
    }
}
