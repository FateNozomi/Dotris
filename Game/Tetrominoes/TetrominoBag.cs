using System;
using System.Collections.Generic;
using System.Linq;
using Dotris.Extensions;

namespace Dotris.Game.Tetrominoes;

public class TetrominoBag
{
    private Random _random;

    public TetrominoBag()
    {
        _random = new Random();
    }

    public List<Tetromino> Tetrominoes { get; } = new();

    public Tetromino GetTetromino()
    {
        int count = Tetrominoes.Count;
        if (count < 7)
        {
            Tetrominoes.AddRange(GenerateRandomizedTetriminoBag());
        }

        var tetromino = Tetrominoes.First();
        Tetrominoes.Remove(tetromino);

        return tetromino;
    }

    private IEnumerable<Tetromino> GenerateRandomizedTetriminoBag()
    {
        List<Tetromino> tetriminos = new List<Tetromino>
        {
            new TetrominoI(),
            new TetrominoJ(),
            new TetrominoL(),
            new TetrominoO(),
            new TetrominoS(),
            new TetrominoT(),
            new TetrominoZ()
        };

        return tetriminos.Shuffle(_random).Shuffle(_random);
    }
}