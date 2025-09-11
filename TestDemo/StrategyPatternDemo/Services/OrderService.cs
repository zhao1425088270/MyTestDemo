using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;

namespace TestDemo.StrategyPatternDemo.Services;

public class OrderService
{
    private readonly DiscountContext _discountContext;
    private readonly ILogger<OrderService> _logger;

    public OrderService(DiscountContext discountContext, ILogger<OrderService> logger)
    {
        _discountContext = discountContext;
        _logger = logger;
    }

    public async Task<Order> ProcessOrderAsync(Order order)
    {
        _logger.LogInformation("Processing order {OrderId} for customer {CustomerName}",
            order.Id, order.Customer.Name);

        // 应用最佳折扣策略
        _discountContext.ApplyBestDiscount(order);

        // 这里可以添加其他业务逻辑，如库存检查、支付处理等
        await Task.Delay(100); // 模拟异步操作

        _logger.LogInformation(
            "Order {OrderId} processed. Original: {Original:C}, Discount: {Discount:C}, Final: {Final:C}",
            order.Id, order.OriginalAmount, order.DiscountAmount, order.FinalAmount);

        return order;
    }

    public List<DiscountOption> GetAvailableDiscounts(Order order)
    {
        return _discountContext.GetAllApplicableDiscounts(order);
    }
}
