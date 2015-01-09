using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.OurGameLibrary;

namespace OurGame.Commands
{
    public class PlaceMultiTextureOnBoardCommand : ICommand
    {
        private Board _gameBoard;

        // these co-ordinates are array indices
        private int _putX;
        private int _putY;
        private Texture2D _putTexture;

        // these co-ordinates are array indices
        private int _undoX;
        private int _undoY;
        private Texture2D[,] _undoTextures;

        // these variables are the how may textures that should be put down onto the game board array
        private int _numberOfHorizontalTiles;
        private int _numberOfVerticalTiles;

        public PlaceMultiTextureOnBoardCommand(Board pBoard, int mouseX, int mouseY, Texture2D tex, int screenXOffset, int numberOfHorizontalTiles, int numberOfVerticalTiles)
        {
            Debug.Assert(pBoard != null, "pBoard can't be null!");

            // Do some calcs with board.
            this._gameBoard = pBoard;
            this._putY = this._gameBoard.CalculateYIndex(mouseY);
            this._putX = this._gameBoard.CalculateXIndex(mouseX, screenXOffset);
            this._putTexture = tex;

            this._numberOfHorizontalTiles = numberOfHorizontalTiles;
            this._numberOfVerticalTiles = numberOfVerticalTiles;

            // Save all the textures that we are about to blow away.
            this._undoTextures = new Texture2D[this._numberOfVerticalTiles, this._numberOfHorizontalTiles];
            for (int i = 0; i < this._undoTextures.GetLength(0); i++)
            {
                for (int j = 0; j < this._undoTextures.GetLength(1); j++)
                {
                    int rowIndex = i + _putY;
                    int columnIndex = j + _putX;

                    this._undoTextures[i,j] = null;
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < this._gameBoard.TheBoard.GetLength(0) && columnIndex < this._gameBoard.TheBoard.GetLength(1))
                    {
                        this._undoTextures[i,j] = this._gameBoard.GetTextureAt(rowIndex, columnIndex);
                    }
                }
            }

            this._undoX = _putX;
            this._undoY = _putY;
        }

        public void Execute()
        {
            for (int i = 0; i < this._numberOfVerticalTiles; i++)
            {
                for (int j = 0; j < this._numberOfHorizontalTiles; j++)
                {
                    int columnIndex= _putX + j;
                    int rowIndex = _putY + i;
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < this._gameBoard.TheBoard.GetLength(0) && columnIndex < this._gameBoard.TheBoard.GetLength(1))
                    {
                        this._gameBoard.PutTextureOntoBoard(this._putTexture, rowIndex, columnIndex);
                    } // end if
                } // end for
            } // end outer for
        } // end method

        public void Undo()
        {
            for (int i = 0; i < this._undoTextures.GetLength(0); i++)
            {
                for (int j = 0; j < this._undoTextures.GetLength(1); j++)
                {
                    int rowIndex = i + _putY;
                    int columnIndex = j + _putX;

                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < this._gameBoard.TheBoard.GetLength(0) && columnIndex < this._gameBoard.TheBoard.GetLength(1))
                    {
                        this._gameBoard.PutTextureOntoBoard(this._undoTextures[i,j], rowIndex, columnIndex);
                    }
                }
            } // end outer for
            
        } // end method
    } // end class
} // end namespace
