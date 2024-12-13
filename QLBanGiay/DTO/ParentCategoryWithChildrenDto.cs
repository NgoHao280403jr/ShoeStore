namespace QLBanGiay.DTO
{
    public class ParentCategoryWithChildrenDto
    {
        public long ParentId { get; set; }
        public string ParentName { get; set; }
        public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    }
}
