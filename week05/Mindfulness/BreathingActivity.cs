using System;
using System.Diagnostics;

sealed class BreathingActivity : Activity
{
    // Configuring this activity with its name and description.
    public BreathingActivity()
        : base(
            "Breathing Activity",
            "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    // Alternate between breathing in and breathing out for the selected duration.
    protected override void RunActivity()
    {
        Stopwatch timer = Stopwatch.StartNew();
        bool breatheIn = true;

        // Continue alternating until no time remains.
        while (GetRemainingSeconds(timer) > 0)
        {
            // Limiting each breathing phase to four seconds or the remaining duration.
            int pauseSeconds = Math.Min(4, GetRemainingSeconds(timer));
            Console.WriteLine(breatheIn ? "\nBreathe in..." : "\nBreathe out...");
            PauseWithCountdown(pauseSeconds, " ");
            breatheIn = !breatheIn;
        }
    }
}
