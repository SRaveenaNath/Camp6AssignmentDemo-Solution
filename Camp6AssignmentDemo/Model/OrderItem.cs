using System;
using System.Collections.Generic;

namespace Camp6AssignmentDemo.Model;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitePrice { get; set; }

    public int? ItemId { get; set; }

    public virtual Item? Item { get; set; }
    //[System.Text.Json.Serialization.JsonIgnore]  //To avoid circular reference
    public virtual ICollection<OrderTable> OrderTables { get; set; } = new List<OrderTable>();
}
