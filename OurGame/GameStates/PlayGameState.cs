using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Sprites;
using OurGame.Commands;
using OurGame.WindowsGame1;
using OurGame.OurGameLibrary;
using OurGame.Commands.ReverseTimeCommands;
using OurGame.Sprites.SpriteObserver;

// Created by someone else.
using ParticleEffects;

namespace OurGame.GameStates
{
    public class PlayGameState : State, SpriteObserver
    {
        readonly EffectManager myEffectsManager;
        int _keyboardDelayCounter = 0;

        bool _fireIsRunning = false;
        bool _fireworksAreRunning = false;
        bool _snowIsFalling = false;
        bool _smokeIsRunning = false;
        bool _spiralIsRunning = false;

        Board _board;
        
        // This instance variable lets us scroll the board horizontally.
        public int ScreenXOffset = 0;
        readonly int scrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        readonly string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";

        KeyboardState _oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        private Game1 OurGame { get; set; }

        private AnimatedSprite Player { get; set; }
        private SpriteManager _spriteManager;

        private Stack<OurGame.Commands.ICommand> _ReversePositionAndScreenOffsetStackOfCommands;
        private Vector2 _previousPlayerPosition = new Vector2(-1.0f, -1.0f);

        private int _playerLife;
        private SpriteFont _helpFont;

        public PlayGameState()
        {
            myEffectsManager = new EffectManager();
        }

        // this is to implement the SpriteObserver interface.
        public void updateObserver(int life)
        {
            this._playerLife = life;
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can not be null!");

            this.OurGame = ourGame;
            this._ReversePositionAndScreenOffsetStackOfCommands = new Stack<OurGame.Commands.ICommand>();
        }

        protected override void LoadStatesContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            _board = new Board(pathToSavedGambeBoardConfigurationFile);

            this._spriteManager = new SpriteManager("MyLevelsEnemySpritesList.txt", _board, this);

            Player = new UserControlledSprite("UserControlledSpriteConfig.txt", _board, this);

            this._playerLife = Player.LifeLeft;
            this.Player.RegisterObserver(this);

            this._helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");

            myEffectsManager.LoadContent(Content);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            Player.Update(gameTime);
            this._spriteManager.Update(gameTime);

            // These next 2 statements make sure, if the sprite hits the ground, it will not go through the ground.
            // (On the closest tile that is below the sprite).
            // SetSpritePositionIfIntersectingWithGroundOrPlatform possibly modifies the sprites sent in as parameters.
            SetSpritePositionIfIntersectingWithGroundOrPlatform(Player);
            for (int i = 0; i < this._spriteManager.Sprites.Length; i++)
            {
                SetSpritePositionIfIntersectingWithGroundOrPlatform(this._spriteManager.Sprites[i]);
            }
            
            if (this._previousPlayerPosition.X != Player.CurrentPosition.X || this._previousPlayerPosition.Y != Player.CurrentPosition.Y)
            {
                SetGameMetricsToPreviousValuesCommand sCommand = new SetGameMetricsToPreviousValuesCommand(this, ScreenXOffset, Player);
                SetGameMetricsToPreviousValuesCommand topOfStack = null;

                if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
                {
                    topOfStack = (SetGameMetricsToPreviousValuesCommand)this._ReversePositionAndScreenOffsetStackOfCommands.Peek();
                }

                if (topOfStack == null ||
                    topOfStack.CurrentPosition.X != Player.CurrentPosition.X ||
                    topOfStack.CurrentPosition.Y != Player.CurrentPosition.Y ||
                    topOfStack.ScreenOffset != ScreenXOffset)
                {
                    this._ReversePositionAndScreenOffsetStackOfCommands.Push(sCommand);
                    this._previousPlayerPosition.X = Player.CurrentPosition.X;
                    this._previousPlayerPosition.Y = Player.CurrentPosition.Y;
                }
            }

            // Move game board.
            KeyboardState keyState = Keyboard.GetState();

