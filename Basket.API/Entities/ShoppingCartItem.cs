using System;

namespace Basket.API.Entities
{
    public class ShoppingCartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTimeOffset AddedAt { get; set; } = DateTimeOffset.Now;
    }
}