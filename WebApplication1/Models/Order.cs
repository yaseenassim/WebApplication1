﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = "";
        public IdentityUser Client { get; set; } = null;
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        [Precision(16 , 2)]
        public decimal ShippingFee { get; set; }

        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
        public string PaymentStatus { get; set; } = "";
        public string PaymentDetails { get; set; } = ""; //To store paypal details
        public string OrderStatus { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
