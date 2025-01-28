namespace notemap;

public class Program
{
    class Cell
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
    static List<Cell> Cells = new()
    {
        new Cell(1, 1, "Welcome, use HJKL/WASD to move around"),
        new Cell(1, 2, "I or T to place text, enter to confirm"),
        new Cell(1, 3, "Escape to exit")
    };
    static int posX = 0;
    static int posY = 0;
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Draw();
        while (true)
        {
            var ki = Console.ReadKey(true);
            var key = ki.Key;
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.K:
                    posY--;
                    break;
                case ConsoleKey.A:
                case ConsoleKey.H:
                    posX--;
                    break;
                case ConsoleKey.S:
                case ConsoleKey.J:
                    posY++;
                    break;
                case ConsoleKey.D:
                case ConsoleKey.L:
                    posX++;
                    break;
                case ConsoleKey.Escape:
                    Console.CursorVisible = true;
                    return;
                case ConsoleKey.T:
                case ConsoleKey.I:
                    Console.CursorVisible = true;
                    var input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        Cells.Add(new(posX + 1, posY, input));
                    }
                    Console.CursorVisible = false;
                    break;
            }
            Draw();
        }
    }
    static void Draw()
    {
        Console.Clear();
        var width = Console.WindowWidth;
        var height = Console.WindowHeight;
        for (int i = 0; i < Cells.Count; i++)
        {
            Cell? cell = Cells[i];
            int wantX = cell.X - posX + (int)(width / 2);
            int wantY = cell.Y - posY + (int)(height / 2);
            if (cell != null && wantX > 0 && wantX < width && wantY > 0 && wantY < height)
            {
                try
                {
                    Console.SetCursorPosition(wantX, wantY);
                    Console.Write(cell.Text ?? "");
                }
                catch (System.Exception)
                {
                }
            }
        }
        try
        {
            // player
            Console.SetCursorPosition((int)(width / 2), (int)(height / 2));
            Console.Write('>');
        }
        catch (System.Exception)
        {
        }
    }
}
