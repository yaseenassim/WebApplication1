using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Products
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 5;

        //search functionality
        public string search = "";

        //sort functionality
        public string column = "Id";
        public string orderBy = "desc";

        public List<Product> Products { get; set; } = new List<Product>();

        public IndexModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void OnGet(int? pageIndex, string? search, string? column, string? orderBy)
        {
            IQueryable<Product> query = context.products;

            //search functionality
            if(search != null)
            {
                this.search = search;
                query = query.Where(p => p.Name.Contains(search) || p.Brand.Contains(search));
            }

            //sort functionality
            string[] validColumns = { "Id", "Name", "Brand", "Category", "Price", "CreatedAt" };
            string[] validOrderBy = { "desc", "asc" };

            if (!validColumns.Contains(column))
            {
                column = "Id";
            }
            if (!validOrderBy.Contains(orderBy))
            {
                orderBy = "desc";
            }

            this.column = column;
            this.orderBy = orderBy;

            if( column == "Name")
            {
                if(orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Name);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Name);
                }
            }

            else if(column == "Brand")
            {
                if(orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Brand);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Brand);
                }
            }

            else if(column == "Category")
            {
                if(orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Category);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Category);
                }
            }

            else if(column == "Price")
            {
                if(orderBy == "asc")
                {
                    query = query.OrderBy(p => p.Price);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Price);
                }
            }

            else if(column == "CreatedAt")
            {
                if(orderBy == "asc")
                {
                    query = query = query.OrderBy(p => p.CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(p => p.CreatedAt);
                }
            }

            else
            {
                if(orderBy == "asc")
                {
                    query = query.OrderBy(P => P.Id);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Id);
                }
            }

            

            //query = query.OrderByDescending(p => p.Id);


            //pagination functionality
            if(pageIndex == null ||  pageIndex < 1)
            {
                pageIndex = 1;
            }
            this.pageIndex = (int) pageIndex;

            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((this.pageIndex - 1) * pageSize).Take(pageSize);
                

            Products = query.ToList();    
        }
    }
}
