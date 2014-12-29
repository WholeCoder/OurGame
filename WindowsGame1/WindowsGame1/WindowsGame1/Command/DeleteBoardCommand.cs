using System;
using System.IO;

// My usings.
using GameState;
using WindowsGameLibrary1;

namespace Command
{
    public class DeleteBoardCommand : Command
    {
        String pathToSavedGambeBoardConfigurationFile;
        TextureCache tCache;
        Board board;

        public Tile[,] TheUndoBoard { get; set; }

        EditBoardState editBoardState;

        public DeleteBoardCommand(String pathToSavedGambeBoardConfigurationFile, TextureCache tCache, Board board, EditBoardState editBoardState)
        {
            this.pathToSavedGambeBoardConfigurationFile = pathToSavedGambeBoardConfigurationFile;
            this.tCache = tCache;
            this.board = board;

            this.editBoardState = editBoardState;
        }

        public void execute()
        {
            this.TheUndoBoard = new Tile[this.board.TheBoard.GetLength(0), this.board.TheBoard.GetLength(1)];
            for (int i = 0; i < this.TheUndoBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.TheUndoBoard.GetLength(1); j++)
                {
                    this.TheUndoBoard[i, j] = this.board.TheBoard[i, j];
                    this.board.TheBoard[i, j] = null;
                }
            }

            if (File.Exists(pathToSavedGambeBoardConfigurationFile))
            {
                File.Delete(pathToSavedGambeBoardConfigurationFile);
            }

            this.board.ReadInBoardConfigurationOrUseDefault(pathToSavedGambeBoardConfigurationFile, tCache);
        }

        public void undo()
        {
            for (int i = 0; i < this.TheUndoBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.TheUndoBoard.GetLength(1); j++)
                {
                    this.board.TheBoard[i, j] = this.TheUndoBoard[i, j];
                }
            }

            this.editBoardState.SaveCurrentBoard();
        }
    }
}
