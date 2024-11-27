using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjWankibackend.Models.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjWankibackend.Controllers.shop
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    //[EnableCors("AllowAnyOrigin")] // ±Ò¥Î CORS

    public class OrderController : ControllerBase
    {
        private readonly WealthierAndKinderContext _context;

        public OrderController(WealthierAndKinderContext context)
        {
            _context = context;
        }

        // GET: api/OrderDetails
        [HttpGet("OrderDetails")]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetails()
        {
            return await _context.TOrderDetails
                .Select(od => new OrderDetailDto
                {
                    FOrderDetailId = od.FOrderDetailId,
                    FOrderId = od.FOrderId,
                    FProductId = od.FProductId,
                    FAmount = od.FAmount,
                    FHelpPoint = od.FHelpPoint
                })
                .ToListAsync();
        }

        // GET: api/Orders
        [HttpGet("Orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            return await _context.TOrders
                .Select(o => new OrderDto
                {
                    FOrderId = o.FOrderId,
                    fPersonSId = o.FPersonSid,
                    FTotalHelpPoint = o.FTotalHelpPoint,
                    FStatus = o.FStatus,
                    FOrderTime = o.FOrderTime,
                    FExecStatus = o.FExecStatus,
                    FBeginTime = o.FBeginTime,
                    FFinishTime = o.FFinishTime,
                    FProof = o.FProof
                })
                .ToListAsync();
        }

        // POST: api/OrderDetails
        [HttpPost("OrderDetails")]
        [EnableCors("AllowAngular")] // ±Ò¥Î CORS
        public async Task<ActionResult<OrderDetailDto>> PostOrderDetail(OrderDetailDto orderDetailDto)
        {

            var orderDetail = new TOrderDetail
            {
                FOrderId = orderDetailDto.FOrderId,
                FProductId = orderDetailDto.FProductId,
                FAmount = orderDetailDto.FAmount,
                FHelpPoint = orderDetailDto.FHelpPoint
            };

            _context.TOrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

            orderDetailDto.FOrderDetailId = orderDetail.FOrderDetailId;

            return CreatedAtAction(nameof(GetOrderDetails), new { id = orderDetail.FOrderDetailId }, orderDetailDto);
        }

        // POST: api/Orders
        [HttpPost("Orders")]
        public async Task<ActionResult<OrderDto>> PostOrder(OrderDto orderDto)
        {
            var order = new TOrder
            {
                FPersonSid = orderDto.fPersonSId,
                FTotalHelpPoint = orderDto.FTotalHelpPoint,
                FStatus = orderDto.FStatus,
                FOrderTime = orderDto.FOrderTime,
                FExecStatus = orderDto.FExecStatus,
                FBeginTime = orderDto.FBeginTime,
                FFinishTime = orderDto.FFinishTime,
                FProof = orderDto.FProof
            };

            _context.TOrders.Add(order);
            await _context.SaveChangesAsync();

            orderDto.FOrderId = order.FOrderId;

            return CreatedAtAction(nameof(GetOrders), new { id = order.FOrderId }, orderDto);
        }

        // PUT: api/OrderDetails/5
        [HttpPut("OrderDetails/{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetailDto orderDetailDto)
        {
            if (id != orderDetailDto.FOrderDetailId)
            {
                return BadRequest();
            }

            var orderDetail = await _context.TOrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            orderDetail.FOrderId = orderDetailDto.FOrderId;
            orderDetail.FProductId = orderDetailDto.FProductId;
            orderDetail.FAmount = orderDetailDto.FAmount;
            orderDetail.FHelpPoint = orderDetailDto.FHelpPoint;

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Orders/5
        [HttpPut("Orders/{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderDto orderDto)
        {
            if (id != orderDto.FOrderId)
            {
                return BadRequest();
            }

            var order = await _context.TOrders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.FPersonSid = orderDto.fPersonSId;
            order.FTotalHelpPoint = orderDto.FTotalHelpPoint;
            order.FStatus = orderDto.FStatus;
            order.FOrderTime = orderDto.FOrderTime;
            order.FExecStatus = orderDto.FExecStatus;
            order.FBeginTime = orderDto.FBeginTime;
            order.FFinishTime = orderDto.FFinishTime;
            order.FProof = orderDto.FProof;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/OrderDetails/5
        [HttpDelete("OrderDetails/{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _context.TOrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.TOrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("Orders/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.TOrders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.TOrders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderDetailExists(int id)
        {
            return _context.TOrderDetails.Any(e => e.FOrderDetailId == id);
        }

        private bool OrderExists(int id)
        {
            return _context.TOrders.Any(e => e.FOrderId == id);
        }

        // GET: api/OrderDetails/ByOrderId/1
        [HttpGet("OrderDetails/ByOrderId/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderDetailDto>>> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _context.TOrderDetails
                .Where(od => od.FOrderId == orderId)
                .Select(od => new OrderDetailDto
                {
                    FOrderDetailId = od.FOrderDetailId,
                    FOrderId = od.FOrderId,
                    FProductId = od.FProductId,
                    FAmount = od.FAmount,
                    FHelpPoint = od.FHelpPoint
                })
                .ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return orderDetails;
        }
    }
}
