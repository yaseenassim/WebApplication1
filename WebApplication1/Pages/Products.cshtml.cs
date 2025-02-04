using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public List<Product> Products { get; set; } = new List<Product>();

        //Search functionality
        public class StoreSearchDto
        {
            public string? Search { get; set; }
            public string? Brand { get; set; }
            public string? Category { get; set; }
            public string? PriceRange { get; set; }
        }
        [BindProperty(SupportsGet = true)]
        public StoreSearchDto SearchDto { get; set; } = new StoreSearchDto();


        //sort
        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; } = "";


        //pagination
        public int pageIndex = 1;
        public int totalPages = 0;
        private readonly int pageSize = 6;

        public ProductsModel(ApplicationDbContext context)
        {
            this.context = context;
        }



        public void OnGet(int? pageIndex)
        {
            IQueryable<Product> query = context.products;

            //Search functionality
            if(SearchDto.Search != null)
            {
                query = query.Where(p => p.Name.Contains(SearchDto.Search) || p.Description.Contains(SearchDto.Search));
            }

            if(SearchDto.Brand != null)
            {
                query = query.Where(p => p.Brand.Contains(SearchDto.Brand));
            }

            if(SearchDto.Category != null)
            {
                query = query.Where(p => p.Category.Contains(SearchDto.Category));
            }

            if(SearchDto.PriceRange != null)
            {
                if(SearchDto.PriceRange == "0_100")
                {
                    query = query.Where(p => p.Price <= 100);
                }
                else if(SearchDto.PriceRange == "100_1000")
                {
                    query = query.Where(p => p.Price >= 100 && p.Price <= 1000);
                }
                else if(SearchDto.PriceRange == "1000_plus")
                {
                    query = query.Where(p => p.Price >= 1000);
                }
            }

            //Sort functionality
            if(SortBy == "price_asc")
            {
                query = query.OrderBy(p => p.Price);
            }
            else if(SortBy == "price_desc")
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                query = query.OrderByDescending((p) => p.Id);
            }

            //Pagination functionality
            if(pageIndex == null || pageIndex < 1)
            {
                pageIndex = 1;
            }
            this.pageIndex = (int)pageIndex;
            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / pageSize);
            query = query.Skip((this.pageIndex - 1) * pageSize).Take(pageSize);

            Products = query.ToList();
        }
    }
}
