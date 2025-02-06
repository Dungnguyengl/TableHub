namespace Core.CoreDtos
{
    public class PaggingDto
    {
        public int Sequence { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Skip { get => (Sequence - 1) * PageSize; }
        public int Take { get => PageSize; }
    }
}
