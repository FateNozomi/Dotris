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

## Project Structure
The project is split into 3 important parts: `Inputs`, `Tetrominoes` and `Game Logic`. The file tree below highlights the important classes for this game to function.
```
Game/
    Inputs/
        InputCommand.cs
        InputControls.cs
        InputEngine.cs
    Tetrominoes/
        Point.cs
        Tetromino.cs
        TetrominoBag.cs
        TetrominoShapes.cs
        WallKickData.cs
    Board.cs
    Game.cs
    Hold.cs
    HUD.cs
    Next.cs
    Tetris.cs
    TileColor.cs
```

### Inputs
`InputControls` enum holds all the possible controls used in Tetris.

`InputEngine` class is reponsible for handling all game inputs and each command is instantiated as an `InputCommand` class property. `InputEngine` has a `Register` method that is called when a key input is held and `Unregister` method for when the key is released.

`InputCommand` class handles the logic for `ARR` (Automatic Repeat Rate) and `DAS` (Delay Auto Shift).

### Tetrominoes
`Point` class contains properties `X` and `Y` for defining a point in a two-dimensional plane.

`Tetromino` abstract class is the base class for all Tetromino shapes. It handles the position of each tiles, rotation state, and wall kick data.

`TetrominoBag` class is responsible for generating a sequence of all 7 Tetronominoes in random order and must be exhausted before generating another bag.

`TetrominoShapes` enum holds all the Tetromino shapes used in the game.

`WallKickData` class contains all the possible movements for the wall kick when a piece is rotated.

### Game Logic
`Game` is the main class for handling the game display. It is responsible for relaying the game state from the `Tetris` class to `Board`, `Hold`, `Next`, and `HUD`.

`Tetris` class contains the core logic for the game.

`Board` class is responsible for displaying the game's playfield.

`Hold` class is responsible for showing the Tetromino piece held.

`Next` class is responsible for showing a preview of the next 5 Tetromino pieces in queue.

`HUD` class is an overlay displayed when the game is 'Paused' or when a 'Game Over' happens. It allows the player to restart the game or go to the main menu.

`TileColor` class contains all the colors used for the Tetrominoes.

## Future Plans
- Implement background music and sound effects
- More complex top out handling
- Controller support