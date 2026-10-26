using System;

class Program
{
        static void Main(string[] args)
    {
        DisplayWelcomeMessage();

        string userName = PromptUserName();
        int userNumber = PromptUserNumber();

        int squaredNumber = SquareNumber(userNumber);

        DisplayResult(userName, squaredNumber);
    }

    // Displays a welcome message to the user.
    static void DisplayWelcomeMessage()
    {
        Console.WriteLine("Welcome to the program!");
    }

    // Prompts the user for their name and returns it.
    static string PromptUserName()
    {
        Console.Write("Please enter your name: ");
        string name = Console.ReadLine();

        return name;
    }

    // Prompts the user for their favorite number and returns it.
    static int PromptUserNumber()
    {
        Console.Write("Please enter your favorite number: ");
        int number = int.Parse(Console.ReadLine());

        return number;
    }

    // Returns the square of the given number.
    static int SquareNumber(int number)
    {
        int square = number * number;
        return square;
    }

    // Displays the user's name and the square of their favorite number.
    static void DisplayResult(string name, int square)
    {
        Console.WriteLine($"{name}, the square of your favorite number is {square}");
    }
}