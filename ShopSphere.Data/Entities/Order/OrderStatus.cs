using System.Runtime.Serialization;

namespace ShopSphere.Data.Entities.Order
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending ,
        [EnumMember(Value = "Received")]
        Received ,
        [EnumMember(Value = "Failed")]
        Failed
    }
}