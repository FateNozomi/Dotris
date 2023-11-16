using System;

namespace Dotris.Game;

public class LineClearedEventArgs : EventArgs
{
    public LineClearedEventArgs(int lineIndex)
    {
        LineIndex = lineIndex;
    }

    public int LineIndex { get; }
}