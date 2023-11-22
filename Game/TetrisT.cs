using System;
using Dotris.Game.Tetrominoes;

namespace Dotris.Game;

public class TetrisT : Tetris
{
    private Random _random = new();
    private string[] _garbageDataSet;
    private int _spawnCounter;

    public TetrisT(int rows = 23, int columns = 10)
        : base(rows, columns)
    {
        TetrominoBag = new TetrominoBagOfT();
        _garbageDataSet = new string[]
        {
            @"
0008888888
8008888888
8000888888
8880888888
8800888888
8800088888
8880888888
8880888888",
            @"
8888888000
8888888008
8888880008
8888880888
8888880088
8888800088
8888880888
8888880888",
        };
    }

    public override void Start()
    {
        _spawnCounter = 0;
        base.Start();
    }

    public override void SpawnTetromino()
    {
        int dataIndex = _random.Next(_garbageDataSet.Length);
        if (_spawnCounter % 4 == 0)
        {
            SpawnGarbage(_garbageDataSet[dataIndex]);
        }

        _spawnCounter++;
        base.SpawnTetromino();
    }
}