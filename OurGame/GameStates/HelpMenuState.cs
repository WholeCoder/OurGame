using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.WindowsGame1;
using OurGame.MenuComponents;
using OurGame.OurGameLibrary;

namespace OurGame.GameStates
{
    public class HelpMenuState : State
    {
        private KeyboardState _oldKeyboardState;
        private SpriteFont _helpFont;
        private MenuComponent _editScreenHelpMenu;
        private MenuComponent _generalHelpMenu;

        // These two constants control the way the command name and description line up on the screen.
        // ReSharper disable once InconsistentNaming
        private const int COMMAND_NAME_OFFSET = 100;
        // ReSharper disable once InconsistentNaming
        private const int COMMAND_DESCRIPTION_OFFSET = 450;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        public Game1 OurGame { get; set; }

        public HelpMenuState()
        {

        }

        public override void Initialize(Game1 ourGame)
        {
            this.OurGame = ourGame;
        }

        protected override void LoadStatesContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            this._helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");

            string menuMessage = "Game Board Edit Help";
            float offsetY = this._helpFont.MeasureString(menuMessage).Y;
            this._editScreenHelpMenu= this.BuildMenuComponent(menuMessage,"", offsetY);

            menuMessage = "Controls"   ;
            offsetY += this._helpFont.MeasureString(menuMessage).Y;
            MenuComponent editGameBoardTitle = this.BuildMenuComponent(menuMessage, "Game Board Edit Mode", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(editGameBoardTitle);

            menuMessage = "mouse scroll wheel"   ;
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent mouseScrollEditMenuItem = this.BuildMenuComponent(menuMessage, "Makes the brush bigger or smaller.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(mouseScrollEditMenuItem);

            menuMessage = "left/right keyboard arrow"   ;
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent leftRightArrowKeysEditModeMenuItem = this.BuildMenuComponent(menuMessage, "Use to scroll the board.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(leftRightArrowKeysEditModeMenuItem);

            menuMessage = "left mouse button"   ;
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent leftMouseButtonMenuItem = this.BuildMenuComponent(menuMessage, "Place a tile on the game board.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(leftMouseButtonMenuItem);

            menuMessage = "right mouse button";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent rightMouseButtonEditMenuItem = this.BuildMenuComponent(menuMessage, "Change to next texture under the mouse brush cursor.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(rightMouseButtonEditMenuItem);

            menuMessage = "press Z key"   ;
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressZEditMenuItem = this.BuildMenuComponent(menuMessage, "Undo placing a tile on the game board grid.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(pressZEditMenuItem);

            menuMessage = "press S key";   
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressSEditMenuItem = this.BuildMenuComponent(menuMessage, "Save the game board's configuration (MyLevel.txt).", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(pressSEditMenuItem);

            menuMessage = "press D key"   ;
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressDEditMenuItem = this.BuildMenuComponent(menuMessage, "Reset the game board and write out a 'blank' MyLevel.txt.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(pressDEditMenuItem);

            menuMessage = "press PageUp"   ;
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPageUpEditMenuItem = this.BuildMenuComponent(menuMessage, "increase the size of the mouse's brush.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(pressPageUpEditMenuItem);

            menuMessage = "press PageDown"   ;
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPageDownEditMenuItem = this.BuildMenuComponent(menuMessage, "Decrease the size of the mouse's brush.", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(pressPageDownEditMenuItem);

            menuMessage = "press U key"  ; 
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressUMenuItem = this.BuildMenuComponent(menuMessage, "Undo a whole board delete (pressed D previously).", offsetY);
            this._editScreenHelpMenu.AddMenuComponents(pressUMenuItem);


            // General Help Menu
            menuMessage = "General Help";
            offsetY += this._helpFont.MeasureString(menuMessage).Y*3;
            this._generalHelpMenu = this.BuildMenuComponent(menuMessage, menuMessage, offsetY);

            menuMessage = "press N key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressBGeneralHelpMenuItem = this.BuildMenuComponent(menuMessage, "Go to the Sprite Editor screen.", offsetY);
            this._generalHelpMenu.AddMenuComponents(pressBGeneralHelpMenuItem);

            menuMessage = "press E key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressEGeneralHelpMenuItem = this.BuildMenuComponent(menuMessage, "Go to Editor screen.", offsetY);
            this._generalHelpMenu.AddMenuComponents(pressEGeneralHelpMenuItem);

            menuMessage = "press P key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPHelpMenuItem = this.BuildMenuComponent(menuMessage, "Enter play game mode.", offsetY);
            this._generalHelpMenu.AddMenuComponents(pressPHelpMenuItem);

            menuMessage = "press H key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressHGeneralHelpMenuItem = this.BuildMenuComponent(menuMessage, "To get help with keyboard and mouse commands.", offsetY);
            this._generalHelpMenu.AddMenuComponents(pressHGeneralHelpMenuItem);

            menuMessage = "press Q key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            MenuComponent pressQEditMenuItem = this.BuildMenuComponent(menuMessage, "Quit the editor.", offsetY);
            this._generalHelpMenu.AddMenuComponents(pressQEditMenuItem);

        }

        private MenuComponent BuildMenuComponent(string title,string description, float y)
        {
            return new MenuComponent(title,HelpMenuState.COMMAND_NAME_OFFSET,description,HelpMenuState.COMMAND_DESCRIPTION_OFFSET,(int)y);
        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, this.OurGame, gameTime);

            _oldKeyboardState = newKeyboardState;  // set the new state as the old state for next time
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            this._editScreenHelpMenu.Draw(spriteBatch,this._helpFont);
            this._generalHelpMenu.Draw(spriteBatch, this._helpFont);
        }
    }
}
