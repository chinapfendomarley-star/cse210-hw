using System;

internal class NewBaseType
{
    static void Main(string[] args)
    {
        Fraction fraction1 = new Fraction();
        Fraction fraction2 = new Fraction(5);
        Fraction fraction3 = new Fraction(3, 4);
        Fraction fraction4 = new Fraction(1, 3);

        DisplayFraction(fraction1);
        DisplayFraction(fraction2);
        DisplayFraction(fraction3);
        DisplayFraction(fraction4);

        
    }

    private static void DisplayFraction(Fraction fraction)
    {
        Console.WriteLine($"{fraction.GetFractionString()} = {fraction.GetDecimalValue()}");
    }
}
