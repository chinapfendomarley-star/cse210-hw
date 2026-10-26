using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter what is your grade percentage?  ");
        string answer = Console.ReadLine();
        int percentage = int.Parse(answer);

        string GradeLetter = "";

        if (percentage >= 90)
        {
            GradeLetter = "A";
        }
        else if (percentage >= 80)
        {
            GradeLetter = "B";
        }
        else if (percentage >= 70)
        {
            GradeLetter = "C";
        }
        else if (percentage >= 60)
        {
            GradeLetter = "D";
        }
        else
        {
            GradeLetter = "F";
        }

        // Determine the sign based on the last digit of the percentage.

        string sign = "";
        int lastDigit = percentage % 10;

        if (lastDigit >= 7)
        {
            sign = "+";
        }
        else if (lastDigit < 3)
        {
            sign = "-";
        }

        if (GradeLetter == "A" && sign == "+")
        {
            sign = "";
        }
        else if (GradeLetter == "F")
        {
            sign = "";
        }

        Console.WriteLine($"Your grade is: {GradeLetter}{sign}");

        if (percentage >= 70)
        {
            Console.WriteLine("Congratulations! You passed the class!");
        }
        else
        {
            Console.WriteLine("Sorry, better luck next time!");
        }
    }
}