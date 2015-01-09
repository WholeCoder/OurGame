﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

// My usings.
using OurGame.Commands;


namespace OurGame.MenuComponents
{
    public class MenuComponent
    {
        private string _Name;
        private ICommand _Command;
        public Vector2 Position;
        
        // This instance member is null unless this MenuComponent is a sub-menu;
        private List<MenuComponent> _MenuComponents;

        public List<MenuComponent> GetMenuComponents()
        {
            if (this._MenuComponents == null)
            {
                throw new DidNotAddAnyMenuItemsToMenucomponent();
            }
            return _MenuComponents;
        }

        public MenuComponent(string name, Vector2 position)
        {
            this._Name = name;
            this.Position = position;
        }

        public void SetCommand(ICommand command)
        {
            this._Command = command;
        }

        public void AddMenuComponents(MenuComponent mComponent)
        {
            if (this._MenuComponents == null)
            {
                this._MenuComponents = new List<MenuComponent>();
            }

            this._MenuComponents.Add(mComponent);
        }

        public string GetName()
        {
            return this._Name;
        }

        public void ExecuteCommand()
        {
            if (this._Command == null)
            {
                throw new DidNotSetCommandInMenuComponent("Did not call MenuComponent.SetCommand(ICommand command) on this MenuComponent!");
            }

            Debug.Assert(this._Command != null, "MenuComponent._Command can't be null!");

            this._Command.Execute();
        } // end method

        public void Draw(SpriteBatch spriteBatch, SpriteFont HelpFont)
        {
            spriteBatch.DrawString(HelpFont, this.GetName(),
                                   this.Position, Color.Black, 0, Vector2.Zero,
                                   1, SpriteEffects.None, 1);
            foreach (var menuItem in this.GetMenuComponents())
            {
                spriteBatch.DrawString(HelpFont, menuItem.GetName(),
                                       menuItem.Position, Color.Black, 0, Vector2.Zero,
                                       1, SpriteEffects.None, 1);
            }
        }
    } // end class
} // end using
