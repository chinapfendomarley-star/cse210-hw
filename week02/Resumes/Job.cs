using System;

public class Job
{
    // Variables for the Job class
    public string _jobTitle;
    public string _company;
    public string _startYear;
    public string _endYear;

    // Display job details in the specified format
    public void Display()
    {
        Console.WriteLine($"{_jobTitle} ({_company}) {_startYear}-{_endYear}");
    }
}