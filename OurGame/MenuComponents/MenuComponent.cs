using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

// My usings.
using OurGame.Commands;

namespace My.MenuComponents
{
    public class MenuComponent
    {
        private string _Name;
        private ICommand _Command;
        
        // This instance member is null unless this MenuComponent is a sub-menu;
        private List<MenuComponent> _MenuComponents;

        public List<MenuComponent> getMenuComponents()
        {
            if (this._MenuComponents == null)
            {
                throw new DidNotAddAnyMenuItemsToMenucomponent();
            }
            return _MenuComponents;
        }

        public MenuComponent(string name)
        {
            this._Name = name;
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
    } // end class
} // end using
