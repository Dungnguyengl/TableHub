namespace Core.CoreDtos
{
    public class PaggingResultDto<TEnity> where TEnity : class
    {
        public IEnumerable<TEnity> Results { get; set; }
        public int Sequence { get; set; }
        public int Total { get; set; }
        public int PageSize { get; set; }
    }
}
