using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Commands;
using OurGame.OurGameLibrary;

namespace OurGame.MenuComponents
{
    public class MenuComponent
    {
        private readonly int _commandNameLeftJustifyPixels;
        private readonly string _commandName;

        private readonly int _commandDescriptionJustifyPixels;
        private readonly string _commandDescription;

        private readonly int _yStart;

        private ICommand _command;
        
        // This instance member is null unless this MenuComponent is a sub-menu;
        private readonly List<MenuComponent> _menuComponents;

        private IEnumerable<MenuComponent> GetMenuComponents()
        {
            return _menuComponents;
        }

        public MenuComponent(string name, int nameJustifyPixels, string description, int descriptionPixels, int yStart)
        {
            Debug.Assert(name != null, "name can not be null!");
            Debug.Assert(description != null, "description can not be null!");

            this._commandName = name;
            this._commandNameLeftJustifyPixels = nameJustifyPixels;

            this._commandDescription = description;
            this._commandDescriptionJustifyPixels = descriptionPixels;

            this._yStart = yStart;

            this._menuComponents = new List<MenuComponent>();
            this._command = new DoNothingCommand();
        }

        public void SetCommand(ICommand command)
        {
            Debug.Assert(command != null, "command can not be null!");

            this._command = command;
        }

        public void AddMenuComponents(MenuComponent mComponent)
        {
            Debug.Assert(mComponent != null, "mComponent can not be null!");

            this._menuComponents.Add(mComponent);
        }

        private string GetName()
        {
            return this._commandName;
        }

        private string GetDescription()
        {
            return this._commandDescription;
        }

        public void ExecuteCommand()
        {
            this._command.Execute();
        } // end method

        public void Draw(SpriteBatch spriteBatch, SpriteFont HelpFont)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");
            Debug.Assert(HelpFont != null, "HelpFont can not be null!");

            float offsetY = this._yStart;
            spriteBatch.DrawString(HelpFont, this.GetName(),
                                   new Vector2((Board.SCREEN_WIDTH - HelpFont.MeasureString(this._commandName).X) / 2, offsetY), Color.Black, 0, Vector2.Zero,
                                   1, SpriteEffects.None, 1);
            offsetY += 2*HelpFont.MeasureString(this.GetName()).Y;
            foreach (var menuItem in this.GetMenuComponents())
            {

                spriteBatch.DrawString(HelpFont, menuItem.GetName(),
                                       new Vector2(this._commandNameLeftJustifyPixels, offsetY), Color.Black, 0, Vector2.Zero,
                                       1, SpriteEffects.None, 1);
                int xoffset = (int)(Board.SCREEN_WIDTH-HelpFont.MeasureString(this._commandDescription).X- this._commandDescriptionJustifyPixels);
                spriteBatch.DrawString(HelpFont, menuItem.GetDescription(),
                                       new Vector2(xoffset, offsetY), Color.Black, 0, Vector2.Zero,
                                       1, SpriteEffects.None, 1);
                offsetY += HelpFont.MeasureString(menuItem.GetDescription()).Y;
            }
        }
    } // end class
} // end using
