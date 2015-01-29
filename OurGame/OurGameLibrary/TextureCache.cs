using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OurGame.OurGameLibrary
{
    // *** note *** First Texture is the delete texture!

    public class TextureCache
    {
        // These are all for the board.
        private Texture2D[] _boardTextures;
        private String[] _boardTextureFileNames;

        // This instance method is used in Board Editing mode to get the current "brush" under the mouse cursor.
        private int _currentTextureIndex;

        
        // These are for the player and non-player characters
        private Texture2D[] _spriteTextures;
        private string[] _spriteTextureFileNames;

        private static TextureCache _textureCacheInsance = null;
        private const String BoardTextureCacheFileNameString = @"BoardTextureCache.txt";
        private const string SpriteFileName = @"SpriteTextureCache.txt";
        private static ContentManager _content;

        // MUST CALL setupFileNamesAndcontent(...) BEFORE CALLING THIS!
        public static TextureCache getInstance() 
        {
            if (TextureCache._content == null)
            {
                throw new MustCallSetContentMethodFirst("TextureCache.SetContent(...) must be called (usually in LoadContent(..) method of the State subclass) before TextureCache.getInstance() is called!");
            }

            Debug.Assert(TextureCache._content != null, "TextureCache._Content must not be nulll!");

            if (TextureCache._textureCacheInsance == null)
            {
                TextureCache._textureCacheInsance = new TextureCache(BoardTextureCacheFileNameString, SpriteFileName, _content);
            }

            return TextureCache._textureCacheInsance;
        }

        // This method needs to be called before the TextureCache.getInstance() is called!
        public static void SetContent(ContentManager Content)
        {
            TextureCache._content = Content;
        }

        private TextureCache(String boardFileNameString, string spriteFileName, ContentManager Content)
        {
            this.LoadBoardTextures(boardFileNameString, Content);
            this.LoadSpriteTextures(spriteFileName, Content);
        }

        private void LoadSpriteTextures(string spriteFileName, ContentManager Content)
        {
            if (File.Exists(spriteFileName))
            {
                // The next call also calls this.loadTheseTextures(Content, texStringRay, false)
                string[] texStringRay = ReadInTextureArrayFromAFile(spriteFileName, Content); // end using
                this.LoadTheseTextures(Content, texStringRay, false);
            }
            else
            {
                // Write out our default texture file for the board.
                String[] texStringRay = new String[2];

                texStringRay[0] = "Images/spritesheets/manspritesheet";
                texStringRay[1] = "Images/spritesheets/testspritesheet";
                //texStringRay[2] = "Images/tile2";

                WriteOutStringRayAndLengthToFile(spriteFileName, texStringRay);

                this.LoadTheseTextures(Content, texStringRay, false);
            } // end else
        }

        // Don't use this publicly - This method is a helper method used by loadTextures.
        private void LoadTheseTextures(ContentManager Content, String[] texStringRay, bool forBoard)
        {
            if (forBoard)
            {
                _boardTextures = new Texture2D[texStringRay.Length];
                _boardTextureFileNames = new String[texStringRay.Length];

                for (int i = 0; i < _boardTextureFileNames.Length; i++)
                {
                    _boardTextureFileNames[i] = texStringRay[i];
                }

                for (int i = 0; i < _boardTextures.Length; i++)
                {
                    _boardTextures[i] = Content.Load<Texture2D>(_boardTextureFileNames[i]);
                }
            }
            else
            {
                // Load sprite sheets for player and non-player characters.
                _spriteTextures = new Texture2D[texStringRay.Length] ;
                _spriteTextureFileNames = new String[texStringRay.Length];

                for (int i = 0; i < _spriteTextureFileNames.Length; i++)
                {
                    _spriteTextureFileNames[i] = texStringRay[i];
                }

                for (int i = 0; i < _spriteTextures.Length; i++)
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
            return _boardTextures[index];
        }

        public String GetFromTextureBoardStringArray(int index)
        {
            return _boardTextureFileNames[index];
        }

        public Texture2D GetTexture2DFromStringBoardArray(String str)
        {
            for (int i = 0; i < _boardTextureFileNames.Length; i++)
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
            for (int i = 0; i < _boardTextures.Length; i++)
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
            for (int i = 0; i < _spriteTextureFileNames.Length; i++)
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
            for (int i = 0; i < _spriteTextures.Length; i++)
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
            _currentTextureIndex = (_currentTextureIndex + 1) % _boardTextures.Length;
        }

        public Texture2D GetCurrentTexture()
        {
            // This is the DeleteBrush texture to delete what is under the mouse cursor brush.
            if (this._boardTextureFileNames[_currentTextureIndex].Equals("Images/DeleteBrush"))
            {
                return null;
            }
            return _boardTextures[_currentTextureIndex];
        }

        // Use this called from the main Game Class.
        public void LoadBoardTextures(String boardsFileNameString, ContentManager Content)
        {
            _currentTextureIndex = 0;

            if (File.Exists(boardsFileNameString))
            {
                // The next call also calls this.loadTheseTextures(Content, texStringRay, true)
                string[] texArray = ReadInTextureArrayFromAFile(boardsFileNameString, Content); // end using

                this.LoadTheseTextures(Content, texArray, true);
            }
            else
            {
                // Write out our default texture file for the board.
                // ReSharper disable once SuggestVarOrType_Elsewhere
                String[] texStringRay = new String[3];
                texStringRay[0] = "Images/DeleteBrush";
                texStringRay[1] = "Images/tile";
                texStringRay[2] = "Images/tile2";

                WriteOutStringRayAndLengthToFile(boardsFileNameString, texStringRay);

                this.LoadTheseTextures(Content, texStringRay, true);
            } // end else
        } // end method

        private string[] ReadInTextureArrayFromAFile(String textureFileNameString, ContentManager Content)
        {
                String[] configStringSplitRay = File.ReadAllLines(textureFileNameString);

                int numberOfTileTextures = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);
                // ReSharper disable once SuggestVarOrType_Elsewhere
                String[] texStringRay = new String[numberOfTileTextures];
                for (int i = 0; i < texStringRay.Length; i++)
                {
                    texStringRay[i] = configStringSplitRay[1 + i];
                }

                return texStringRay;
            
        }

        private static void WriteOutStringRayAndLengthToFile(String textureFileName, String[] texStringRay)
        {
            using (FileStream fs = File.Create(textureFileName))
            {
                AddText(fs, "numberOfTileTextures:" + texStringRay.Length);
                AddText(fs, "\n");

                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < texStringRay.Length; i++)
                {
                    AddText(fs, texStringRay[i]);
                    AddText(fs, "\n");
                }
            } // end using
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // End method.

    } // End class.

}
