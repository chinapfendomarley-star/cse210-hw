namespace MyApp
{
    // Adds the writing title to the shared assignment data.
    public class WritingAssignment : Assignment
    {
        private readonly string _title;

        // Initialize shared data through Assignment and store the title here.
        public WritingAssignment(string studentName, string topic, string title)
            : base(studentName, topic)
        {
            _title = title;
        }

        // Include the inherited student name in the writing details.
        public string GetWritingInformation()
        {
            return $"{_title} by {GetStudentName()}";
        }
    }
}