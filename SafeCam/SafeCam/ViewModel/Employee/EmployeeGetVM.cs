namespace SafeCam.ViewModel.Employee
{
    public class EmployeeGetVM
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public IFormFile CoverFile { get; set; }
    }
}
