﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Sprites;
using OurGame.Commands;
using OurGame.WindowsGame1;
using OurGame.OurGameLibrary;
using OurGame.Commands.ReverseTimeCommands;

// Created by someone else.
using ParticleEffects;

namespace OurGame.GameStates
{
    public class PlayGameState : State
    {
        EffectManager myEffectsManager;
        int keyboardDelayCounter = 0;

        bool fireIsRunning = false;
        bool fireworksAreRunning = false;
        bool snowIsFalling = false;
        bool smokeIsRunning = false;
        bool spiralIsRunning = false;

        Board board;
        
        // This instance variable lets us scroll the board horizontally.
        public int screenXOffset = 0;
        int scrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";

        KeyboardState oldKeyboardState;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public AnimatedSprite Player { get; set; }
        public AnimatedSprite Enemy { get; set; }

        private Stack<OurGame.Commands.ICommand> _ReversePositionAndScreenOffsetStackOfCommands;
        private Vector2 _PreviousPlayerPosition = new Vector2(-1.0f, -1.0f);

        public PlayGameState()
        {
            myEffectsManager = new EffectManager();
        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
            this._ReversePositionAndScreenOffsetStackOfCommands = new Stack<OurGame.Commands.ICommand>();
        }

        protected override void LoadStatesContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            board = new Board(pathToSavedGambeBoardConfigurationFile);

            // TODO:  Create the "UserControlledSpriteConfig.txt" file or make the class create it if not found.
            Player = new UserControlledSprite("UserControlledSpriteConfig.txt", board, this);
            Enemy = new AutomatedSprite("AutomatedSpriteConfig.txt", this);

            myEffectsManager.LoadContent(Content);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Player.Update(gameTime);
            Enemy.Update(gameTime);

            List<Tile> tilesThatHaveCollisionWithSprite = this.board.RetrieveTilesThatIntersectWithThisSprite(this.Player, screenXOffset);
            if (tilesThatHaveCollisionWithSprite.Count != 0)
            {
                int leastY = this.board.BoardHeight;
                foreach (var tile in tilesThatHaveCollisionWithSprite)
                {
                    if (tile.BoundingRectangle.Y < leastY)
                    {
                        leastY = tile.BoundingRectangle.Y;
                    }
                }
                this.Player.CurrentPosition.Y = leastY - this.Player.BoundingRectangle.Height;
            }

            if (this._PreviousPlayerPosition.X != Player.CurrentPosition.X || this._PreviousPlayerPosition.Y != Player.CurrentPosition.Y)
            {
                SetGameMetricsToPreviousValuesCommand sCommand = new SetGameMetricsToPreviousValuesCommand(this, screenXOffset, Player);
                SetGameMetricsToPreviousValuesCommand topOfStack = null;

                if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
                {
                    topOfStack = (SetGameMetricsToPreviousValuesCommand)this._ReversePositionAndScreenOffsetStackOfCommands.Peek();
                }

                if (topOfStack == null ||
                    topOfStack._CurrentPosition.X != Player.CurrentPosition.X ||
                    topOfStack._CurrentPosition.Y != Player.CurrentPosition.Y ||
                    topOfStack._ScreenOffset != screenXOffset)
                {
                    this._ReversePositionAndScreenOffsetStackOfCommands.Push(sCommand);
                    this._PreviousPlayerPosition.X = Player.CurrentPosition.X;
                    this._PreviousPlayerPosition.Y = Player.CurrentPosition.Y;
                }
            }

            // Move game board.
            KeyboardState keyState = Keyboard.GetState();

            if (keyboardDelayCounter > 0)
            {
                keyboardDelayCounter -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && !fireworksAreRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.explosion);
                    keyboardDelayCounter = 300;
                    fireworksAreRunning = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && !fireIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.fire);
                    keyboardDelayCounter = 300;
                    fireIsRunning = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && !snowIsFalling)
                {
                    //myEffectsManager.AddEffect(eEffectType.snow);
                    keyboardDelayCounter = 300;
                    snowIsFalling = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && !smokeIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.smoke);
                    keyboardDelayCounter = 300;
                    smokeIsRunning = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !spiralIsRunning)
                {
                    //myEffectsManager.AddEffect(eEffectType.spiral);
                    keyboardDelayCounter = 300;
                    spiralIsRunning = true;
                }
            }

            myEffectsManager.Update(gameTime);


            if (!this.board.IsThereACollisionWith(Player, screenXOffset))
            {
                if (keyState.IsKeyDown(Keys.Right))
                {
                    SetGameMetricsToPreviousValuesCommand topStack = null;
                    if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
                    {
                        topStack= (SetGameMetricsToPreviousValuesCommand)this._ReversePositionAndScreenOffsetStackOfCommands.Peek();

                    }

                    if (topStack== null ||
                        topStack._CurrentPosition.X != Player.CurrentPosition.X ||
                        topStack._CurrentPosition.Y != Player.CurrentPosition.Y ||
                        topStack._ScreenOffset != screenXOffset)
                    {
                        SetGameMetricsToPreviousValuesCommand sCommand = new SetGameMetricsToPreviousValuesCommand(this, screenXOffset, Player);
                        this._ReversePositionAndScreenOffsetStackOfCommands.Push(sCommand);

                    }
                    screenXOffset -= scrollAmount;
                }

                if (keyState.IsKeyDown(Keys.Left))
                {
                    SetGameMetricsToPreviousValuesCommand topStack = null;
                    if (this._ReversePositionAndScreenOffsetStackOfCommands.Count > 0)
                    {
                        topStack = (SetGameMetricsToPreviousValuesCommand)this._ReversePositionAndScreenOffsetStackOfCommands.Peek();

                    }

                    if (topStack == null ||
                        topStack._CurrentPosition.X != Player.CurrentPosition.X ||
                        topStack._CurrentPosition.Y != Player.CurrentPosition.Y ||
                        topStack._ScreenOffset != screenXOffset)
                    {
                        SetGameMetricsToPreviousValuesCommand sCommand = new SetGameMetricsToPreviousValuesCommand(this, screenXOffset, Player);
                        this._ReversePositionAndScreenOffsetStackOfCommands.Push(sCommand);

                    }
                    screenXOffset += scrollAmount;
                }
            }


            if (screenXOffset <= -this.board.BoardWidth)
            {
                screenXOffset = -this.board.BoardWidth;
            }

            if (screenXOffset >= 0)
            {
                screenXOffset = 0;
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
                // This next operation makes sure the character falls down to a new floor tile when it walks.
                Player.CurrentPosition.Y += UserControlledSprite.GRAVITY_DOWNWARD;
            }

            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, oldKeyboardState, this.OurGame, gameTime);

            oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.board.DrawBoard(spriteBatch, screenXOffset, false);  // screenXOffset scrolls the board left and right!
            Player.Draw(spriteBatch);
            Enemy.Draw(spriteBatch);
            myEffectsManager.Draw(spriteBatch);
        }
    }
}
