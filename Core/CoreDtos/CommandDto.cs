using Core.Enum;
using Core.Extentions;

namespace Core.CoreDtos
{
    public class CommandDto
    {
        public string Status { get; set; }
        public string Message { get; set; } = string.Empty;

        public static CommandDto NotFound<TEntity>(string? key = "") where TEntity : class
        {
            return NotFound(typeof(TEntity).Name, key);
        }

        public static CommandDto NotFound(string entityName = "", string? key = "")
        {
            return new CommandDto
            {
                Status = CommandStatus.NotFound.GetDescription(),
                Message = (entityName.IsNullOrEmpty() || key.IsNullOrEmpty()) ? "Not Found" : $"{entityName} has Key: {key} not found"
            };
        }

        public static CommandDto Success()
        {
            return new CommandDto
            {
                Status = CommandStatus.Success.GetDescription()
            };
        }
    }
}
