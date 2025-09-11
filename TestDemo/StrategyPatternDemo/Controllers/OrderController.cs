using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;
using TestDemo.StrategyPatternDemo.Services;

namespace TestDemo.StrategyPatternDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// 处理订单并应用最佳折扣
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    [HttpPost("process")]
    public async Task<ActionResult<Order>> ProcessOrder([FromBody] Order order)
    {
        if (order?.Customer == null || !order.Items.Any())
        {
            return BadRequest("Invalid order data");
        }

        order.Id = new Random().Next(1000, 9999);
        order.OrderDate = DateTime.Now;

        var processedOrder = await _orderService.ProcessOrderAsync(order);
        return Ok(processedOrder);
    }

    /// <summary>
    /// 获取所有适用的折扣策略
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    [HttpPost("discounts")]
    public ActionResult<List<DiscountOption>> GetAvailableDiscounts([FromBody] Order order)
    {
        if (order?.Customer == null || !order.Items.Any())
        {
            return BadRequest("Invalid order data");
        }

        var discounts = _orderService.GetAvailableDiscounts(order);
        return Ok(discounts);
    }

    /// <summary>
    /// 获取示例数据
    /// </summary>
    /// <returns></returns>
    [HttpGet("sample-data")]
    public ActionResult<object> GetSampleData()
    {
        return Ok(new
        {
            Customers = new[]
            {
                    new { Type = 0, Name = "Regular" },
                    new { Type = 1, Name = "VIP" },
                    new { Type = 2, Name = "Premium" }
                },
            SampleOrder = new
            {
                Customer = new
                {
                    Id = 1,
                    Name = "张三",
                    Type = 1, // VIP
                    MemberSince = "2022-01-01T00:00:00",
                    TotalSpent = 5000
                },
                Items = new[]
                {
                        new { ProductName = "iPhone 15", Price = 5999, Quantity = 1, Category = "Electronics" },
                        new { ProductName = "AirPods", Price = 1299, Quantity = 1, Category = "Electronics" }
                    }
            }
        });
    }
}
