using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;

namespace TestDemo.StrategyPatternDemo.Strategies;

public class PremiumCustomerStrategy : IDiscountStrategy
{
    public string StrategyName => "Premium Customer Discount";

    public decimal CalculateDiscount(Order order)
    {
        if (!IsApplicable(order)) return 0;

        // 高级客户：20%折扣 + 忠诚度奖励
        var baseDiscount = order.OriginalAmount * 0.20m;

        // 会员年限奖励
        var membershipYears = (DateTime.Now - order.Customer.MemberSince).Days / 365;
        var loyaltyBonus = Math.Min(membershipYears * 10, 100); // 每年10元，最多100元

        return baseDiscount + loyaltyBonus;
    }

    public bool IsApplicable(Order order)
    {
        return order.Customer.Type == CustomerType.Premium;
    }
}
