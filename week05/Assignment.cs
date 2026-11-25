namespace MyApp
{
    // Holds the information shared by every type of assignment.
    public class Assignment
    {
        private readonly string _studentName;
        private readonly string _topic;

        // Store the shared student and topic information.
        public Assignment(string studentName, string topic)
        {
            _studentName = studentName;
            _topic = topic;
        }

        // Return a summary using the shared assignment information.
        public string GetSummary()
        {
            return $"{_studentName} - {_topic}";
        }

        // Let derived classes use the student name without exposing the field.
        public string GetStudentName()
        {
            return _studentName;
        }
    }
}
