namespace PlantWebApps.Models
{
    public class UserViewModel
    {
        public string Data { get; set; }
        public PaginationInfo Pagination { get; set; }
    }

    public class PaginationInfo
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
