using System;

class Program
{
    static void Main(string[] args)
    {
        Job job1 = new Job();
        job1._jobTitle = "Software Engineer";
        job1._company = "Microsoft";
        job1._startDate = "2019";
        job1._endDate = "2022";
        job1._jobDescription = "Built and improved software solutions.";

        Console.WriteLine(job1._company);

        Job job2 = new Job();
        job2._jobTitle = "Manager";
        job2._company = "Apple";
        job2._startDate = "2022";
        job2._endDate = "2023";
        job2._jobDescription = "Led team operations and project delivery.";

        Console.WriteLine(job2._company);

        job1.Display();
        job2.Display();

        Resume myResume = new Resume();
        myResume._name = "Jane Doe";
        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        Console.WriteLine(myResume._jobs[0]._jobTitle);

        myResume.Display();
    }
}