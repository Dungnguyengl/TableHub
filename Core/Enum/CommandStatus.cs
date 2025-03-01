using System.ComponentModel;
using System.Runtime.Serialization;

namespace Core.Enum
{
    public enum CommandStatus
    {
        [Description("Success")]
        [EnumMember(Value = "Success")]
        Success,

        [Description("Error")]
        [EnumMember(Value = "Error")]
        Error,

        [Description("NotFound")]
        [EnumMember(Value = "NotFound")]
        NotFound
    }
}
