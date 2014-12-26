using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using WindowsGameLibrary1;

namespace Command
{
    public class PlaceMultiTextureOnBoardCommand : Command
    {
        Board gameBoard;

        // these co-ordinates are array indices
        private int putX;
        private int putY;
        private Texture2D putTexture;

        // these co-ordinates are array indices
        private int undoX;
        private int undoY;
        private Texture2D[,] undoTextures;

        // these variables are the how may textures that should be put down onto the game board array
        private int numberOfHorizontalTiles;
        private int numberOfVerticalTiles;

        public PlaceMultiTextureOnBoardCommand(Board pBoard, int mouseX, int mouseY, Texture2D tex, int screenXOffset, int numberOfHorizontalTiles, int numberOfVerticalTiles)
        {
            // Do some calcs with board.
            this.gameBoard = pBoard;
            this.putY = this.gameBoard.CalculateYIndex(mouseY);
            this.putX = this.gameBoard.CalculateXIndex(mouseX, screenXOffset);
            this.putTexture = tex;

            this.numberOfHorizontalTiles = numberOfHorizontalTiles;
            this.numberOfVerticalTiles = numberOfVerticalTiles;

            // Save all the textures that we are about to blow away.
            this.undoTextures = new Texture2D[this.numberOfVerticalTiles, this.numberOfHorizontalTiles];
            for (int i = 0; i < this.undoTextures.GetLength(0); i++)
            {
                for (int j = 0; j < this.undoTextures.GetLength(1); j++)
                {
                    int rowIndex = i + putY;
                    int columnIndex = j + putX;

                    this.undoTextures[i,j] = null;
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < this.gameBoard.TheBoard.GetLength(0) && columnIndex < this.gameBoard.TheBoard.GetLength(1))
                    {
                        this.undoTextures[i,j] = this.gameBoard.GetTextureAt(rowIndex, columnIndex);
                    }
                }
            }

            this.undoX = putX;
            this.undoY = putY;
        }

        public void execute()
        {
            for (int i = 0; i < this.numberOfVerticalTiles; i++)
            {
                for (int j = 0; j < this.numberOfHorizontalTiles; j++)
                {
                    int columnIndex= putX + j;
                    int rowIndex = putY + i;
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < this.gameBoard.TheBoard.GetLength(0) && columnIndex < this.gameBoard.TheBoard.GetLength(1))
                    {
                        this.gameBoard.putTextureOntoBoard(this.putTexture, rowIndex, columnIndex);
                    } // end if
                } // end for
            } // end outer for
        } // end method

        public void undo()
        {
            for (int i = 0; i < this.undoTextures.GetLength(0); i++)
            {
                for (int j = 0; j < this.undoTextures.GetLength(1); j++)
                {
                    int rowIndex = i + putY;
                    int columnIndex = j + putX;

                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < this.gameBoard.TheBoard.GetLength(0) && columnIndex < this.gameBoard.TheBoard.GetLength(1))
                    {
                        this.gameBoard.putTextureOntoBoard(this.undoTextures[i,j], rowIndex, columnIndex);
                    }
                }
            } // end outer for
            
        } // end method
    } // end class
} // end namespace
