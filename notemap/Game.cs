namespace notemap;
using System.Text.Json;

public class Game
{
    public List<Cell> Cells { get; set; }
    public int posX { get; set; }
    public int posY { get; set; }
    public static string? FilePath = null;
    public Game()
    {
        Cells = new()
        {
            new(1, 1, "Welcome, use HJKL/WASD to move around"),
            new(1, 2, "I to place text, T to edit, enter to confirm"),
            new(1, 3, "R or Delete to remove a piece of text"),
            new(1, 4, "Esc or Q to exit")
        };
        posX = 0;
        posY = 0;
    }
    public void Run()
    {
        Console.CursorVisible = false;
        Draw();
        while (true)
        {
            var ki = Console.ReadKey(true);
            var key = ki.Key;
        again:
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
                case ConsoleKey.I:
                    {
                        Console.CursorVisible = true;
                        var input = Console.ReadLine();
                        if (!string.IsNullOrEmpty(input))
                        {
                            Cells.Add(new(posX + 1, posY, input));
                        }
                        Console.CursorVisible = false;
                    }
                    break;
                case ConsoleKey.R:
                case ConsoleKey.Delete:
                    {
                        // check if the player is on a piece of text
                        var cell = Cells.FirstOrDefault(c => c.Y == posY && posX >= c.X && posX < c.X + c.Text?.Length);
                        if (cell != null)
                        {
                            Cells.Remove(cell);
                        }
                    }
                    break;
                case ConsoleKey.T:
                    {
                        // check if the player is on a piece of text
                        // move to the start of it and edit it
                        var cell = Cells.FirstOrDefault(c => c.Y == posY && posX >= c.X && posX < c.X + c.Text?.Length);
                        if (cell != null)
                        {
                            posX = cell.X - 1;
                            posY = cell.Y;
                            Cells.Remove(cell);
                            Draw();
                            Console.CursorVisible = true;
                            key = ConsoleKey.I;
                            goto again;
                        }
                    }
                    break;
                case ConsoleKey.E:
                    {
                        // mode for more options that don't fit into common keys
                        var ki2 = Console.ReadKey(true);
                        var key2 = ki2.Key;
                        switch (key2)
                        {
                            case ConsoleKey.S:
                                {
                                    Console.Clear();
                                    string jsonText = JsonSerializer.Serialize(this);
                                    if (FilePath == null)
                                    {
                                        Console.CursorVisible = true;
                                        Console.Write("Please enter the path of a .json file to save the data to: ");
                                        var input = Console.ReadLine() ?? "";
                                        var fullPath = Utils.GetFullPath(input);
                                        if (!fullPath.EndsWith(".json"))
                                        {
                                            fullPath += ".json";
                                        }
                                        var parts = fullPath.Split(Path.DirectorySeparatorChar);
                                        var parentDirectory = string.Join(Path.DirectorySeparatorChar, parts.Take(parts.Length - 1));
                                        if (!Directory.Exists(parentDirectory))
                                        {
                                            Directory.CreateDirectory(parentDirectory);
                                        }
                                        FilePath = fullPath;
                                        Console.CursorVisible = false;
                                    }
                                    File.WriteAllText(FilePath, jsonText);
                                    try
                                    {
                                        Console.SetCursorPosition(0, Console.WindowHeight);
                                        Console.Write("Saved!");
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                break;
                            case ConsoleKey.L:
                                {
                                    Console.Clear();
                                    Console.CursorVisible = true;
                                    Console.Write("Please enter the path of a .json file to load data from: ");
                                    var input = Console.ReadLine() ?? "";
                                    var fullPath = Utils.GetFullPath(input);
                                    if (!fullPath.EndsWith(".json") && !File.Exists(fullPath) && File.Exists(fullPath + ".json"))
                                    {
                                        fullPath += ".json";
                                    }
                                    var parts = fullPath.Split(Path.DirectorySeparatorChar);
                                    var parentDirectory = string.Join(Path.DirectorySeparatorChar, parts.Take(parts.Length - 1));
                                    if (File.Exists(fullPath))
                                    {
                                        FilePath = fullPath;
                                        var jsonText = File.ReadAllText(fullPath);
                                        try
                                        {
                                            var game = JsonSerializer.Deserialize<Game>(jsonText);
                                            if (game != null)
                                            {
                                                game.Run();
                                                Environment.Exit(0);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    Console.CursorVisible = false;
                                }
                                break;
                        }
                    }
                    break;
            }
            Draw();
        }
    }
    void Draw()
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.Write($"[{posX}, {posY}]");
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
