OurGame
=======

This is a 2D video game level editor created with the XNA framework.

---

**Building on Visual Studio 2013 Community Edition**

After a fresh clone you must do the following to get nuget to fetch the dependencies (XUnit at the moment):

1.  Go to **Tools -> NuGet Package Manager -> Package Manager Console.**

2.  Enter:  **Install-Package xunit -Version 1.9.2**

3.  Enter:  **Install-Package xunit.runner.visualstudio -Pre**

---

**Controls** - **Game Board Edit Mode**

**mouse scroll wheel** - makes the brush bigger or smaller

**left keyboard arrow/right keyboard arrow** - use to scroll the board

**left mouse button** - place a tile on the game board

**right mouse button** - change to next texture under the mouse cursor.

**press Z key** - undo placing a tile on the game board grid

**press S key** - save the game boards' configuration as MyLevel.txt in the location of the .exe executable file

**press D key** - reset the game board and write out  a "blank" MyLevel.txt.

**press PageUp** -- increase the size of the tile under the mouse cursor.

**press PageDown** - Decrease the size of the tile under the mouse cursor.

**press U key** - To undo if the D key was pressed (The whole board was deleted.)

---

**Controls** - **Play Game Mode**

**left keyboard arrow/right keyboard arrow** - Scroll the board and move the character.

**press Space Bar** - Make character jump.

**press R key** - Reverse time and undo all the movement and jumps you did so far.

---
**General Help** - **Any Mode**

**press B key** - Go to Blank Screen.

**press E key** - Go to Editor screen.

**press P key** - Enter play game mode that scrolls the board by pressing Left/Right arrow keys

**press H key** - To get help with keyboard and mouse commands

**press Q key** - Quit the editor.

