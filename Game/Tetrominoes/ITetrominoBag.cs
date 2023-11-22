using System.Collections.Generic;

namespace Dotris.Game.Tetrominoes;

public interface ITetrominoBag
{
    public List<Tetromino> Tetrominoes { get; }

    public Tetromino GetTetromino();
}