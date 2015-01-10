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
        private KeyboardState _OldKeyboardState;
        private SpriteFont _HelpFont;
        private MenuComponent _EditScreenHelpMenu;
        private MenuComponent _GeneralHelpMenu;

        // These two constants control the way the command name and description line up on the screen.
        public const int COMMAND_NAME_OFFSET = 100;
        public const int COMMAND_DESCRIPTION_OFFSET = 450;

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
            this._EditScreenHelpMenu= this.BuildMenuComponent(menuMessage,"", offsetY);

            menuMessage = "Controls"   ;
            offsetY += this._HelpFont.MeasureString(menuMessage).Y;
            MenuComponent editGameBoardTitle = this.BuildMenuComponent(menuMessage, "Game Board Edit Mode", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(editGameBoardTitle);

            menuMessage = "mouse scroll wheel"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent mouseScrollEditMenuItem = this.BuildMenuComponent(menuMessage, "Makes the brush bigger or smaller.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(mouseScrollEditMenuItem);

            menuMessage = "left/right keyboard arrow"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent leftRightArrowKeysEditModeMenuItem = this.BuildMenuComponent(menuMessage, "Use to scroll the board.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(leftRightArrowKeysEditModeMenuItem);

            menuMessage = "left mouse button"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent leftMouseButtonMenuItem = this.BuildMenuComponent(menuMessage, "Place a tile on the game board.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(leftMouseButtonMenuItem);

            menuMessage = "right mouse button";
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent rightMouseButtonEditMenuItem = this.BuildMenuComponent(menuMessage, "Change to next texture under the mouse brush cursor.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(rightMouseButtonEditMenuItem);

            menuMessage = "press Z key"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressZEditMenuItem = this.BuildMenuComponent(menuMessage, "Undo placing a tile on the game board grid.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(pressZEditMenuItem);

            menuMessage = "press S key";   
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressSEditMenuItem = this.BuildMenuComponent(menuMessage, "Save the game board's configuration (MyLevel.txt).", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(pressSEditMenuItem);

            menuMessage = "press D key"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressDEditMenuItem = this.BuildMenuComponent(menuMessage, "Reset the game board and write out a 'blank' MyLevel.txt.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(pressDEditMenuItem);

            menuMessage = "press PageUp"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPageUpEditMenuItem = this.BuildMenuComponent(menuMessage, "increase the size of the mouse's brush.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(pressPageUpEditMenuItem);

            menuMessage = "press PageDown"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPageDownEditMenuItem = this.BuildMenuComponent(menuMessage, "Decrease the size of the mouse's brush.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(pressPageDownEditMenuItem);

            menuMessage = "press Q key"   ;
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressQEditMenuItem = this.BuildMenuComponent(menuMessage, "Quit the editor.", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(pressQEditMenuItem);

            menuMessage = "press U key"  ; 
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressUMenuItem = this.BuildMenuComponent(menuMessage, "Undo a whole board delete (pressed D previously).", offsetY);
            this._EditScreenHelpMenu.AddMenuComponents(pressUMenuItem);


            // General Help Menu
            menuMessage = "General Help";
            offsetY += this._HelpFont.MeasureString(menuMessage).Y*3;
            this._GeneralHelpMenu = this.BuildMenuComponent(menuMessage, menuMessage, offsetY);

            menuMessage = "press B key";
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressBGeneralHelpMenuItem = this.BuildMenuComponent(menuMessage, "Go to Blank Screen.", offsetY);
            this._GeneralHelpMenu.AddMenuComponents(pressBGeneralHelpMenuItem);

            menuMessage = "press E key";
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressEGeneralHelpMenuItem = this.BuildMenuComponent(menuMessage, "Go to Editor screen.", offsetY);
            this._GeneralHelpMenu.AddMenuComponents(pressEGeneralHelpMenuItem);

            menuMessage = "press P key";
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressPHelpMenuItem = this.BuildMenuComponent(menuMessage, "Enter play game mode.", offsetY);
            this._GeneralHelpMenu.AddMenuComponents(pressPHelpMenuItem);

            menuMessage = "press H key";
            offsetY += _HelpFont.MeasureString(menuMessage).Y;
            MenuComponent pressHGeneralHelpMenuItem = this.BuildMenuComponent(menuMessage, "To get help with keyboard and mouse commands.", offsetY);
            this._GeneralHelpMenu.AddMenuComponents(pressHGeneralHelpMenuItem);
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
            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _OldKeyboardState, this.OurGame, gameTime);

            _OldKeyboardState = newKeyboardState;  // set the new state as the old state for next time
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            this._EditScreenHelpMenu.Draw(spriteBatch,this._HelpFont);
            this._GeneralHelpMenu.Draw(spriteBatch, this._HelpFont);
        }
    }
}
