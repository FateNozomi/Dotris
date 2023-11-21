using Godot;
using System;
using Dotris.Game.Tetrominoes;

namespace Dotris.Game;

public class TileColor
{
    public TileColor()
    {
        int count = Enum.GetNames<TetrominoShapes>().Length;
        Colors = new Color[count];

        Colors[(int)TetrominoShapes.None] = new Color(0x30 / 255f, 0x30 / 255f, 0x30 / 255f, 1);
        Colors[(int)TetrominoShapes.I] = new Color(0x00 / 255f, 0xBC / 255f, 0xD4 / 255f, 1);
        Colors[(int)TetrominoShapes.J] = new Color(0x21 / 255f, 0x96 / 255f, 0xF3 / 255f, 1);
        Colors[(int)TetrominoShapes.L] = new Color(0xFF / 255f, 0x98 / 255f, 0x00 / 255f, 1);
        Colors[(int)TetrominoShapes.O] = new Color(0xFF / 255f, 0xEB / 255f, 0x3B / 255f, 1);
        Colors[(int)TetrominoShapes.S] = new Color(0x4C / 255f, 0xAF / 255f, 0x50 / 255f, 1);
        Colors[(int)TetrominoShapes.T] = new Color(0x9C / 255f, 0x27 / 255f, 0xB0 / 255f, 1);
        Colors[(int)TetrominoShapes.Z] = new Color(0xF4 / 255f, 0x43 / 255f, 0x36 / 255f, 1);
        Colors[(int)TetrominoShapes.FreeForm] = new Color(0x75 / 255f, 0x75 / 255f, 0x75 / 255f, 1);
    }

    public Color[] Colors;
}