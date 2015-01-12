OurGame
=======

This is a 2D video game level editor created with the XNA framework.

---

**Building**
After a fresh clone you must do the following to get nuget to fetch the dependencies (XUnit at the moment):

1.  Go to the **OurGame** directory with **packages.config** in it.

2.  Run the following command:

**nuget.exe install -o packages .\packages.config**

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
**General Help** - **Any Mode**

**press B key** - Go to Blank Screen (Only when on editor grid screen)

**press E key** - Go to Editor screen (Only when on blank screen)

**press P key** - Enter play game mode that scrolls the board by pressing Left/Right arrow keys

**press H key** - To get help with keyboard and mouse commands

**press Q key** - Quit the editor.

