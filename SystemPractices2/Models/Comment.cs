namespace SystemPractices2.Models;

public class Comment
{
    public int postId { get; set; }
    public int id { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string body { get; set; }
}
