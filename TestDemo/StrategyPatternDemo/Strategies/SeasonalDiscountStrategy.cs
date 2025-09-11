using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;

namespace TestDemo.StrategyPatternDemo.Strategies;

public class SeasonalDiscountStrategy : IDiscountStrategy
{
    public string StrategyName => "Seasonal Discount";

    public decimal CalculateDiscount(Order order)
    {
        if (!IsApplicable(order)) return 0;

        var currentMonth = DateTime.Now.Month;

        // 双11 (11月)
        if (currentMonth == 11)
        {
            return order.OriginalAmount * 0.25m; // 25%折扣
        }

        // 春节促销 (1-2月)
        if (currentMonth <= 2)
        {
            return order.OriginalAmount * 0.18m; // 18%折扣
        }

        // 夏季促销 (6-8月)
        if (currentMonth >= 6 && currentMonth <= 8)
        {
            return order.OriginalAmount * 0.12m; // 12%折扣
        }

        return 0;
    }

    public bool IsApplicable(Order order)
    {
        var currentMonth = DateTime.Now.Month;
        return (currentMonth == 11) ||
               (currentMonth <= 2) ||
               (currentMonth >= 6 && currentMonth <= 8);
    }
}
