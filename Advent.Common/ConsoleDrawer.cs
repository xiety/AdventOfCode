#pragma warning disable CA1416

namespace System;

public class ConsoleDrawer
{
    private readonly int width;
    private readonly int height;

    private readonly char[,] lastFrame;
    private readonly char[,] currentFrame;

    public ConsoleDrawer(int width, int height)
    {
        this.width = width;
        this.height = height;

        lastFrame = new char[width, height];
        currentFrame = new char[width, height];

        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                lastFrame[x, y] = ' ';
                currentFrame[x, y] = ' ';
            }
        }

        Console.BufferWidth = Console.WindowWidth = Console.LargestWindowWidth;
        Console.BufferHeight = Console.WindowHeight = Console.LargestWindowHeight;
        Console.CursorVisible = false;
    }

    public void Put(int x, int y, char c)
    {
        currentFrame[x, y] = c;
    }

    public void Draw()
    {
        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                if (currentFrame[x, y] != lastFrame[x, y])
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(currentFrame[x, y]);

                    lastFrame[x, y] = currentFrame[x, y];
                }

                currentFrame[x, y] = ' ';
            }
        }
    }
}
