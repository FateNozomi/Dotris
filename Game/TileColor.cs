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
        Colors[(int)TetrominoShapes.J] = new Color(0x29 / 255f, 0x62 / 255f, 0xFF / 255f, 1);
        Colors[(int)TetrominoShapes.L] = new Color(0xFF / 255f, 0x6D / 255f, 0x00 / 255f, 1);
        Colors[(int)TetrominoShapes.O] = new Color(0xFF / 255f, 0xD6 / 255f, 0x00 / 255f, 1);
        Colors[(int)TetrominoShapes.S] = new Color(0x00 / 255f, 0xC8 / 255f, 0x53 / 255f, 1);
        Colors[(int)TetrominoShapes.T] = new Color(0xAA / 255f, 0x00 / 255f, 0xFF / 255f, 1);
        Colors[(int)TetrominoShapes.Z] = new Color(0xF4 / 255f, 0x43 / 255f, 0x36 / 255f, 1);
    }

    public Color[] Colors;
}