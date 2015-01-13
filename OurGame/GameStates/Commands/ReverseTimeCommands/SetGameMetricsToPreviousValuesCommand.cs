using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;


// My usings.
using OurGame.Sprites;
using OurGame.GameStates;

namespace OurGame.Commands.ReverseTimeCommands
{
    public class SetGameMetricsToPreviousValuesCommand : ICommand
    {
        public PlayGameState _PlayGameState;
        public int _ScreenOffset;
        public Vector2 _CurrentPosition;
        public AnimatedSprite _Player;

        public SetGameMetricsToPreviousValuesCommand(PlayGameState pGameState, int screenOffset, AnimatedSprite player)
        {
            Debug.Assert(pGameState != null, "pGameState can't be null!");
            Debug.Assert(player != null, "player can't be null!");

            this._PlayGameState = pGameState;
            this._ScreenOffset = screenOffset;
            this._CurrentPosition = new Vector2(player.CurrentPosition.X, player.CurrentPosition.Y);
            this._Player = player;
        }

        public void Execute()
        {
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
