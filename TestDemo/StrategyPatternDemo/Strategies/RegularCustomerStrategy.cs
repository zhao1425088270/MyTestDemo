using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;

namespace TestDemo.StrategyPatternDemo.Strategies;

public class RegularCustomerStrategy : IDiscountStrategy
{
    public string StrategyName => "Regular Customer Discount";

    public decimal CalculateDiscount(Order order)
    {
        if (!IsApplicable(order)) return 0;

        // 普通客户：满100减10
        var discount = Math.Floor(order.OriginalAmount / 100) * 10;
        return Math.Min(discount, order.OriginalAmount * 0.1m); // 最多10%折扣
    }

    public bool IsApplicable(Order order)
    {
        return order.Customer.Type == CustomerType.Regular && order.OriginalAmount >= 100;
    }
}
