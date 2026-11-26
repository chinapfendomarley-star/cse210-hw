namespace MyApp
{
    // Adds math-specific homework details to the shared assignment data.
    public class MathAssignment : Assignment
    {
        private readonly string _textbookSection;
        private readonly string _problems;

        // Initialize shared data through Assignment and store math details here.
        public MathAssignment(string studentName, string topic, string textbookSection, string problems)
            : base(studentName, topic)
        {
            _textbookSection = textbookSection;
            _problems = problems;
        }

        // Format the textbook section and assigned problem range.
        public string GetHomeworkList()
        {
            return $"{_textbookSection} Problems {_problems}";
        }
    }
}