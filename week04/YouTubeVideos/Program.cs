using System;
using System.Collections.Generic;
using System.Linq;

// Viewer's comment and formats it for display.
class Comment
{
    public string CommenterName { get; set; }
    public string TextContent { get; set; }
    public int LikesCount { get; private set; }
    public int DislikesCount { get; private set; }
    private readonly List<Comment> _replies = new List<Comment>();
    public IReadOnlyList<Comment> Replies => _replies;

    public Comment(string commenterName, string textContent)
    {
        CommenterName = commenterName;
        TextContent = textContent;
    }

    public string DisplayComment()
    {
        return $"{CommenterName}: \"{TextContent}\" (Likes: {LikesCount}, Dislikes: {DislikesCount})";
    }

    public void AddReply(Comment reply)
    {
        _replies.Add(reply);
    }

    public void AddLike()
    {
        LikesCount++;
    }

    public void RemoveLike()
    {
        if (LikesCount > 0)
        {
            LikesCount--;
        }
    }

    public void AddDislike()
    {
        DislikesCount++;
    }

    public void RemoveDislike()
    {
        if (DislikesCount > 0)
        {
            DislikesCount--;
        }
    }

    public double GetEngagementRatio()
    {
        int totalReactions = LikesCount + DislikesCount;
        return totalReactions == 0 ? 0 : (double)LikesCount / totalReactions;
    }

    public int GetTotalReplyCount()
    {
        return _replies.Sum(reply => 1 + reply.GetTotalReplyCount());
    }

    public int GetEngagementDepth()
    {
        return _replies.Count == 0
            ? 0
            : 1 + _replies.Max(reply => reply.GetEngagementDepth());
    }

    public void DisplayThread(int indentation = 2)
    {
        Console.WriteLine($"{new string(' ', indentation)}- {DisplayComment()}");
        foreach (Comment reply in _replies)
        {
            reply.DisplayThread(indentation + 2);
        }
    }
}

//Topic for a video.
class Tag
{
    public string Name { get; }

    public Tag(string name)
    {
        Name = name;
    }
}

//Video details and the comments associated with the video.
class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    public int LikesCount { get; private set; }
    public int DislikesCount { get; private set; }
    private readonly List<Comment> _comments;
    private readonly List<Tag> _tags = new List<Tag>();
    public IReadOnlyList<Tag> Tags => _tags;

    public Video(string title, string author, int lengthInSeconds)
    {
        Title = title;
        Author = author;
        LengthInSeconds = lengthInSeconds;
        _comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetTotalComments()
    {
        return _comments.Sum(comment => 1 + comment.GetTotalReplyCount());
    }

    public void AddLike()
    {
        LikesCount++;
    }

    public void RemoveLike()
    {
        if (LikesCount > 0)
        {
            LikesCount--;
        }
    }

    public void AddDislike()
    {
        DislikesCount++;
    }

    public void RemoveDislike()
    {
        if (DislikesCount > 0)
        {
            DislikesCount--;
        }
    }

    public double GetEngagementRatio()
    {
        int totalReactions = LikesCount + DislikesCount;
        return totalReactions == 0 ? 0 : (double)LikesCount / totalReactions;
    }

    public void AddTag(Tag tag)
    {
        if (!_tags.Any(existingTag => string.Equals(existingTag.Name, tag.Name, StringComparison.OrdinalIgnoreCase)))
        {
            _tags.Add(tag);
        }
    }

    public bool HasTag(string tagName)
    {
        return _tags.Any(tag => string.Equals(tag.Name, tagName, StringComparison.OrdinalIgnoreCase));
    }

    public static IEnumerable<Video> FindByTag(IEnumerable<Video> videos, string tagName)
    {
        return videos.Where(video => video.HasTag(tagName));
    }

    public void DisplayDetails()
    {
        // Printing the video information followed by each viewer comment.
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {LengthInSeconds} seconds");
        Console.WriteLine($"Total Comments: {GetTotalComments()}");
        Console.WriteLine($"Likes: {LikesCount}, Dislikes: {DislikesCount}, Engagement Ratio: {GetEngagementRatio():P0}");
        Console.WriteLine($"Tags: {string.Join(", ", _tags.Select(tag => tag.Name))}");
        Console.WriteLine();
        Console.WriteLine("Comments:");
        foreach (Comment comment in _comments)
        {
            comment.DisplayThread();
        }
        Console.WriteLine(new string('-', 40));
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        // Create sample videos and add three comments to each one.
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