namespace Questao2
{
    public class ApiResponse<T>
    {
        public int Page { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public int Total_pages { get; set; }
        public T Data { get; set; }
    }
}
