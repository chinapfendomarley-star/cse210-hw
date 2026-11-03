using System;
using System.Collections.Generic;

public class Resume
{
    // Variables for the Resume class
    public string _name;
    public List<Job> _jobs = new List<Job>();

    // The resume details including name and all jobs
    public void Display()
    {
        Console.WriteLine($"Name: {_name}");
        Console.WriteLine("Jobs:");
        
        foreach (Job job in _jobs)
        {
            job.Display();
        }
    }
}