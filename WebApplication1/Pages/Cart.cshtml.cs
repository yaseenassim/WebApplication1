using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext context;

        [BindProperty]
        [Required(ErrorMessage = "Delivery address is required"), MaxLength(200)]
        public string DeliveryAddress { get; set; } = "";

        [BindProperty]
        public string PaymentMethod { get; set; } = "";


        public List<OrderItem> cartItems = new List<OrderItem>();
        public readonly decimal shippingFee;
        public decimal subTotal;
        public decimal total;

        public string errorMessage = "";

        public CartModel(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            shippingFee = configuration.GetValue<decimal>("CartSettings:ShippingFee");
        }
        public void OnGet()
        {
            cartItems = CartHelper.GetCartItems(Request, Response, context);
            subTotal = CartHelper.GetSubTotal(cartItems);
            total = subTotal + shippingFee;
        }

        public IActionResult OnPost()
        {
            cartItems = CartHelper.GetCartItems(Request, Response, context);
            subTotal = CartHelper.GetSubTotal(cartItems);
            total = subTotal + shippingFee;

            if(!ModelState.IsValid)
            {
                return Page();
            }

            if (cartItems.Count == 0)
            {
                errorMessage = "Your cart is empty";
                return Page();
            }

            TempData["DeliveryAddress"] = DeliveryAddress;
            TempData["PaymentMethod"] = PaymentMethod;

            return RedirectToPage("/Confirm");
        }
    }
}
