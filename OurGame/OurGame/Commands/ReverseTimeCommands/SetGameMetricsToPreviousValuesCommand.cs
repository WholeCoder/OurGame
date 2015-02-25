using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using OurGame.GameStates;
using OurGame.Sprites;

namespace OurGame.Commands.ReverseTimeCommands
{
    public class SetGameMetricsToPreviousValuesCommand : ICommand
    {
        private readonly AnimatedSprite _player;
        private readonly PlayGameState _playGameState;
        private readonly int _screenOffset;
        private Vector2 _currentPosition;

        public SetGameMetricsToPreviousValuesCommand(PlayGameState pGameState, int screenOffset, AnimatedSprite player)
        {
            Debug.Assert(pGameState != null, "pGameState can't be null!");
            Debug.Assert(player != null, "player can't be null!");

            _playGameState = pGameState;
            _screenOffset = screenOffset;
            _currentPosition = new Vector2(player.CurrentPosition.X, player.CurrentPosition.Y);
            _player = player;
        }

        public void Execute()
        {
            _playGameState.ScreenXOffset = _screenOffset;
            _player.CurrentPosition.X = _currentPosition.X;
            _player.CurrentPosition.Y = _currentPosition.Y;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "SetGameMetricsToPreviousValuesCommand\n\t_currentPosition == (" + _currentPosition.X + ", " +
                   _currentPosition.Y + ")\n\t_screenOffset == " + _screenOffset;
        }
    }
}