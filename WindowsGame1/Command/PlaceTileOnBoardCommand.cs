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


namespace Command
{
    public class PlaceTileOnBoardCommand : Command
    {
        private Texture2D[,] gameBoard;

        // these co-ordinates are array indices
        private int putX;
        private int putY;
        private Texture2D putTexture;

        // these co-ordinates are array indices
        private int undoX;
        private int undoY;
        private Texture2D undoTexture;

        public PlaceTileOnBoardCommand(Texture2D[,] pBoard, int x, int y, Texture2D tex)
        {
            this.gameBoard = pBoard;
            this.putX = x;
            this.putY = y;
            this.putTexture = tex;

            this.undoTexture = this.gameBoard[putY, putX]; ;
            this.undoX = putX;
            this.undoY = putY;
        }

        public void execute()
        {
            this.gameBoard[putY, putX] = this.putTexture;
        }

        public void undo()
        {
            this.gameBoard[undoY, undoX] = this.undoTexture;
        }
    }
}
