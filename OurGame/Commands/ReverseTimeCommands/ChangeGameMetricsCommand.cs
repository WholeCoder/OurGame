using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


// My usings.
using OurGame.Sprites;
using OurGame.Commands;
using OurGame.GameStates;

namespace OurGame.Commands.ReverseTimeCommands
{
    public class SetGameMetricsToPreviousValuesCommand : ICommand
    {
        private PlayGameState _PlayGameState;
        private int _ScreenOffset;
        private Vector2 _CurrentPosition;
        private AnimatedSprite _Player;

        public SetGameMetricsToPreviousValuesCommand(PlayGameState pGameState, int screenOffset, AnimatedSprite player)
        {
            this._PlayGameState = pGameState;
            this._ScreenOffset = screenOffset;
            this._CurrentPosition = new Vector2(player.CurrentPosition.X, player.CurrentPosition.Y);
            this._Player = player;
        }

        public void Execute()
        {
            Console.WriteLine("-----------------XExecuting ChangGameMetrisCommand.(x,y) == ("+this._CurrentPosition.X+","+this._CurrentPosition.Y+")");
            this._PlayGameState.screenXOffset = this._ScreenOffset;
            this._Player.CurrentPosition.X = this._CurrentPosition.X;
            this._Player.CurrentPosition.Y = this._CurrentPosition.Y;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
