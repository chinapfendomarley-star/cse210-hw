using System;
using System.Diagnostics;

sealed class ReflectionActivity : Activity
{
    //Reflection prompts that are returned in a randomized cycle.
    private readonly RandomizedCycle<string> _prompts = new RandomizedCycle<string>(new[]
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    });

    // Follow-up questions that are returned in a randomized cycle.
    private readonly RandomizedCycle<string> _questions = new RandomizedCycle<string>(new[]
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    });

    // Configuring this activity with its name and description.
    public ReflectionActivity()
        : base(
            "Reflection Activity",
            "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
    }

    // Loop presenting a reflection prompt and cycling through questions until time expires.
    protected override void RunActivity()
    {
        Console.WriteLine("\nConsider the following prompt:");
        Console.WriteLine($"--- {_prompts.Next()} ---");

        Stopwatch timer = Stopwatch.StartNew();
        // Loop asking randomized questions while time remains.
        while (GetRemainingSeconds(timer) > 0)
        {
            Console.WriteLine($"\n{_questions.Next()}");
            int pauseSeconds = Math.Min(5, GetRemainingSeconds(timer));
            PauseWithSpinner(pauseSeconds, "Reflect on your experience");
        }
    }
}
