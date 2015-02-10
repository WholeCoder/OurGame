using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OurGame.OurGameLibrary
{
    public class Tile
    {
        // This will be changed by the Board class to allow a check to see if something (ex: Player) hits a Tile on the board.
        public Rectangle BoundingRectangle;

        public Tile(Texture2D tTexture,
            int arrayX, // array position column   DON noT FORGET THESE ARE BACKWARDS IN ARRAY!!
            int arrayY, //                row

            int width, // in pixels
            int height,

            // in pixels relative tot he upper left- hand corner of tile (in pixels)
            int startBoundaryX,
            int startBoundaryY,
            int endBoundaryX,
            int endBoundaryY)
        {
            TheTexture = tTexture;

            ArrayX = arrayX;
            ArrayY = arrayY;

            Width = width;
            Height = height;

            StartBoundaryX = startBoundaryX;
            StartBoundaryY = startBoundaryY;

            // Setting these to 0 just to initialize them but the Board class will alter these.
            BoundingRectangle.X = 0;
            BoundingRectangle.Y = 0;

            BoundingRectangle.Width = Width;
            BoundingRectangle.Height = Height;
        }

        public Texture2D TheTexture { get; set; } // null denotes an empty tile.
        // These are in pixels.
        public int Width { get; set; }
        public int Height { get; set; }
        // These are the co-ordinates in the board's array;
        private int ArrayX { get; set; }
        private int ArrayY { get; set; }
        // These next co-ordinates specify a line that the character will stand on.  Right now
        // it will just be a straight horizontal line but in the future we mihgt give it an
        // incline or decline.
        private int StartBoundaryX { get; set; }
        private int StartBoundaryY { get; set; }
        public int EndBoundaryX { get; set; }
        public int EndBoundaryY { get; set; }

        public override string ToString()
        {
            return "Tile";
        }
    } // End class.
}