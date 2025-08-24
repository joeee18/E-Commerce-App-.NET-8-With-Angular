using ECom.Core.DTO;
using ECom.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Services
{
    public interface IOrderService
    {
        Task<Orders> CreateOrdersAsync(OrderDTO orderDTO, string buyerEmail);
        Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string buyerEmail);
        Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

    }
}
