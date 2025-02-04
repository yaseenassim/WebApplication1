using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    [Authorize]
    public class ConfirmModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public string deliveryAddress { get; set; } = "";
        public string paymentMethod { get; set; } = "";

        public readonly decimal shippingFee;

        public decimal total;

        public int cartSize;

        public ConfirmModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.context = context;
            this.userManager = userManager;
            shippingFee = configuration.GetValue<decimal>("CartSettings:ShippingFee");
        }

        public string successMessage = "";



        public IActionResult OnGet()
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
            total = CartHelper.GetSubTotal(cartItems) + shippingFee;
            cartSize = 0;

            foreach(var item in cartItems)
            {
                cartSize += item.Quantity;
            }

            deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();

            if(cartSize == 0 || deliveryAddress.Length == 0 || paymentMethod.Length == 0)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }



        public async Task<IActionResult> OnPost()
        {
            List<OrderItem> cartItems = CartHelper.GetCartItems(Request, Response, context);
            total = CartHelper.GetSubTotal(cartItems) + shippingFee;
            cartSize = 0;

            foreach (var item in cartItems)
            {
                cartSize += item.Quantity;
            }

            deliveryAddress = TempData["DeliveryAddress"] as string ?? "";
            paymentMethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();

            if (cartSize == 0 || deliveryAddress.Length == 0 || paymentMethod.Length == 0)
            {
                return RedirectToPage("/Index");
            }

            var appUser = await userManager.GetUserAsync(User);

            if (appUser == null)
            {
                return RedirectToPage("/Index");
            }

            //Save the order
            var order = new Order
            {
                ClientId = appUser.Id,
                Items = cartItems,
                ShippingFee = shippingFee,
                DeliveryAddress = deliveryAddress,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Pending",
                PaymentDetails = "",
                OrderStatus = "Created",
                CreatedAt = DateTime.Now,
            };

            context.Orders.Add(order);
            context.SaveChanges();

            //Delete shopping cart cookie
            Response.Cookies.Delete("shopping_cart");

            successMessage = "Order created successfully";

            return Page();
        }


    }
}
