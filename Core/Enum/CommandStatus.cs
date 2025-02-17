using System.ComponentModel;

namespace Core.Enum
{
    public enum CommandStatus
    {
        [Description("Success")]
        Success,

        [Description("Error")]
        Error,

        [Description("NotFound")]
        NotFound
    }
}
