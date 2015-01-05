﻿using System;
using System.IO;

// My usings.
using OurGame.GameStates;
using OurGame.WindowsGameLibrary1;

namespace OurGame.Commands
{
    public class DeleteBoardCommand : ICommand
    {
        private String _pathToSavedGambeBoardConfigurationFile;
        private TextureCache _tCache;
        private Board _board;

        public Tile[,] TheUndoBoard { get; set; }

        private EditBoardState _editBoardState;

        public DeleteBoardCommand(String pathToSavedGambeBoardConfigurationFile, TextureCache tCache, Board board, EditBoardState editBoardState)
        {
            this._pathToSavedGambeBoardConfigurationFile = pathToSavedGambeBoardConfigurationFile;
            this._tCache = tCache;
            this._board = board;

            this._editBoardState = editBoardState;
        }

        public void Execute()
        {
            this.TheUndoBoard = new Tile[this._board.TheBoard.GetLength(0), this._board.TheBoard.GetLength(1)];
            for (int i = 0; i < this.TheUndoBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.TheUndoBoard.GetLength(1); j++)
                {
                    this.TheUndoBoard[i, j] = this._board.TheBoard[i, j];
                    this._board.TheBoard[i, j] = null;
                }
            }

            if (File.Exists(_pathToSavedGambeBoardConfigurationFile))
            {
                File.Delete(_pathToSavedGambeBoardConfigurationFile);
            }

            this._board.ReadInBoardConfigurationOrUseDefault(_pathToSavedGambeBoardConfigurationFile, _tCache);
        }

        public void Undo()
        {
            for (int i = 0; i < this.TheUndoBoard.GetLength(0); i++)
            {
                for (int j = 0; j < this.TheUndoBoard.GetLength(1); j++)
                {
                    this._board.TheBoard[i, j] = this.TheUndoBoard[i, j];
                }
            }

            this._editBoardState.SaveCurrentBoard();
        }
    }
}