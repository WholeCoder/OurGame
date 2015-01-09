using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OurGame.OurGameLibrary
{
    public class Tile
    {
        public Texture2D TheTexture { get; set; }  // null denotes an empty tile.

        // These are in pixels.
        public int Width { get; set; }
        public int Height { get; set; }

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

        // This will be changed by the Board class to allow a check to see if something (ex: Player) hits a Tile on the board.
        public Rectangle BoundingRectangle;

        public Tile(Texture2D tTexture,

                    int arrayX,  // array position column   DON noT FORGET THESE ARE BACKWARDS IN ARRAY!!
                    int arrayY,  //                row

                    int width,   // in pixels
                    int height,

                    // in pixels relative tot he upper left- hand corner of tile (in pixels)
                    int startBoundaryX, 
                    int startBoundaryY, 

                    int endBoundaryX, 
                    int endBoundaryY)
        {
            this.TheTexture = tTexture;

            this.ArrayX = arrayX;
            this.ArrayY = arrayY;

            this.Width = width;
            this.Height = height;

            this.StartBoundaryX = startBoundaryX;
            this.StartBoundaryY = startBoundaryY;

            // Setting these to 0 just to initialize them but the Board class will alter these.
            this.BoundingRectangle.X = 0;
            this.BoundingRectangle.Y = 0;

            this.BoundingRectangle.Width = this.Width;
            this.BoundingRectangle.Height = this.Height;
        } // End constructor.
    } // End class.
}
