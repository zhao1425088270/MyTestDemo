using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;

namespace TestDemo.StrategyPatternDemo.Strategies;

public class VipCustomerStrategy : IDiscountStrategy
{
    public string StrategyName => "VIP Customer Discount";

    public decimal CalculateDiscount(Order order)
    {
        if (!IsApplicable(order)) return 0;

        // VIP客户：15%折扣，满500额外减50
        var percentageDiscount = order.OriginalAmount * 0.15m;
        var bonusDiscount = order.OriginalAmount >= 500 ? 50 : 0;

        return percentageDiscount + bonusDiscount;
    }

    public bool IsApplicable(Order order)
    {
        return order.Customer.Type == CustomerType.Vip && order.OriginalAmount >= 50;
    }
}
