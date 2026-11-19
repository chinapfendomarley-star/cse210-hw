using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Create sample videos and add at least three comments to each one.
        List<Video> videos = new List<Video>();

        Video v1 = new Video("The Africa Awakening", "PLO Lumumba", 710);
        Comment firstComment = new Comment("Tawanda", "Africa's renewal begins when we invest in its people and leadership.");
        firstComment.AddLike();
        firstComment.AddReply(new Comment("PLO Lumumba", "A strong reminder that African unity and vision are essential."));
        v1.AddComment(firstComment);
        v1.AddComment(new Comment("Tamia", "This message highlights the importance of African independence and self-belief."));
        v1.AddComment(new Comment("Sasha", "Africa has the talent and potential to shape its own future."));
        v1.AddLike();
        v1.AddLike();
        v1.AddDislike();
        v1.AddTag(new Tag("History"));
        v1.AddTag(new Tag("African Studies"));
        videos.Add(v1);

        Video v2 = new Video("Top 5 Tourist Destinations in Zimbabwe", "TrailBlazerZW", 840);
        v2.AddComment(new Comment("Trish", "Victoria Falls is breathtaking in the morning."));
        v2.AddComment(new Comment("Elena", "Adding Vumba to my bucket list!"));
        v2.AddComment(new Comment("Timothy", "Great drone shots at the end."));
        v2.AddTag(new Tag("Travel"));
        videos.Add(v2);

        Video v3 = new Video("Last 24 Hour World News", "Global News", 780);
        v3.AddComment(new Comment("Chloe", "The international headlines were clear and easy to follow."));
        v3.AddComment(new Comment("Daniel", "I appreciated the quick update on current world events."));
        v3.AddComment(new Comment("Victoria", "Thanks for covering the latest news in one 24-hour recap."));
        v3.AddTag(new Tag("Education"));
        v3.AddTag(new Tag("News"));
        videos.Add(v3);

        // Display each video's details and its comments.
        foreach (Video video in videos)
        {
            video.DisplayDetails();
        }

        Console.WriteLine("Videos tagged Education:");
        foreach (Video video in Video.FindByTag(videos, "education"))
        {
            Console.WriteLine($"- {video.Title}");
        }
    }
}