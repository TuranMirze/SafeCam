namespace SafeCam.ViewModel.Employee
{
    public class EmployeeCreateVM
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
        public IFormFile CoverFile { get; set; }
    }
}
