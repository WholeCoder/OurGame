using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.WindowsGame1;
using OurGame.MenuComponents;
using OurGame.OurGameLibrary;

namespace OurGame.GameStates
{
    class HelpMenuState : State
    {
        private KeyboardState _OldKeyboardState;
        private SpriteFont _HelpFont;
        private MenuComponent _Menu;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public HelpMenuState()
        {

        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            this._HelpFont = Content.Load<SpriteFont>(@"fonts\helpfont");

            string menuMessage = "Game Board Edit Help";
            float offsetY = this._HelpFont.MeasureString(menuMessage).Y;
            this._Menu= this.BuildMenuComponent(menuMessage, offsetY);

            menuMessage = "Controls - Game Board Edit Mode"   ;
            offsetY += this._HelpFont.MeasureString(menuMessage).Y;
            MenuComponent editGameBoardTitle = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(editGameBoardTitle);

            menuMessage = "mouse scroll wheel - Makes the brush bigger or smaller."   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent mouseScrollEditMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(mouseScrollEditMenuItem);

            menuMessage = "left keyboard arrow/right keyboard arrow - Use to scroll the board."   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent leftRightArrowKeysEditModeMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(leftRightArrowKeysEditModeMenuItem);

            menuMessage = "left mouse button - Place a tile on the game board."   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent leftMouseButtonMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(leftMouseButtonMenuItem);

            menuMessage = "right mouse button - Change to next texture under the mouse brush cursor.";
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent rightMouseButtonEditMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(rightMouseButtonEditMenuItem);

            menuMessage = "press Z key - Undo placing a tile on the game board grid"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressZEditMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(pressZEditMenuItem);

            menuMessage = "press S key - Save the game board's configuration (MyLevel.txt)";   
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressSEditMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(pressSEditMenuItem);

            menuMessage = "press D key - Reset the game board and write out a 'blank' MyLevel.txt."   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressDEditMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(pressDEditMenuItem);

            menuMessage = "press PageUp - increase the size of the mouse's brush."   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPageUpEditMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(pressPageUpEditMenuItem);

            menuMessage = "press PageDown - Decrease the size of the mouse's brush."   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPageDownEditMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(pressPageDownEditMenuItem);

            menuMessage = "press Q key - Quit the editor."   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressQEditMenuItem= this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(pressQEditMenuItem);

            menuMessage = "press U key - Undo a whole board delete (pressed D previously)."  ; 
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressUMenuItem = this.BuildMenuComponent(menuMessage,offsetY);
            this._Menu.AddMenuComponents(pressUMenuItem);

        }

        private MenuComponent BuildMenuComponent(string title, float y)
        {
            return new MenuComponent(title, new Vector2((Board.SCREEN_WIDTH - _HelpFont.MeasureString(title).X) / 2, y));
        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _OldKeyboardState, this.OurGame, gameTime);

            _OldKeyboardState = newKeyboardState;  // set the new state as the old state for next time
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this._Menu.Draw(spriteBatch,this._HelpFont);
        }
    }
}
