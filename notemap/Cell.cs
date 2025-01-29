namespace notemap;
using Newtonsoft.Json;

public class Cell
{
    public int X { get; set; }
    public int Y { get; set; }
    public string? Text { get; set; }
    public ConsoleColor Color { get; set; }
    public Cell(int x, int y, string? text, ConsoleColor color)
    {
        X = x;
        Y = y;
        Text = text;
        Color = color;
    }
    [JsonConstructor]
    public Cell() { }
}
