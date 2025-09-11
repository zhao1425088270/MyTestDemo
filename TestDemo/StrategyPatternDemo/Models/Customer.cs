using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo.StrategyPatternDemo.Models;

public enum CustomerType
{
    Regular,
    Vip,
    Premium
}

public class Customer
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public CustomerType Type { get; set; }

    public DateTime MemberSince { get; set; }

    public decimal TotalSpent { get; set; }
}
