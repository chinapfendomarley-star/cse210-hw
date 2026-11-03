using System;

public class Job
{
    public string _name;
    public string _jobTitle;
    public string _company;
    public string _startDate;
    public string _endDate;
    public string _jobDescription;

    public void Display()
    {
        Console.WriteLine($"{_name}: {_jobTitle} ({_company}) {_startDate}-{_endDate}");
    }
}
