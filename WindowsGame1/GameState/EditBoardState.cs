using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// My usings.
using Command;
using WindowsGameLibrary1;

namespace GameState
{
    public class EditBoardState : State
    {
        Board board;

        // This is the position of the mouse "locked" onto a grid position.
        Vector2 mouseCursorLockedToNearestGridPositionVector;

        // Holds textures so they aren't re-created.
        TextureCache tCache;

        // This instance variable lets us scroll the board horizontally.
        int screenXOffset = 0;
        int scrollAmount = 5;

        // This is the name the gameboard is saved to when S is pressed.
        string pathToSavedGambeBoardConfigurationFile = @"MyLevel.txt";
        string pathToTextureCacheConfig = @"TextureCache.txt";

        MultiTexture multiTexture;
        int multiTextureWidthHeight = 1;

        public EditBoardState()
        {

        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }

        public override void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
