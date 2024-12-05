namespace Camp6AssignmentDemo.ViewModel
{
    public class OrderCustomerViewModel
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
      
        public int OrderItem_Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }

        
    }
}
