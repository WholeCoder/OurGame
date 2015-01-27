OurGame
=======

This is a 2D video game level editor created with the MonoGame framework.

---

**Building on Visual Studio 2013 Community Edition**

1.  Download and install **Visual Studio 2013 Community Edition** <a href="http://www.visualstudio.com/products/free-developer-offers-vs?wt.mc_id=p~CO~COMMUNITY~SEARCH~FY15~11Nov~GOOGLE~visual%20studio%20free%20download&WT.srch=1&WT.mc_id=SEM_GOO_USEvergreenSearch_Unassigned&CR_CC=300219949">here</a>

2.  Download and install the **MonoGame** <a href="http://www.monogame.net/">Here</a>

After a fresh clone you must do the following to get nuget to fetch the dependencies (XUnit at the moment).  

3.  Open up the solution with **Visual Studio** 2013 in the ./OurGame directory.

4.  Go to **Tools -> NuGet Package Manager -> Package Manager Console.**

5.  Enter:  **Install-Package xunit -Version 1.9.2**

6.  Enter:  **Install-Package xunit.runner.visualstudio -Pre**

---

**Important Note**  - In order to use a **SpriteFont** you must use **Visual Studio 2010 Express** to **compile** them into **.xnb** files and then **include** them in the **Content/font** directory and make them **copy to output**.

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

**Controls** - **Edit Sprites Mode**

**moust left click** - Place a sprite on the game board.

**press S key** - Save the sprite positions.

---

**General Help** - **Any Mode**

**press B key** - Go to Blank Screen.

**press E key** - Go to Editor screen.

**press P key** - Enter play game mode that scrolls the board by pressing Left/Right arrow keys

**press H key** - To get help with keyboard and mouse commands

**press N key** - Enter Edit Sprite mode.

**press Q key** - Quit the editor.

