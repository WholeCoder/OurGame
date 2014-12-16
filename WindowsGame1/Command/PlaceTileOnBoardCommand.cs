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

        public PlaceTileOnBoardCommand(Texture2D[,] pBoard)
        {
            this.gameBoard = pBoard;
            this.putX = -1;
            this.putY = -1;
            this.putTexture = null;

            this.undoTexture = null;
            this.undoX = -1;
            this.undoY = -1;
        }

        // MUST call this method before execute is called!
        public void setTilePositionAndTextureInArrayCoordinates(int x, int y, Texture2D tex)
        {
            this.putX = x;
            this.putY = y;

            this.undoX = putX;
            this.undoY = putY;
            this.undoTexture = this.gameBoard[putY, putX];

            this.putTexture = tex;
        }

        public void execute()
        {
            if (putX == -1)
            {
                throw new SetTilePositionAndTextureInArrayCoordinatesNetCalledFirstException();
            }
            
            this.gameBoard[putY, putX] = this.putTexture;
        }

        public void undo()
        {
            if (putX == -1)
            {
                throw new SetTilePositionAndTextureInArrayCoordinatesNetCalledFirstException();
            }

            this.gameBoard[undoY, undoX] = this.undoTexture;
        }
    }
}
