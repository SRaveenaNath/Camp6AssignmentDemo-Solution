using Camp6AssignmentDemo.Model;
using Camp6AssignmentDemo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Camp6AssignmentDemo.Repository
{
    public class OrderRepository : IOrderRepository
    {
        //We need to call virtual db--EF
        private readonly Camp6AssignmentDbContext _context; 
        private object order;

        //DI--Constructor injection
        public OrderRepository(Camp6AssignmentDbContext context)
        {
            _context = context;

        }

        #region 1- Get all Order --Search all
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetOrderTable()
        {
            try
            {
                if (_context != null)
                {
                    return await _context.OrderTables.Include(ord => ord.Customer ).Include(ord => ord.OrderItem).ToListAsync();
                }

                //Return an empty List if context is null
                return new List<OrderTable>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 2- Get all using view model
        public async Task<ActionResult<IEnumerable<OrderCustomerViewModel>>> GetViewModelOrders()
        {
            //LINQ
            try
            {
                if (_context != null)
                {


                    return await (from o in _context.OrderTables
                                  join c in _context.Customers on o.CustomerId equals c.CustomerId
                                  join oi in _context.OrderItems on o.OrderItemId equals oi.OrderItemId
                                  join i in _context.Items on oi.ItemId equals i.ItemId
                                  select new OrderCustomerViewModel
                                  {
                                      CustomerId = c.CustomerId,
                                      CustomerName = c.CustomerName,
                                      ItemName = i.ItemName,
                                      Price = i.Price,
                                      Quantity = oi.Quantity,
                                      OrderDate = o.OrderDate
                                  }).ToListAsync();


                }

                //Return an empty List if context is null
                return new List<OrderCustomerViewModel>();

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region -3- Search By id
        public async Task<ActionResult<OrderTable>> GetOrderById(int id)
        {
            try
            {
                if (_context != null)
                {
                    //Find the order by ID
                    var order = await _context.OrderTables
                        .Include(ord => ord.Customer)
                        .Include(ord => ord.OrderItem).FirstOrDefaultAsync(o => o.OrderId == id);
                    return order;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion
        #region 4-Insert an Order - Return Order Record
        public async Task<ActionResult<OrderTable>> PostTblOrdersReturnRecord(OrderTable order)
        {
            try
            {
                // Check if the order object is not null
                if (order == null)
                {
                    throw new ArgumentNullException(nameof(order), "Order data is null");
                }

                // Ensure the database context is not null
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized");
                }

                // Add the order record to DbContext
                await _context.OrderTables.AddAsync(order);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Retrieve the order with related customer details (adjust relationships as necessary)
                var orderWithCustomer = await _context.OrderTables
                    .Include(o => o.Customer) // Assuming 'Customer' is a navigation property
                    .FirstOrDefaultAsync(o => o.OrderId == order.OrderId); // Assuming 'OrderId' is the unique identifier

                // Return the added order record
                return orderWithCustomer;
            }
            catch (Exception ex)
            {
                // Log the exception if necessary (e.g., using a logging framework)
                return null;
            }
        }
        #endregion

        #region 5- Add Item to an Order
        public async Task<ActionResult<OrderItem>> AddItemToOrder(int orderId, OrderItem orderItem)
        {
            try
            {
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialized");
                }

                // Validate the order exists
                var order = await _context.OrderTables.FindAsync(orderId);
                if (order == null)
                {
                    return null; // Order not found
                }

                // Add the new order item to the database
                await _context.OrderItems.AddAsync(orderItem);

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the added order item
                return orderItem;
            }
            catch (Exception ex)
            {
                // Log exception as necessary
                return null;
            }
        }
        #endregion



        #region 6- Update Customer

        public async Task<bool> UpdateCustomerAsync(int customerId, Customer updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                return false;

            customer.CustomerName = updatedCustomer.CustomerName;
            customer.CustomerNumber = updatedCustomer.CustomerNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region 7 Update Order

        public async Task<bool> UpdateOrderAsync(int orderId, OrderTable updatedOrder)
        {
            var order = await _context.OrderTables.FindAsync(orderId);
            if (order == null)
                return false;

            order.OrderDate = updatedOrder.OrderDate;
            order.CustomerId = updatedOrder.CustomerId;

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region 8 Update Order Item
        public async Task<bool> UpdateOrderItemAsync(int orderId, int orderItemId, OrderItem updatedItem)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId && oi.OrderTableId == orderId);
            if (orderItem == null)
                return false;

            orderItem.Quantity = updatedItem.Quantity;
            orderItem.Price = updatedItem.Price;
            orderItem.ItemId = updatedItem.ItemId;

            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region 9  Delete Customer
        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region 10 Delete Order

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.OrderTables.FindAsync(orderId);
            if (order == null)
                return false;

            _context.OrderTables.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region 11 Delete Order Item
        public async Task<bool> DeleteOrderItemAsync(int orderId, int orderItemId)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId && oi.OrderTableId == orderId);
            if (orderItem == null)
                return false;

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

    }
}
