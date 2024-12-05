using Camp6AssignmentDemo.Model;
using Camp6AssignmentDemo.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Camp6AssignmentDemo.Repository
{
    public interface IOrderRepository
    {
        #region 1- Get all orders from DB-Search All

        public Task<ActionResult<IEnumerable<OrderTable>>> GetOrderTable();

        #endregion

        #region 2- Get All Order using ViewModel

        public Task<ActionResult<IEnumerable<OrderCustomerViewModel>>> GetViewModelOrders();

        #endregion
        #region 3- Get an Order based on Id
        public Task<ActionResult<OrderTable>> GetOrderById(int id);

        #endregion


        #region 4-Insert an Order - Return Order Record

        public Task<ActionResult<OrderTable>> PostTblOrdersReturnRecord(OrderTable orderTable);

        #endregion

        #region 5-Add Item to an Order
        Task<ActionResult<OrderItem>> AddItemToOrder(int orderId, OrderItem orderItem);
        #endregion



        //#region 5-Insert an Order - Return ID

        //public Task<ActionResult<int>> PostTblOrdersReturnId(OrderTable orderTable);

        //#endregion
    }
}
