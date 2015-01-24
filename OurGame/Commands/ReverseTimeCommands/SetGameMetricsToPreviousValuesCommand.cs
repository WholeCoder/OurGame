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
        public readonly int ScreenOffset;
        public Vector2 CurrentPosition;

        private readonly PlayGameState _playGameState;
        private readonly AnimatedSprite _player;

        public SetGameMetricsToPreviousValuesCommand(PlayGameState pGameState, int screenOffset, AnimatedSprite player)
        {
            Debug.Assert(pGameState != null, "pGameState can't be null!");
            Debug.Assert(player != null, "player can't be null!");

            this._playGameState = pGameState;
            this.ScreenOffset = screenOffset;
            this.CurrentPosition = new Vector2(player.CurrentPosition.X, player.CurrentPosition.Y);
            this._player = player;
        }

        public void Execute()
        {
            this._playGameState.ScreenXOffset = this.ScreenOffset;
            this._player.CurrentPosition.X = this.CurrentPosition.X;
            this._player.CurrentPosition.Y = this.CurrentPosition.Y;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
