public class Article
{
    public string Name { get; set; }
    public double Weight { get; set; }
    public (int X, int Y) Location { get; set; }

    public Article(string name, double weight, (int, int) location)
    {
        Name = name;
        Weight = weight;
        Location = location;
    }
}
