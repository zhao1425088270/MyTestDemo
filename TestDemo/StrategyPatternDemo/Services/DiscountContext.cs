using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;
using TestDemo.StrategyPatternDemo.Strategies;

namespace TestDemo.StrategyPatternDemo.Services;

public class DiscountContext
{
    private readonly IEnumerable<IDiscountStrategy> _strategies;
    private readonly ILogger<DiscountContext> _logger;

    public DiscountContext(IEnumerable<IDiscountStrategy> strategies, ILogger<DiscountContext> logger)
    {
        _strategies = strategies;
        _logger = logger;
    }

    public void ApplyBestDiscount(Order order)
    {
        var applicableStrategies = _strategies.Where(s => s.IsApplicable(order)).ToList();

        if (!applicableStrategies.Any())
        {
            _logger.LogInformation("No applicable discount strategies for order {OrderId}", order.Id);
            return;
        }

        // 选择折扣最大的策略
        var bestStrategy = applicableStrategies
            .Select(strategy => new
            {
                Strategy = strategy,
                Discount = strategy.CalculateDiscount(order)
            })
            .OrderByDescending(x => x.Discount)
            .First();

        order.DiscountAmount = bestStrategy.Discount;
        order.AppliedStrategy = bestStrategy.Strategy.StrategyName;

        _logger.LogInformation(
            "Applied strategy '{Strategy}' to order {OrderId}. Discount: {Discount:C}",
            bestStrategy.Strategy.StrategyName,
            order.Id,
            bestStrategy.Discount);
    }

    public List<DiscountOption> GetAllApplicableDiscounts(Order order)
    {
        return _strategies
            .Where(s => s.IsApplicable(order))
            .Select(s => new DiscountOption
            {
                StrategyName = s.StrategyName,
                DiscountAmount = s.CalculateDiscount(order)
            })
            .OrderByDescending(x => x.DiscountAmount)
            .ToList();
    }
}

public class DiscountOption
{
    public string StrategyName { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
}
