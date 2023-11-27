# DOTRIS
#### Video Demo:  https://youtu.be/cRAK6zrYcUs


## Description
<img src="https://github.com/FateNozomi/Dotris/assets/6317676/7c90a7af-d200-4a83-a067-1c37ae300a51" alt="Modern Tetris gameplay" width=500>

Dotris is a recreation of modern Tetris written in C# using the [Godot game engine](https://godotengine.org/). As you can infer from the game's title, the game's theme revolves around dots. No assets were used other than Godot's custom 2D drawing functions: `draw_circle()` and `draw_arc()` to draw the dots. The game's core logic is written in .NET, so switching to a different game engine or front end framework shouldn't be much of a hassle.

## Features
- Delayed Auto-Shift (DAS)
- Lock Delay
- Next Piece
- Ghost Piece
- Hold Piece
- Super Rotation System
- 7-bag Random Generator


## Player Controls
Customizable controls under CONFIG.
- Move Piece Left: <kbd>←</kbd>
- Move Piece Right: <kbd>→</kbd>
- Soft Drop: <kbd>↓</kbd>
- Hard Drop: <kbd>Spacebar</kbd>
- Rotate Counterclockwise: <kbd>Z</kbd>
- Rotate Clockwise: <kbd>X</kbd>
- Hold: <kbd>Shift</kbd>
- Back: <kbd>Escape</kbd>

## Future Plans
- Implement background music and sound effects
- More complex top out handling
- Controller support