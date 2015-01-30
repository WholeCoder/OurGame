using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OurGame.Commands;
using OurGame.OurGameLibrary;

namespace OurGame.MenuComponents
{
    public class MenuComponent
    {
        private readonly string _commandDescription;
        private readonly int _commandDescriptionJustifyPixels;
        private readonly string _commandName;
        private readonly int _commandNameLeftJustifyPixels;
        // This instance member is null unless this MenuComponent is a sub-menu;
        private readonly List<MenuComponent> _menuComponents;
        private readonly int _yStart;
        private ICommand _command;

        public MenuComponent(string name, int nameJustifyPixels, string description, int descriptionPixels, int yStart)
        {
            Debug.Assert(name != null, "name can not be null!");
            Debug.Assert(description != null, "description can not be null!");

            _commandName = name;
            _commandNameLeftJustifyPixels = nameJustifyPixels;

            _commandDescription = description;
            _commandDescriptionJustifyPixels = descriptionPixels;

            _yStart = yStart;

            _menuComponents = new List<MenuComponent>();
            _command = new DoNothingCommand();
        }

        private IEnumerable<MenuComponent> GetMenuComponents()
        {
            return _menuComponents;
        }

        public void SetCommand(ICommand command)
        {
            Debug.Assert(command != null, "command can not be null!");

            _command = command;
        }

        public void AddMenuComponents(MenuComponent mComponent)
        {
            Debug.Assert(mComponent != null, "mComponent can not be null!");

            _menuComponents.Add(mComponent);
        }

        private string GetName()
        {
            return _commandName;
        }

        private string GetDescription()
        {
            return _commandDescription;
        }

        public void ExecuteCommand()
        {
            _command.Execute();
        } // end method

        public void Draw(SpriteBatch spriteBatch, SpriteFont HelpFont)
        {
            Debug.Assert(spriteBatch != null, "spriteBatch can not be null!");
            Debug.Assert(HelpFont != null, "HelpFont can not be null!");

            float offsetY = _yStart;
            spriteBatch.DrawString(HelpFont, GetName(),
                new Vector2((Board.SCREEN_WIDTH - HelpFont.MeasureString(_commandName).X)/2, offsetY), Color.Black, 0,
                Vector2.Zero,
                1, SpriteEffects.None, 1);
            offsetY += 2*HelpFont.MeasureString(GetName()).Y;
            foreach (var menuItem in GetMenuComponents())
            {
                spriteBatch.DrawString(HelpFont, menuItem.GetName(),
                    new Vector2(_commandNameLeftJustifyPixels, offsetY), Color.Black, 0, Vector2.Zero,
                    1, SpriteEffects.None, 1);
                var xoffset =
                    (int)
                        (Board.SCREEN_WIDTH - HelpFont.MeasureString(_commandDescription).X -
                         _commandDescriptionJustifyPixels);
                spriteBatch.DrawString(HelpFont, menuItem.GetDescription(),
                    new Vector2(xoffset, offsetY), Color.Black, 0, Vector2.Zero,
                    1, SpriteEffects.None, 1);
                offsetY += HelpFont.MeasureString(menuItem.GetDescription()).Y;
            }
        }
    } // end class
} // end using