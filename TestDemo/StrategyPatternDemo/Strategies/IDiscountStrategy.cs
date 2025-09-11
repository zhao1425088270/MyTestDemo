using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDemo.StrategyPatternDemo.Models;

namespace TestDemo.StrategyPatternDemo.Strategies;

public interface IDiscountStrategy
{
    string StrategyName { get; }
    decimal CalculateDiscount(Order order);
    bool IsApplicable(Order order);
}
