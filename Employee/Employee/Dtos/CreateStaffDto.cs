namespace Employee.Dtos
{
    public record CreateStaffDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
