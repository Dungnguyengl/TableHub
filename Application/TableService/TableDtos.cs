using Core.CoreDtos;
using Core.Enum;

namespace Application.TableService
{
    public class SearchTableQuery : PaggingDto
    {
    }

    public class SearchTableDto
    {
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

    public class DeleteTableCommand
    {

    }
}
