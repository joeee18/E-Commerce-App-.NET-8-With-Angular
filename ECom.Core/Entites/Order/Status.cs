using System.Runtime.Serialization;

namespace ECom.Core.Entites.Order
{
    public enum Status
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived,
        [EnumMember(Value = "PaymentFaild")]
        PaymentFaild
    }
}