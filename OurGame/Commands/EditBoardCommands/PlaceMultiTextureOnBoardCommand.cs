using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using OurGame.OurGameLibrary;

namespace OurGame.Commands.EditBoardCommands
{
    public class PlaceMultiTextureOnBoardCommand : ICommand
    {
        private readonly Board _gameBoard;
        // these variables are the how may textures that should be put down onto the game board array
        private readonly int _numberOfHorizontalTiles;
        private readonly int _numberOfVerticalTiles;
        private readonly Texture2D _putTexture;
        // these co-ordinates are array indices
        private readonly int _putX;
        private readonly int _putY;
        private readonly Texture2D[,] _undoTextures;

        public override string ToString()
        {
            return "PlaceMultiTextureOnBoardCommand\n\t_numberOfHorizontalTiles == " + _numberOfHorizontalTiles + "\n\t_numberOfVerticalTiles == "+_numberOfVerticalTiles;
        }

        public PlaceMultiTextureOnBoardCommand(Board pBoard, int mouseX, int mouseY, Texture2D tex, int screenXOffset,
            int numberOfHorizontalTiles, int numberOfVerticalTiles)
        {
            Debug.Assert(pBoard != null, "pBoard can't be null!");
            // tex can be null!

            // Do some calcs with board.
            _gameBoard = pBoard;
            _putY = _gameBoard.CalculateYIndex(mouseY);
            _putX = _gameBoard.CalculateXIndex(mouseX, screenXOffset);
            _putTexture = tex;

            _numberOfHorizontalTiles = numberOfHorizontalTiles;
            _numberOfVerticalTiles = numberOfVerticalTiles;

            // Save all the textures that we are about to blow away.
            _undoTextures = new Texture2D[_numberOfVerticalTiles, _numberOfHorizontalTiles];
            for (var i = 0; i < _undoTextures.GetLength(0); i++)
            {
                for (var j = 0; j < _undoTextures.GetLength(1); j++)
                {
                    var rowIndex = i + _putY;
                    var columnIndex = j + _putX;

                    _undoTextures[i, j] = null;
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < _gameBoard.TheBoard.GetLength(0) &&
                        columnIndex < _gameBoard.TheBoard.GetLength(1))
                    {
                        _undoTextures[i, j] = _gameBoard.GetTextureAt(rowIndex, columnIndex);
                    }
                }
            }
        }

        public void Execute()
        {
            for (var i = 0; i < _numberOfVerticalTiles; i++)
            {
                for (var j = 0; j < _numberOfHorizontalTiles; j++)
                {
                    var columnIndex = _putX + j;
                    var rowIndex = _putY + i;
                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < _gameBoard.TheBoard.GetLength(0) &&
                        columnIndex < _gameBoard.TheBoard.GetLength(1))
                    {
                        _gameBoard.PutTextureOntoBoard(_putTexture, rowIndex, columnIndex);
                    } // end if
                } // end for
            } // end outer for
        } // end method

        public void Undo()
        {
            for (var i = 0; i < _undoTextures.GetLength(0); i++)
            {
                for (var j = 0; j < _undoTextures.GetLength(1); j++)
                {
                    var rowIndex = i + _putY;
                    var columnIndex = j + _putX;

                    if (rowIndex + i >= 0 && columnIndex >= 0 && rowIndex < _gameBoard.TheBoard.GetLength(0) &&
                        columnIndex < _gameBoard.TheBoard.GetLength(1))
                    {
                        _gameBoard.PutTextureOntoBoard(_undoTextures[i, j], rowIndex, columnIndex);
                    }
                }
            } // end outer for
        } // end method
    } // end class
} // end namespace