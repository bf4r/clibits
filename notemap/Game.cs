namespace notemap;
using Newtonsoft.Json;

public class Game
{
    public List<Cell> Cells { get; set; } = new();
    public int posX { get; set; }
    public int posY { get; set; }
    public static string? FilePath { get; set; } = null;
    public static Cell? Clipboard { get; set; } = null;
    public List<Cell> FollowingCells { get; set; } = new();
    public void Run()
    {
        Console.CursorVisible = false;
        Draw();
        while (true)
        {
            var ki = Console.ReadKey(true);
            var key = ki.Key;
            int moveStep = 1;
            if (ki.Modifiers == ConsoleModifiers.Shift)
            {
                moveStep = 5;
            }
        again:
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.K:
                    posY -= moveStep;
                    MoveFollowingCells(0, -moveStep);
                    break;
                case ConsoleKey.A:
                case ConsoleKey.H:
                    posX -= moveStep;
                    MoveFollowingCells(-moveStep, 0);
                    break;
                case ConsoleKey.S:
                case ConsoleKey.J:
                    posY += moveStep;
                    MoveFollowingCells(0, moveStep);
                    break;
                case ConsoleKey.D:
                case ConsoleKey.L:
                    posX += moveStep;
                    MoveFollowingCells(moveStep, 0);
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
                            Cells.Add(new(posX + 1, posY, input, ConsoleColor.White));
                        }
                        Console.CursorVisible = false;
                    }
                    break;
                case ConsoleKey.R:
                case ConsoleKey.Delete:
                    {
                        var cell = GetCellPlayerIsOn();
                        if (cell != null)
                        {
                            Cells.Remove(cell);
                        }
                    }
                    break;
                case ConsoleKey.T:
                    {
                        // move to the start of it and edit it
                        var cell = GetCellPlayerIsOn();
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
                                    string jsonText = JsonConvert.SerializeObject(this);
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
                                            var game = JsonConvert.DeserializeObject<Game>(jsonText);
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
                            case ConsoleKey.J:
                                {
                                    // jump to coordinates
                                    var ki3 = Console.ReadKey(true);
                                    var key3 = ki3.Key;
                                    switch (key3)
                                    {
                                        case ConsoleKey.X:
                                            {
                                                Console.CursorVisible = true;
                                                Console.Clear();
                                                Console.Write("Enter the X coordinate to jump to: ");
                                                var input = Console.ReadLine();
                                                Console.CursorVisible = false;
                                                if (!int.TryParse(input, out int x))
                                                {
                                                    break;
                                                }
                                                posX = x;
                                            }
                                            break;
                                        case ConsoleKey.Y:
                                            {
                                                Console.CursorVisible = true;
                                                Console.Clear();
                                                Console.Write("Enter the Y coordinate to jump to: ");
                                                var input = Console.ReadLine();
                                                Console.CursorVisible = false;
                                                if (!int.TryParse(input, out int y))
                                                {
                                                    break;
                                                }
                                                posY = y;
                                            }
                                            break;
                                        case ConsoleKey.S:
                                        case ConsoleKey.Multiply:
                                            {
                                                // jump to spawn
                                                posX = 0;
                                                posY = 0;
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case ConsoleKey.Y:
                    {
                        // copy its text to the notemap clipboard
                        var cell = GetCellPlayerIsOn();
                        if (cell != null)
                        {
                            Clipboard = cell;
                        }
                    }
                    break;
                case ConsoleKey.P:
                    {
                        if (Clipboard != null)
                        {
                            Cells.Add(new Cell(posX + 1, posY, Clipboard.Text, Clipboard.Color));
                        }
                    }
                    break;
                case ConsoleKey.M:
                    {
                        var cell = GetCellPlayerIsOn();
                        if (cell != null)
                        {
                            if (FollowingCells.Contains(cell))
                            {
                                FollowingCells.Remove(cell);
                            }
                            else
                            {
                                FollowingCells.Add(cell);
                            }
                        }
                    }
                    break;
                case ConsoleKey.G:
                    {
                        var cell = GetCellPlayerIsOn();
                        if (cell != null)
                        {
                            // cycle color
                            var colors = Enum.GetValues<ConsoleColor>().Cast<ConsoleColor>().ToList();
                            var currentColor = cell.Color;
                            var currentColorIndex = 0;
                            for (int i = 0; i < colors.Count; i++)
                            {
                                if (colors[i] == currentColor)
                                {
                                    currentColorIndex = i;
                                    break;
                                }
                            }
                        pickIndexAgain:
                            currentColorIndex++;
                            if (currentColorIndex >= colors.Count)
                            {
                                currentColorIndex = 0;
                            }
                            var newColor = colors[currentColorIndex];
                            if (newColor == ConsoleColor.Black)
                            {
                                goto pickIndexAgain;
                            }
                            cell.Color = newColor;
                        }
                    }
                    break;
            }
            Draw();
        }
    }
    void MoveFollowingCells(int x, int y)
    {
        for (int i = 0; i < FollowingCells.Count; i++)
        {
            var cell = FollowingCells[i];
            cell.X += x;
            cell.Y += y;
        }
    }
    Cell? GetCellPlayerIsOn()
    {
        return Cells.FirstOrDefault(c => c.Y == posY && posX >= c.X && posX < c.X + c.Text?.Length);
    }
    void Draw()
    {
        Console.Clear();
        Console.ResetColor();
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
                    Console.ForegroundColor = cell.Color;
                    Console.Write(text);
                }
                catch (System.Exception)
                {
                }
            }
        }
        try
        {
            Console.ResetColor();
            // player
            Console.SetCursorPosition((int)(width / 2), (int)(height / 2));
            Console.Write('>');
        }
        catch (System.Exception)
        {
        }
    }
}
