using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotris.Game.Tetrominoes;

public class TetrominoBagOfT : ITetrominoBag
{
    private Random _random;

    public TetrominoBagOfT()
    {
        _random = new Random();
    }

    public List<Tetromino> Tetrominoes { get; } = new();

    public Tetromino GetTetromino()
    {
        int count = Tetrominoes.Count;
        if (count < 7)
        {
            Tetrominoes.AddRange(GenerateTetrominoBagOfT());
        }

        var tetromino = Tetrominoes.First();
        Tetrominoes.Remove(tetromino);

        return tetromino;
    }

    private IEnumerable<Tetromino> GenerateTetrominoBagOfT()
    {
        List<Tetromino> tetrominoes = new List<Tetromino>();
        for (int i = 0; i < 7; i++)
        {
            tetrominoes.Add(new TetrominoT());
        }

        return tetrominoes;
    }
}