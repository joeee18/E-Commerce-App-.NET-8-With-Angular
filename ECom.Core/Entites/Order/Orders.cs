using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.Entites.Order
{
    public class Orders:BaseEntity<int>
    {
        private ShippingAddress ship;
        private DeliveryMethod? deliveryMethod1;
        private List<OrderItem> orderItems;

        public Orders() { }

        public Orders(string buyerEmail,
                      decimal supTotal,
                      ShippingAddress shippingAddress,
                      DeliveryMethod deliveryMethod,
                      IReadOnlyList<OrderItem> orderItems
                      
                      )
        {
            BuyerEmail = buyerEmail;
            SupTotal = supTotal;
            this.shippingAddress = shippingAddress;
            this.deliveryMethod = deliveryMethod;
            OrderItems = orderItems;
        }

        public string BuyerEmail { get; set; }
        public decimal SupTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public ShippingAddress shippingAddress { get; set; }
        public DeliveryMethod deliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public Status status { get; set; } = Status.Pending;

        public decimal GetTotal()
        {
            return SupTotal + deliveryMethod.Price;
        }


    }
}
