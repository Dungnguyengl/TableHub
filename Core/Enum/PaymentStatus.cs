using System.ComponentModel;

namespace Core.Enum
{
    public enum PaymentStatus
    {
        [Description("PAID")]
        Paid,

        [Description("PENDING")]
        Pending,

        [Description("PROCESSING")]
        Processing,

        [Description("CANCELLED")]
        Canceled,
    }
}
