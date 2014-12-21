using System;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

using WindowsGameLibrary1;

namespace Command
{
    public class PlaceTileOnBoardCommand : Command
    {
        Board gameBoard;

        // these co-ordinates are array indices
        private int putX;
        private int putY;
        private Texture2D putTexture;

        // these co-ordinates are array indices
        private int undoX;
        private int undoY;
        private Texture2D undoTexture;

        public PlaceTileOnBoardCommand(Board pBoard, int mouseX, int mouseY, Texture2D tex, int screenXOffset)
        {
            // Do some calcs with board.
            this.gameBoard = pBoard;
            this.putY = this.gameBoard.CalculateYIndex(mouseY);
            this.putX = this.gameBoard.CalculateXIndex(mouseX, screenXOffset);
            this.putTexture = tex;

            this.undoTexture = this.gameBoard.GetTextureAt(putY, putX);
            this.undoX = putX;
            this.undoY = putY;
        }

        public void execute()
        {
            this.gameBoard.putTextureOntoBoard(this.putTexture, putY, putX);
        }

        public void undo()
        {
            this.gameBoard.putTextureOntoBoard(this.undoTexture, undoY, undoX);
        }
    }
}
