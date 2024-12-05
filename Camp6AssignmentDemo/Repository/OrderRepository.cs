using Camp6AssignmentDemo.Model;
using Camp6AssignmentDemo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Camp6AssignmentDemo.Repository
{
    public class OrderRepository : IOrderRepository
    {
        //We need to call virtual db--EF
        private readonly Camp6AssignmentDbContext _context; //_context for accessing variables in DemoAugust2024DbContext ie TblDept and TblEmployee
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
                    return await _context.OrderTables.Include(ord => ord.CustomerId).Include(ord => ord.OrderItemId).ToListAsync();
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
        public async Task<ActionResult<OrderTable>> GetOrderTableById(int id)
        {
            try
            {
                if (_context != null)
                {
                    //Find the order by ID
                    var order = await _context.OrderTables
                        .Include(ord => ord.Customer)
                        .FirstOrDefaultAsync(ord => ord.CustomerId== id);
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
        #region 4-Insert an Employee - Return Employee Record
        public async Task<ActionResult<OrderTable>> PostTblOrdersReturnRecord(OrderTable orderTable)
        {
            try
            {

                //check if employee object is not null

                if (order == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(order), "Order data is null");

                }

                //Ensure the context is not null
                if (_context == null)
                {
                    throw new InvalidOperationException("Database context is not initialised");

                }
                //Add the employee record to DbContext
                await _context.OrderTables.AddAsync((OrderTable)order);

                //Save changes to db
                await _context.SaveChangesAsync();

                //Retrieve the employee with related Department
                var orderwithcustomer = await _context.OrderTables
                    .Include(ord => ord.Customer) //Eager load
                    .FirstOrDefaultAsync(or => or.OrderId == order.OrderId);

                //Return the added employee record
                return orderwithcustomer;


            }
            catch (Exception ex)
            {
                //Log exception here if needed
                return null;
            }
        }

        #endregion

        //#region 5-Insert an Employee - Return ID
        //public async Task<ActionResult<int>> PostTblEmployeesReturnId(TblEmployee tblEmployee)
        //{
        //    try
        //    {

        //        //check if employee object is not null

        //        if (_context != null)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(employee), "Employee data is null");
        //            //return  null

        //        }

        //        //Ensure the context is not null
        //        if (_context == null)
        //        {
        //            throw new InvalidOperationException("Database context is not initialised");

        //        }
        //        //Add the employee record to DbContext
        //        await _context.TblEmployees.AddAsync(employee);

        //        //Save changes to db
        //        var changesRecord = await _context.SaveChangesAsync();


        //        //Return the added employee record
        //        if (changesRecord > 0)
        //        {
        //            //Return the added employee ID
        //            return employee.EmployeeId;
        //        }
        //        else
        //        {
        //            throw new Exception("Failed to save employee record to the database.");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        //Log exception here if needed
        //        return null;
        //    }
        //}
        //#endregion
        

    }
}
