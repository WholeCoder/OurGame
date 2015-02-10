using System;
using System.Diagnostics;
using System.IO;
using OurGame.GameStates;
using OurGame.OurGameLibrary;

namespace OurGame.Commands.EditBoardCommands
{
    public class DeleteBoardCommand : ICommand
    {
        private readonly Board _board;
        private readonly EditBoardState _editBoardState;
        private readonly String _pathToSavedGambeBoardConfigurationFile;

        public override string ToString()
        {
            return "DeleteBoardCommand - _pathToSavedGambeBoardConfigurationFile == " +
                   _pathToSavedGambeBoardConfigurationFile;
        }

        public DeleteBoardCommand(String pathToSavedGameBoardConfigurationFile, Board board,
            EditBoardState editBoardState)
        {
            Debug.Assert(
                !pathToSavedGameBoardConfigurationFile.Equals("") && pathToSavedGameBoardConfigurationFile != null,
                "pathToSavedGameBoardConfigurationFile is null or empty string!");
            Debug.Assert(board != null, "board is NULL!");
            Debug.Assert(editBoardState != null, "editBoardState is NULL!");

            _pathToSavedGambeBoardConfigurationFile = pathToSavedGameBoardConfigurationFile;
            _board = board;

            _editBoardState = editBoardState;
        }

        private Tile[,] TheUndoBoard { get; set; }

        public void Execute()
        {
            TheUndoBoard = new Tile[_board.TheBoard.GetLength(0), _board.TheBoard.GetLength(1)];
            for (var i = 0; i < TheUndoBoard.GetLength(0); i++)
            {
                for (var j = 0; j < TheUndoBoard.GetLength(1); j++)
                {
                    TheUndoBoard[i, j] = _board.TheBoard[i, j];
                    _board.TheBoard[i, j] = null;
                }
            }

            if (File.Exists(_pathToSavedGambeBoardConfigurationFile))
            {
                File.Delete(_pathToSavedGambeBoardConfigurationFile);
            }

            _board.ReadInBoardConfigurationOrUseDefault(_pathToSavedGambeBoardConfigurationFile);
        }

        public void Undo()
        {
            for (var i = 0; i < TheUndoBoard.GetLength(0); i++)
            {
                for (var j = 0; j < TheUndoBoard.GetLength(1); j++)
                {
                    _board.TheBoard[i, j] = TheUndoBoard[i, j];
                }
            }

            _editBoardState.SaveCurrentBoard();
        }
    }
}