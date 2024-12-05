using Camp6AssignmentDemo.Model;
using Camp6AssignmentDemo.Repository;
using Camp6AssignmentDemo.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Camp6AssignmentDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        //Call repository
        private readonly IOrderRepository _repository;
        private readonly Camp6AssignmentDbContext _context;


        //Dependency Injection--Constructor Injection
        public OrdersController(IOrderRepository repository)
        {
            _repository = repository;
        }
        //// GET: api/<OrdersController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<OrdersController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        #region 1-Get all orders - search all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderTable>>> GetAllOrders()
        {
            var orders = await _repository.GetOrderTable();
            if (orders == null)
            {
                return NotFound("No orders found");
            }
            return Ok(orders);
        }
        #endregion

        #region 2-Get all orders - search all
        [HttpGet("vm")]

        public async Task<ActionResult<IEnumerable<OrderCustomerViewModel>>> GetAllOrdersByViewModel()
        {
            var orders = await _repository.GetViewModelOrders();
            if (orders == null)
            {
                return NotFound("No orders found");
            }
            return Ok(orders);
        }
        #endregion


        #region 3-Get orders - search by id
        [HttpGet("{id}")]
        //http://localhost:7725/api/employees/vm
        public async Task<ActionResult<OrderTable>> GetOrderCustomerById(int id)
        {
            var orders = await _repository.GetOrderById(id);
            if (orders == null)
            {
                return NotFound("No orders found");
            }
            return Ok(orders);
        }
        #endregion

        #region 4-Insert an Order - Return Order Record
        [HttpPost]
        public async Task<ActionResult<OrderTable>> InsertTblOrdersReturnRecord(OrderTable order)
        {
            if (ModelState.IsValid)
            {
                //Insert a new record and return as an object named employee

                var newOrder = await _repository.PostTblOrdersReturnRecord(order);

                if (newOrder != null)
                {
                    return Ok(newOrder);
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }
        #endregion

        #region 5- Insert Item to an Order
        [HttpPost("{orderId}/items")]
        public async Task<ActionResult<OrderItem>> AddItemToOrder(int orderId, [FromBody] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                var addedItem = await _repository.AddItemToOrder(orderId, orderItem);
                if (addedItem != null)
                {
                    return Ok(addedItem);
                }
                else
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }
            }
            return BadRequest();
        }
        #endregion

        #region 6- Insert/Add a Customer

        [HttpPost("/api/customers")]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (_context != null)
                {
                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();
                    return Ok(customer);
                }
            }
            return BadRequest();
        }
        #endregion

    }



}


