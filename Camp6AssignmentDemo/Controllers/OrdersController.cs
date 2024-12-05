using Camp6AssignmentDemo.Model;
using Camp6AssignmentDemo.Repository;
using Camp6AssignmentDemo.ViewModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Camp6AssignmentDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        //Call repository
        private readonly IOrderRepository _repository;

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


        //#region 3-Get orders - search by id
        //[HttpGet("{id}")]
        ////http://localhost:7725/api/employees/vm
        //public async Task<ActionResult<OrderTable>> GetAllOrderById(int id)
        //{
        //    var orders = await _repository.GetOrderTableById(id);
        //    if (orders == null)
        //    {
        //        return NotFound("No orders found");
        //    }
        //    return Ok(orders);
        //}
        //#endregion

    }
}