            if (_keyboardDelayCounter > 0)
            {
                _keyboardDelayCounter -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_fireworksAreRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.explosion);
                    _keyboardDelayCounter = 300;
                    _fireworksAreRunning = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !_fireIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.fire);
                    _keyboardDelayCounter = 300;
                    _fireIsRunning = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && !_snowIsFalling)
                {
                    //myEffectsManager.AddEffect(eEffectType.snow);
                    _keyboardDelayCounter = 300;
                    _snowIsFalling = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !_smokeIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.smoke);
                    _keyboardDelayCounter = 300;
                    _smokeIsRunning = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !_spiralIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.spiral);
                    _keyboardDelayCounter = 300;
                    _spiralIsRunning = true;
                }
            }

            myEffectsManager.Update(gameTime);


            if (keyState.IsKeyDown(Keys.Right))
            {
                SetGameMetricsToPreviousValuesCommand topStack = null;
                if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
                {
                    topStack= (SetGameMetricsToPreviousValuesCommand)this._ReversePositionAndScreenOffsetStackOfCommands.Peek();

                }

                if (topStack== null ||
                    topStack.CurrentPosition.X != Player.CurrentPosition.X ||
                    topStack.CurrentPosition.Y != Player.CurrentPosition.Y ||
                    topStack.ScreenOffset != ScreenXOffset)
                {
                    SetGameMetricsToPreviousValuesCommand sCommand = new SetGameMetricsToPreviousValuesCommand(this, ScreenXOffset, Player);
                    this._ReversePositionAndScreenOffsetStackOfCommands.Push(sCommand);

                }
                ScreenXOffset -= scrollAmount;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                SetGameMetricsToPreviousValuesCommand topStack = null;
                if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
                {
                    topStack = (SetGameMetricsToPreviousValuesCommand)this._ReversePositionAndScreenOffsetStackOfCommands.Peek();

                }

                if (topStack == null ||
                    topStack.CurrentPosition.X != Player.CurrentPosition.X ||
                    topStack.CurrentPosition.Y != Player.CurrentPosition.Y ||
                    topStack.ScreenOffset != ScreenXOffset)
                {
                    SetGameMetricsToPreviousValuesCommand sCommand = new SetGameMetricsToPreviousValuesCommand(this, ScreenXOffset, Player);
                    this._ReversePositionAndScreenOffsetStackOfCommands.Push(sCommand);

                }
                ScreenXOffset += scrollAmount;
            }
            


            if (ScreenXOffset <= -this._board.BoardWidth+Board.SCREEN_WIDTH)
            {
                ScreenXOffset = -this._board.BoardWidth+Board.SCREEN_WIDTH;
            }

            if (ScreenXOffset >= 0)
            {
                ScreenXOffset = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
                {
                    ICommand sCommand = this._ReversePositionAndScreenOffsetStackOfCommands.Pop();
                    sCommand.Execute();
                }
            }
            else
            {
                // "Gravity" to pull down the Player if it is in mid-air.
                Player.ApplyDownwardGravity();
            }

            // "Gravity" to pull down the enemies if they are in mid-air.
            this._spriteManager.ApplyDownwordGravity();

            List<AnimatedSprite> enemiesTouchingPlayer = this._spriteManager.GetSpritesThatPlayerCollidedWith(Player);
            if (enemiesTouchingPlayer.Count > 0)
            {
                Player.DecreaseSpriteLife(1);
            }

            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, this.OurGame, gameTime);

            _oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        private void SetSpritePositionIfIntersectingWithGroundOrPlatform(AnimatedSprite sSprite)
        {
            Debug.Assert(sSprite != null, "sSprite can not be null!");

            
            // Get all tiles, on the screen, that intersect with our sprite.
            List<Tile> tilesThatHaveCollisionWithSprite = this._board.RetrieveTilesThatIntersectWithThisSprite(sSprite, ScreenXOffset);
            
            // if we are not in mid air then find the tile that interesects our sprite with the least Y co-ordinate.
            if (tilesThatHaveCollisionWithSprite.Count != 0)
            {
                int leastY = this._board.BoardHeight;
                leastY = tilesThatHaveCollisionWithSprite.Select(tile => tile.BoundingRectangle.Y).Concat(new[] {leastY}).Min();

                // Now make the sprite stay on that tile that was found.
                sSprite.CurrentPosition.Y = leastY - sSprite.BoundingRectangle.Height;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            this._board.DrawBoard(spriteBatch, ScreenXOffset, false);  // screenXOffset scrolls the board left and right!
            Player.Draw(spriteBatch);
            this._spriteManager.Draw(spriteBatch);
            myEffectsManager.Draw(spriteBatch);
            spriteBatch.DrawString(this._helpFont, "Life:  "+this._playerLife,
                       new Vector2(10, 10), Color.Black, 0, Vector2.Zero,
                       1, SpriteEffects.None, 1);

        }
    }
}
