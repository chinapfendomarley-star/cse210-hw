using System;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Demonstrate the summary provided by the base Assignment class.
            Assignment assignment = new Assignment("Samuel Bennett", "Multiplication");
            System.Console.WriteLine(assignment.GetSummary());

            // MathAssignment inherits the summary and adds its homework list.
            MathAssignment mathAssignment = new MathAssignment(
                "Roberto Rodriguez",
                "Fractions",
                "Section 7.3",
                "8-19");
            System.Console.WriteLine(mathAssignment.GetSummary());
            System.Console.WriteLine(mathAssignment.GetHomeworkList());

            // WritingAssignment inherits the summary and adds writing details.
            WritingAssignment writingAssignment = new WritingAssignment(
                "Mary Waters",
                "European History",
                "The Causes of World War II");
            System.Console.WriteLine(writingAssignment.GetSummary());
            System.Console.WriteLine(writingAssignment.GetWritingInformation());
        }
    }
}