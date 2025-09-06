using blackbox.Graphics;
using blackbox.Utils;
using Microsoft.Xna.Framework;
using System;

namespace blackbox.Assets;

public class MapGridData
{
    public int[,] Data;
    public int Columns;
    public int Rows;
    public int Size;
    public int Total;

    public void Create(int columns, int rows, int size)
    {
        Columns = columns;
        Rows = rows;
        Size = size;
        Total = Columns * Rows;
        Data = new int[Rows, Columns];
    }

    public void SetMap(string grid)
    {
        ArrayUtils.StringToIntArray(Data, grid);
    }

    public string GetMap()
    {
        return ArrayUtils.IntArrayToString(Data);
    }

    public void SetTile(int x, int y, int tileIndex)
    {
        if (InvalidGridPos(x, y))
        {
            return;
        }
        Data[y, x] = tileIndex;
    }

    public void UpdateTiles(int x0, int y0, int x1, int y1, int tileIndex)
    {
        var (rx0, ry0, rx1, ry1, w, h) = Shapes.AdjustRect(x0, y0, x1, y1);
        (rx0, ry0, rx1, ry1) = Shapes.ClampToBounds(rx0, ry0, w, h, Columns * Size, Rows * Size);

        for (int x = rx0; x < rx1; x++)
        {
            for (int y = ry0; y < ry1; y++)
            {
                Data[y, x] = tileIndex;
            }
        }
    }

    public void DrawMap(
        GridData gridData, int mapX, int mapY,   // starting tile in map
        int px, int py,       // screen position to draw at
        int width, int height, // how many tiles wide/tall to draw
        Color color)
    {
        for (int y = 0; y < height; y++)
        {
            int mapYIndex = mapY + y;
            if (mapYIndex < 0 || mapYIndex >= Rows) continue;

            for (int x = 0; x < width; x++)
            {
                int mapXIndex = mapX + x;
                if (mapXIndex < 0 || mapXIndex >= Columns) continue;

                int tileIndex = Data[mapYIndex, mapXIndex];
                if (tileIndex <= 0) continue;

                Rectangle source = gridData.TileRects[tileIndex];
                Rectangle dest = new Rectangle(
                    px + x * Size,
                    py + y * Size,
                    Size,
                    Size);

                GFW.SpriteBatch.Draw(gridData.Texture, dest, source, color);
            }
        }
    }

    public bool InvalidGridPos(int x, int y)
    {
        return x < 0 || y < 0 || x >= Columns || y >= Rows;
    }
}

public static class MapGrid
{
    public static MapGridData[] GridList = new MapGridData[Constants.MaxGameGrid];

    public static void Create(int index, int columns, int rows, int size)
    {
        GridList[index] = new MapGridData();
        GridList[index].Create(columns, rows, size);
    }

    public static void SetMap(int index, string grid)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].SetMap(grid);
    }

    private static bool IsValidIndex(int index)
    {
        return index >= 0 && index < Constants.MaxGameGrid && GridList[index] is not null && GameGrid.GridList[index] is not null;
    }

    public static string GetMap(int index)
    {
        if (!IsValidIndex(index))
        {
            return string.Empty;
        }

        return GridList[index].GetMap();
    }

    public static void SetTile(int index, int x, int y, int tileIndex)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].SetTile(x, y, tileIndex);
    }

    public static void UpdateTileInMap(int index, int x0, int y0, int x1, int y1, int tileIndex)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].UpdateTiles(x0, y0, x1, y1, tileIndex);
    }

    public static void DrawMap(
        int index, int mapX, int mapY,   // starting tile in map
        int px, int py,       // screen position to draw at
        int width, int height, // how many tiles wide/tall to draw
        Color color)
    {
        if (!IsValidIndex(index))
        {
            return;
        }

        GridList[index].DrawMap(GameGrid.GridList[index], mapX, mapY, px, py, width, height, color);
    }
}
