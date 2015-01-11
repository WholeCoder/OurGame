using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Commands;
using OurGame.OurGameLibrary;

namespace OurGame.MenuComponents
{
    public class MenuComponent
    {
        private int _CommandNameLeftJustifyPixels;
        private string _CommandName;

        private int _CommandDescriptionJustifyPixels;
        private string _CommandDescription;

        private int _YStart;

        private ICommand _Command;
        
        // This instance member is null unless this MenuComponent is a sub-menu;
        private List<MenuComponent> _MenuComponents;

        public List<MenuComponent> GetMenuComponents()
        {
            return _MenuComponents;
        }

        public MenuComponent(string name, int nameJustifyPixels, string description, int descriptionPixels, int yStart)
        {
            this._CommandName = name;
            this._CommandNameLeftJustifyPixels = nameJustifyPixels;

            this._CommandDescription = description;
            this._CommandDescriptionJustifyPixels = descriptionPixels;

            this._YStart = yStart;

            this._MenuComponents = new List<MenuComponent>();
            this._Command = new DoNothingCommand();
        }

        public void SetCommand(ICommand command)
        {
            this._Command = command;
        }

        public void AddMenuComponents(MenuComponent mComponent)
        {
            this._MenuComponents.Add(mComponent);
        }

        public string GetName()
        {
            return this._CommandName;
        }

        public string GetDescription()
        {
            return this._CommandDescription;
        }

        public void ExecuteCommand()
        {
            this._Command.Execute();
        } // end method

        public void Draw(SpriteBatch spriteBatch, SpriteFont HelpFont)
        {
            float offsetY = this._YStart;
            spriteBatch.DrawString(HelpFont, this.GetName(),
                                   new Vector2((Board.SCREEN_WIDTH - HelpFont.MeasureString(this._CommandName).X) / 2, offsetY), Color.Black, 0, Vector2.Zero,
                                   1, SpriteEffects.None, 1);
            offsetY += 2*HelpFont.MeasureString(this.GetName()).Y;
            foreach (var menuItem in this.GetMenuComponents())
            {

                spriteBatch.DrawString(HelpFont, menuItem.GetName(),
                                       new Vector2(this._CommandNameLeftJustifyPixels, offsetY), Color.Black, 0, Vector2.Zero,
                                       1, SpriteEffects.None, 1);
                int xoffset = (int)(Board.SCREEN_WIDTH-HelpFont.MeasureString(this._CommandDescription).X- this._CommandDescriptionJustifyPixels);
                spriteBatch.DrawString(HelpFont, menuItem.GetDescription(),
                                       new Vector2(xoffset, offsetY), Color.Black, 0, Vector2.Zero,
                                       1, SpriteEffects.None, 1);
                offsetY += HelpFont.MeasureString(menuItem.GetDescription()).Y;
            }
        }
    } // end class
} // end using
