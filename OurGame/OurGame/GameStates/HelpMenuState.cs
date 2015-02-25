using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OurGame.MenuComponents;

namespace OurGame.GameStates
{
    public class HelpMenuState : State
    {
        private MenuComponent _editScreenHelpMenu;
        private MenuComponent _generalHelpMenu;
        private SpriteFont _helpFont;
        private KeyboardState _oldKeyboardState;
        // These two constants control the way the command name and description line up on the screen.
        // ReSharper disable once InconsistentNaming
        private const int COMMAND_NAME_OFFSET = 100;
        // ReSharper disable once InconsistentNaming
        private const int COMMAND_DESCRIPTION_OFFSET = 450;

        // Call setStateWhenUpdating on this instance variable to change to a different game state.
        private Game1 OurGame { get; set; }

        public override string ToString()
        {
            return "HelpMenuState";
        }

        public override void Initialize(Game1 ourGame)
        {
            Debug.Assert(ourGame != null, "ourGame can not be null!");

            OurGame = ourGame;
        }

        protected override void LoadStatesContent(ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            _helpFont = Content.Load<SpriteFont>(@"fonts\helpfont");

            var menuMessage = "Game Board Edit Help";
            var offsetY = _helpFont.MeasureString(menuMessage).Y;
            _editScreenHelpMenu = BuildMenuComponent(menuMessage, "", offsetY);

            menuMessage = "Controls";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var editGameBoardTitle = BuildMenuComponent(menuMessage, "Game Board Edit Mode", offsetY);
            _editScreenHelpMenu.AddMenuComponents(editGameBoardTitle);

            menuMessage = "mouse scroll wheel";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var mouseScrollEditMenuItem = BuildMenuComponent(menuMessage, "Makes the brush bigger or smaller.", offsetY);
            _editScreenHelpMenu.AddMenuComponents(mouseScrollEditMenuItem);

            menuMessage = "left/right keyboard arrow";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var leftRightArrowKeysEditModeMenuItem = BuildMenuComponent(menuMessage, "Use to scroll the board.", offsetY);
            _editScreenHelpMenu.AddMenuComponents(leftRightArrowKeysEditModeMenuItem);

            menuMessage = "left mouse button";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var leftMouseButtonMenuItem = BuildMenuComponent(menuMessage, "Place a tile on the game board.", offsetY);
            _editScreenHelpMenu.AddMenuComponents(leftMouseButtonMenuItem);

            menuMessage = "right mouse button";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var rightMouseButtonEditMenuItem = BuildMenuComponent(menuMessage,
                "Change to next texture under the mouse brush cursor.", offsetY);
            _editScreenHelpMenu.AddMenuComponents(rightMouseButtonEditMenuItem);

            menuMessage = "press Z key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressZEditMenuItem = BuildMenuComponent(menuMessage, "Undo placing a tile on the game board grid.",
                offsetY);
            _editScreenHelpMenu.AddMenuComponents(pressZEditMenuItem);

            menuMessage = "press S key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressSEditMenuItem = BuildMenuComponent(menuMessage,
                "Save the game board's configuration (MyLevel.txt).", offsetY);
            _editScreenHelpMenu.AddMenuComponents(pressSEditMenuItem);

            menuMessage = "press D key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressDEditMenuItem = BuildMenuComponent(menuMessage,
                "Reset the game board and write out a 'blank' MyLevel.txt.", offsetY);
            _editScreenHelpMenu.AddMenuComponents(pressDEditMenuItem);

            menuMessage = "press PageUp";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressPageUpEditMenuItem = BuildMenuComponent(menuMessage, "increase the size of the mouse's brush.",
                offsetY);
            _editScreenHelpMenu.AddMenuComponents(pressPageUpEditMenuItem);

            menuMessage = "press PageDown";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressPageDownEditMenuItem = BuildMenuComponent(menuMessage, "Decrease the size of the mouse's brush.",
                offsetY);
            _editScreenHelpMenu.AddMenuComponents(pressPageDownEditMenuItem);

            menuMessage = "press U key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressUMenuItem = BuildMenuComponent(menuMessage, "Undo a whole board delete (pressed D previously).",
                offsetY);
            _editScreenHelpMenu.AddMenuComponents(pressUMenuItem);


            // General Help Menu
            menuMessage = "General Help";
            offsetY += _helpFont.MeasureString(menuMessage).Y*3;
            _generalHelpMenu = BuildMenuComponent(menuMessage, menuMessage, offsetY);

            menuMessage = "press N key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressBGeneralHelpMenuItem = BuildMenuComponent(menuMessage, "Go to the Sprite Editor screen.", offsetY);
            _generalHelpMenu.AddMenuComponents(pressBGeneralHelpMenuItem);

            menuMessage = "press E key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressEGeneralHelpMenuItem = BuildMenuComponent(menuMessage, "Go to Editor screen.", offsetY);
            _generalHelpMenu.AddMenuComponents(pressEGeneralHelpMenuItem);

            menuMessage = "press P key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressPHelpMenuItem = BuildMenuComponent(menuMessage, "Enter play game mode.", offsetY);
            _generalHelpMenu.AddMenuComponents(pressPHelpMenuItem);

            menuMessage = "press H key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressHGeneralHelpMenuItem = BuildMenuComponent(menuMessage,
                "To get help with keyboard and mouse commands.", offsetY);
            _generalHelpMenu.AddMenuComponents(pressHGeneralHelpMenuItem);

            menuMessage = "press Q key";
            offsetY += _helpFont.MeasureString(menuMessage).Y;
            var pressQEditMenuItem = BuildMenuComponent(menuMessage, "Quit the editor.", offsetY);
            _generalHelpMenu.AddMenuComponents(pressQEditMenuItem);
        }

        private MenuComponent BuildMenuComponent(string title, string description, float y)
        {
            Debug.Assert(title != null, "title can not be null!");
            Debug.Assert(description != null, "description can not be null");

            return new MenuComponent(title, COMMAND_NAME_OFFSET, description, COMMAND_DESCRIPTION_OFFSET, (int) y);
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");

            var newKeyboardState = Keyboard.GetState(); // get the newest state

            SwitchStateLogic.DoChangeGameStateFromKeyboardLogic(newKeyboardState, _oldKeyboardState, OurGame, gameTime);

            _oldKeyboardState = newKeyboardState; // set the new state as the old state for next time
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Debug.Assert(gameTime != null, "gameTime can not be null!");
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");

            _editScreenHelpMenu.Draw(spriteBatch, _helpFont);
            _generalHelpMenu.Draw(spriteBatch, _helpFont);
        }
    }
}