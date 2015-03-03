using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OurGame.GameStates;
using OurGame.OurGameLibrary;
using OurGame.Sprites;

namespace OurGame.Commands.EditSpritesCommands
{
    class DeleteAllSprites: ICommand
    {
        private readonly string _nameOfSpritesFile;
        private readonly Board _board ;
        private readonly State _state;

        public DeleteAllSprites(string nameOfSpritesFile, Board board, State state)
        {
            _nameOfSpritesFile = nameOfSpritesFile;
            _board = board;
            _state = state;
        }

        public void ExecuteDelete(ref SpriteManager sManager)
        {
            if (File.Exists(_nameOfSpritesFile))
            {
                File.Delete(_nameOfSpritesFile);
            }

            sManager = new SpriteManager(_nameOfSpritesFile, _board, _state);
            sManager.WriteOutSpritesToAfile();
        }

        public void Execute()
        {
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
