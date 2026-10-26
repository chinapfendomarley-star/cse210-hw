using System;

class Program
{
    static void Main(string[] args)
    {
        Random randomGenerator = new Random();
        string playAgain;

        do
        {
            int magicNumber = randomGenerator.Next(1, 101);
            int guess = -1;
            int guessCount = 0;

            while (guess != magicNumber)
            {
                //user guess and guess count
                Console.Write("What is your guess? ");
                guess = int.Parse(Console.ReadLine());
                guessCount++;

                if (magicNumber > guess)
                {
                    Console.WriteLine("Higher");
                }
                else if (magicNumber < guess)
                {
                    Console.WriteLine("Lower");
                }
                else
                {
                    Console.WriteLine("You guessed it!");
                }
            }

            // Display the number of guesses and ask if the user wants to play again.
            Console.WriteLine($"You made {guessCount} guesses.");
            Console.Write("Would you like to play again? ");
            playAgain = Console.ReadLine().Trim();
        }
        while (playAgain.Equals("yes", StringComparison.OrdinalIgnoreCase));
    }
}