using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGameLibrary1
{
    public class Tile
    {
        public int Width { get; set; }
        public int Height { get; set; }

        // These are the pixel co-ordinates on the screen of this tile.
        public int ScreenX { get; set; }
        public int ScreenY { get; set; }

        // These are the co-ordinates in the board's array;
        public int ArrayX {get; set;}
        public int ArrayY { get; set; }

        // These next co-ordinates specify a line that the character will stand on.  Right now
        // it will just be a straight horizontal line but in the future we mihgt give it an
        // incline or decline.
        public int StartBoundaryX { get; set; }
        public int StartBoundaryY { get; set; }

        public int EndBoundaryX { get; set; }
        public int EndBoundaryY { get; set; }

        public Tile(int screenX, // in pixels
                    int screenY, 

                    int arrayX,  // array position column   DON'T FORGET THESE ARE BACKWARDS IN ARRAY!!
                    int arrayY,  //                row

                    int width,   // in pixels
                    int height,

                    // in pixels relative tot he upper left- hand corner of tile (in pixels)
                    int startBoundaryX, 
                    int startBoundaryY, 

                    int endBoundaryX, 
                    int endBoundaryY)
        {
            this.ScreenX = screenX;
            this.ScreenY = screenY;

            this.ArrayX = arrayX;
            this.ArrayY = arrayY;

            this.Width = width;
            this.Height = height;

            this.StartBoundaryX = startBoundaryX;
            this.StartBoundaryY = startBoundaryY;
        }
    }
}
