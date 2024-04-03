namespace MinimalAPIs.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Address { get; set; }
        public int DepartmentId { get; set; }

        public bool IsValid()
        {
            return true;
        }
    }

    
}
