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
        new Cell(1, 3, "R or Delete to remove a piece of text"),
        new Cell(1, 4, "Escape to exit")
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
                case ConsoleKey.Q:
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
                case ConsoleKey.R:
                case ConsoleKey.Delete:
                    // check if the player is on a piece of text
                    var cell = Cells.FirstOrDefault(c => c.Y == posY && posX >= c.X && posX < c.X + c.Text?.Length);
                    if (cell != null)
                    {
                        Cells.Remove(cell);
                    }
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
            bool isInBounds = (wantX >= 0 && wantX < width && wantY >= 0 && wantY < height);
            bool isOutOfBoundsLeftButSomeTextShouldShow = (wantY >= 0 && wantY < height && wantX < 0 && cell.Text != null && cell.Text.Length + wantX > 0);

            if (cell != null && (isInBounds || isOutOfBoundsLeftButSomeTextShouldShow))
            {
                try
                {
                    var text = cell.Text ?? "";
                    if (isOutOfBoundsLeftButSomeTextShouldShow)
                    {
                        text = text.Substring(-wantX);
                        Console.SetCursorPosition(0, wantY);
                    }
                    else
                    {
                        Console.SetCursorPosition(wantX, wantY);
                        if (text.Length + wantX > width)
                        {
                            text = text.Substring(0, width - wantX);
                        }
                    }
                    Console.Write(text);
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
