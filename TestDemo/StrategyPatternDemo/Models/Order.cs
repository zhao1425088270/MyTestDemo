using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo.StrategyPatternDemo.Models;

public class Order
{
    public int Id { get; set; }
    public Customer Customer { get; set; } = null!;
    public List<OrderItem> Items { get; set; } = new();
    public DateTime OrderDate { get; set; }
    public decimal OriginalAmount => Items.Sum(x => x.Price * x.Quantity);
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount => OriginalAmount - DiscountAmount;
    public string? AppliedStrategy { get; set; }
}

public class OrderItem
{
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; } = string.Empty;
}
