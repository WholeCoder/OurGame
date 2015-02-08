using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OurGame.OurGameLibrary
{
    // *** note *** First Texture is the delete texture!

    public class TextureCache
    {
        private const String BoardTextureCacheFileNameString = @"BoardTextureCache.txt";
        private const string SpriteFileName = @"SpriteTextureCache.txt";
        private static TextureCache _textureCacheInsance;
        private static ContentManager _content;
        private String[] _boardTextureFileNames;
        // These are all for the board.
        private Texture2D[] _boardTextures;
        // This instance method is used in Board Editing mode to get the current "brush" under the mouse cursor.
        private int _currentTextureIndex;
        private string[] _spriteTextureFileNames;
        // These are for the player and non-player characters
        private Texture2D[] _spriteTextures;

        private TextureCache(String boardFileNameString, string spriteFileName, ContentManager Content)
        {
            Debug.Assert(boardFileNameString != null && !boardFileNameString.Equals(""),
                "boardFileNameString can not be null or empty");
            Debug.Assert(spriteFileName != null && !spriteFileName.Equals(""),
                "spriteFileName can not be null or empty!");
            Debug.Assert(Content != null, "Content can not be null");

            LoadBoardTextures(boardFileNameString, Content);
            LoadSpriteTextures(spriteFileName, Content);
        }

        // MUST CALL setupFileNamesAndcontent(...) BEFORE CALLING THIS!
        public static TextureCache getInstance()
        {
            if (_content == null)
            {
                throw new MustCallSetContentMethodFirst(
                    "TextureCache.SetContent(...) must be called (usually in LoadContent(..) method of the State subclass) before TextureCache.getInstance() is called!");
            }

            Debug.Assert(_content != null, "TextureCache._Content must not be nulll!");

            if (_textureCacheInsance == null)
            {
                _textureCacheInsance = new TextureCache(BoardTextureCacheFileNameString, SpriteFileName, _content);
            }

            return _textureCacheInsance;
        }

        // This method needs to be called before the TextureCache.getInstance() is called!
        public static void SetContent(ContentManager Content)
        {
            Debug.Assert(Content != null, "Content can not be null!");

            _content = Content;
        }

        private void LoadSpriteTextures(string spriteFileName, ContentManager Content)
        {
            Debug.Assert(spriteFileName != null && !spriteFileName.Equals(""),
                "spriteFileName can not be null or empty string!");

            if (File.Exists(spriteFileName))
            {
                // The next call also calls this.loadTheseTextures(Content, texStringRay, false)
                var texStringRay = ReadInTextureArrayFromAFile(spriteFileName, Content); // end using
                LoadTheseTextures(Content, texStringRay, false);
            }
            else
            {
                // Write out our default texture file for the board.
                var texStringRay = new String[2];

                texStringRay[0] = "Images/spritesheets/manspritesheet";
                texStringRay[1] = "Images/spritesheets/testspritesheet";
                //texStringRay[2] = "Images/tile2";

                WriteOutStringRayAndLengthToFile(spriteFileName, texStringRay);

                LoadTheseTextures(Content, texStringRay, false);
            } // end else
        }

        // Don't use this publicly - This method is a helper method used by loadTextures.
        private void LoadTheseTextures(ContentManager Content, String[] texStringRay, bool forBoard)
        {
            Debug.Assert(Content != null, "Content can not be null!");
            Debug.Assert(texStringRay != null, "texStringRay can not be null!");

            if (forBoard)
            {
                _boardTextures = new Texture2D[texStringRay.Length];
                _boardTextureFileNames = new String[texStringRay.Length];

                for (var i = 0; i < _boardTextureFileNames.Length; i++)
                {
                    _boardTextureFileNames[i] = texStringRay[i];
                }

                for (var i = 0; i < _boardTextures.Length; i++)
                {
                    _boardTextures[i] = Content.Load<Texture2D>(_boardTextureFileNames[i]);
                }
            }
            else
            {
                // Load sprite sheets for player and non-player characters.
                _spriteTextures = new Texture2D[texStringRay.Length];
                _spriteTextureFileNames = new String[texStringRay.Length];

                for (var i = 0; i < _spriteTextureFileNames.Length; i++)
                {
                    _spriteTextureFileNames[i] = texStringRay[i];
                }

                for (var i = 0; i < _spriteTextures.Length; i++)
                {
                    _spriteTextures[i] = Content.Load<Texture2D>(_spriteTextureFileNames[i]);
                }
            }
        }

        public int GetLengthOfBoardTextureArray()
        {
            return _boardTextures.Length;
        }

        public Texture2D GetFromTexture2DBoardArray(int index)
        {
            Debug.Assert(index >= 0, "index must by >= 0!");

            return _boardTextures[index];
        }

        public String GetFromTextureBoardStringArray(int index)
        {
            Debug.Assert(index >= 0, "index must by >= 0!");

            return _boardTextureFileNames[index];
        }

        public Texture2D GetTexture2DFromStringBoardArray(String str)
        {
            Debug.Assert(str != null, "str can not be null!");

            for (var i = 0; i < _boardTextureFileNames.Length; i++)
            {
                if (_boardTextureFileNames[i].Equals(str))
                {
                    return _boardTextures[i];
                }
            }
            return null;
        }

        public String GetStringFilenameFromTexture2DForBoard(Texture2D text)
        {
            for (var i = 0; i < _boardTextures.Length; i++)
            {
                if (_boardTextures[i] == text)
                {
                    return _boardTextureFileNames[i];
                }
            }
            return "null";
        }

        public Texture2D GetTexture2DFromStringSpriteArray(String str)
        {
            for (var i = 0; i < _spriteTextureFileNames.Length; i++)
            {
                if (_spriteTextureFileNames[i].Equals(str))
                {
                    return _spriteTextures[i];
                }
            }
            return null;
        }

        public String GetStringFilenameFromTexture2DForSprite(Texture2D text)
        {
            for (var i = 0; i < _spriteTextures.Length; i++)
            {
                if (_spriteTextures[i] == text)
                {
                    return _spriteTextureFileNames[i];
                }
            }
            return "null";
        }

        // This method is called by the EditBoardState class to change to the next "brush" under the mouse cursor.
        public void NextTexture()
        {
            _currentTextureIndex = (_currentTextureIndex + 1)%_boardTextures.Length;
        }

        public Texture2D GetCurrentTexture()
        {
            // This is the DeleteBrush texture to delete what is under the mouse cursor brush.
            if (_boardTextureFileNames[_currentTextureIndex].Equals("Images/DeleteBrush"))
            {
                return null;
            }
            return _boardTextures[_currentTextureIndex];
        }

        // Use this called from the main Game Class.
        public void LoadBoardTextures(String boardsFileNameString, ContentManager Content)
        {
            Debug.Assert(boardsFileNameString != null && !boardsFileNameString.Equals(""),
                "boardsFileNameString can not be null or empty!");
            Debug.Assert(Content != null, "Content can not be null!");

            _currentTextureIndex = 0;

            if (File.Exists(boardsFileNameString))
            {
                // The next call also calls this.loadTheseTextures(Content, texStringRay, true)
                var texArray = ReadInTextureArrayFromAFile(boardsFileNameString, Content); // end using

                LoadTheseTextures(Content, texArray, true);
            }
            else
            {
                // Write out our default texture file for the board.
                // ReSharper disable once SuggestVarOrType_Elsewhere
                var texStringRay = new String[3];
                texStringRay[0] = "Images/DeleteBrush";
                texStringRay[1] = "Images/tile";
                texStringRay[2] = "Images/tile2";

                WriteOutStringRayAndLengthToFile(boardsFileNameString, texStringRay);

                LoadTheseTextures(Content, texStringRay, true);
            } // end else
        } // end method

        private string[] ReadInTextureArrayFromAFile(String textureFileNameString, ContentManager Content)
        {
            Debug.Assert(textureFileNameString != null && !textureFileNameString.Equals(""),
                "textureFileNameString can not be null or equal the empty string!");
            Debug.Assert(Content != null, "Content can not equal null!");

            var configStringSplitRay = File.ReadAllLines(textureFileNameString);

            var numberOfTileTextures = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);
            // ReSharper disable once SuggestVarOrType_Elsewhere
            var texStringRay = new String[numberOfTileTextures];
            for (var i = 0; i < texStringRay.Length; i++)
            {
                texStringRay[i] = configStringSplitRay[1 + i];
            }

            return texStringRay;
        }

        private static void WriteOutStringRayAndLengthToFile(String textureFileName, String[] texStringRay)
        {
            Debug.Assert(textureFileName != null && !textureFileName.Equals(""),
                "textureFileName can not be null or an empty string!");
            Debug.Assert(texStringRay != null, "textStringRay can not be null!");

            using (var fs = File.Create(textureFileName))
            {
                Utilities.AddText(fs, "numberOfTileTextures:" + texStringRay.Length);
                Utilities.AddText(fs, "\n");

                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < texStringRay.Length; i++)
                {
                    Utilities.AddText(fs, texStringRay[i]);
                    Utilities.AddText(fs, "\n");
                }
            } // end using
        } // End method.
    } // End class.
}//sdffsfsd