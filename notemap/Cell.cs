namespace notemap;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public string? Text { get; set; }
    public Cell(int x, int y, string text)
    {
        X = x;
        Y = y;
        Text = text;
    }
}
