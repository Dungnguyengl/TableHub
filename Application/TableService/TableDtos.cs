using Core.CoreDtos;
using Core.Enum;

namespace Application.TableService
{
    public class SearchTableQuery : PaggingDto
    {
        public Guid? StoreId { get; set; }
    }


    public class SearchTableDto
    {
        public Guid? TableId { get; set; }
        public string? TableName { get; set; }
        public TableStatus? Status { get; set; }
    }

    public class CreateTableCommand
    {

    }

    public class CreateTableDto
    {

    }

    public class UpdateTableCommand
    {

    }

    public class UpdateTableDto
    {

    }

    public class ChangeStatusCommand
    {
        public Guid? TableId { get; set; }
        public TableStatus? Status { get; set; }
    }

    public class ChangeStatusDto
    {

    }

    public class DeleteTableCommand
    {

    }
}
