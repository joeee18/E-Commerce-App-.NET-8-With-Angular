using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entites.Order;
using ECom.Core.Entites.Product;
using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Infrasructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrasructure.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;


        public OrderService(IUnitOfWork unitOfWork, AppDbContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Orders> CreateOrdersAsync(OrderDTO orderDTO, string buyerEmail)
        {
            //1. Get Basket From basket repo
            var basket = await _unitOfWork.CustomerBasket.GetBasketAsync(orderDTO.basketId);
            //2. Get Selected Items at Basket from product repo and add it to orderitems 
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.basketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                var orderItem = new OrderItem(product.Id,item.Image,product.Name,item.Price,item.Quantity);
                orderItems.Add(orderItem);
            }

            //3. Get DeliveryMethod from DeliveryMethod repo
            var deliveryMethod = await _context.DeliveryMethods.FirstOrDefaultAsync(m=>m.Id==orderDTO.deliveryMethodID);

            //4 Calculate Suptotal
            var supTotal = orderItems.Sum(m => m.Price * m.Quntity);
            

            ////5. Create Order 
            var ship = _mapper.Map<ShippingAddress>(orderDTO.shipAddress);
            var order = new Orders(buyerEmail,supTotal,ship,deliveryMethod,orderItems);
            //order.PaymentIntentId = "pending"; // Set default value to avoid null
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await _unitOfWork.CustomerBasket.DeleteBasketAsync(orderDTO.basketId);
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync() 
            => await _context.DeliveryMethods.AsNoTracking().ToListAsync();

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var order = await _context.Orders.Where(m => m.Id == id && m.BuyerEmail == buyerEmail)
                                                .Include(o => o.OrderItems)
                                                .Include(o => o.deliveryMethod)
                                                .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var orders = await _context.Orders.Where(m => m.BuyerEmail == buyerEmail)
                                                .Include(o => o.OrderItems)
                                                .Include(o => o.deliveryMethod)
                                                .ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
            return result;
        }
    }
}

