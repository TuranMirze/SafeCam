namespace SafeCam.Models
{
    public class Employee :BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public string CoverFile { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
