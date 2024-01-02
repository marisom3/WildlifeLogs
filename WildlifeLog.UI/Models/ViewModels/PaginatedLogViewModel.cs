using WildlifeLog.UI.Models.DTO;

namespace WildlifeLog.UI.Models.ViewModels
{
    public class PaginatedLogViewModel
    {
        public List<LogDto> Logs { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

}
