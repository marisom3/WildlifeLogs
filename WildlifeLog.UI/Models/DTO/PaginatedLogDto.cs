

namespace WildlifeLog.UI.Models.DTO
{
    public class PaginatedLogDto
    {
        public List<LogDto> Logs { get; set; }
		public PaginationInfo PaginationInfo { get; set; }
    }
}
