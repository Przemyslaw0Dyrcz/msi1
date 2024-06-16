using System.Collections.Generic;

public class Store
{
    public List<Article> Articles { get; set; }
    public (int X, int Y) Entrance { get; set; } = (0, 0);
    public (int X, int Y) CashRegister { get; set; } = (0, 1);

    public Store()
    {
        Articles = new List<Article>
        {
            new Article("Milk", 1.0, (2, 1)),
            new Article("Bread", 0.5, (2, 2)),
            new Article("Eggs", 0.3, (2, 3)),
            new Article("Butter", 0.2, (2, 4)),
            new Article("Cheese", 0.7, (2, 5)),

            new Article("Tomatoes", 0.4, (4, 1)),
            new Article("Potatoes", 1.5, (4, 2)),
            new Article("Chicken", 1.2, (4, 3)),
            new Article("Fish", 1.1, (4, 4)),
            new Article("Apples", 0.8, (4, 5)),

            new Article("Oranges", 0.9, (6, 1)),
            new Article("Bananas", 0.6, (6, 2)),
            new Article("Grapes", 0.3, (6, 3)),
            new Article("Pasta", 0.5, (6, 4)),
            new Article("Rice", 0.7, (6, 5)),

            new Article("Carrots", 0.4, (8, 1)),
            new Article("Peas", 0.5, (8, 2)),
            new Article("Lettuce", 0.3, (8, 3)),
            new Article("Cucumbers", 0.2, (8, 4)),
            new Article("Peppers", 0.7, (8, 5)),

            new Article("Crunchy", 0.6, (0, 7)),
            new Article("Tymbark", 1.0, (1, 7)),
            new Article("Onions", 0.5, (2, 7)),
            new Article("Water", 1.0, (3, 7)),
            new Article("Garlic", 0.2, (4, 7)),
            new Article("Pills", 0.2, (5, 7)),
            new Article("Ginger", 0.3, (6, 7)),
            new Article("Heroin", 0.1, (7, 7)),
            new Article("Celery", 0.5, (8, 7)),
            new Article("Knife", 0.3, (9, 7))
        };
    }
}