namespace SushmaElectrical.ViewModels.Utility
{
    public class PageInfo
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPrevious => PageNo > 1;
        public bool HasNext => PageNo < TotalPages;
    }
}
