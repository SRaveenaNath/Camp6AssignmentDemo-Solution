using System;
using System.Collections.Generic;

namespace Camp6AssignmentDemo.Model;

public partial class Item
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public decimal? Price { get; set; }
    //[System.Text.Json.Serialization.JsonIgnore]  //To avoid circular reference
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
