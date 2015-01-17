OurGame
=======

This is a 2D video game level editor created with the XNA framework.  **(This version discontinued 1/16/2015)**

---

**Building**

**These instructions are for Visual Studio 2010 Express and the XNA Studio Framework.**

After a fresh clone you must do the following to get nuget to fetch the dependencies (XUnit at the moment):

1.  Go to the **OurGame** directory with **build.proj** in it.

2.  Run the following command:

**msbuild build.proj /t:Build /p:Platform="x86"**

This will download XUnit automatically and put it into the 'packages' directory.

---
**How to setup xUnit in visual studio**

1.  Go to **Tools->External Tools**... menu

2.  Click **Add**

3. **Enter** the **following:**


Title:  **XUnit Test**

Command:  **Path to OurGame\OurGame\packages\xunit.runners.1.9.2\tools\xunit.console.clr4.exe**

Arguments:  **$(BinDir)$(TargetName)$(TargetExt)**

Initial directory:  **$(BinDir)**


4.  Check **"Use Ouput Window"**

5.  Click **Ok**.



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

