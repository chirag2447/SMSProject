namespace SMSProject.Models
{
    public class AssignmentModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public double percentComplete { get; set; }
    }
}