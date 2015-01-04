using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace OurGame.WindowsGameLibrary1
{
    // *** note *** First Texture is the delete texture!

    public class TextureCache
    {
        // These are all for the board.
        private Texture2D[] boardTextures;
        private String[] boardTextureFileNames;

        // This instance method is used in Board Editing mode to get the current "brush" under the mouse cursor.
        private int currentTextureIndex;

        
        // These are for the player and non-player characters
        private Texture2D[] spriteTextures;
        private string[] spriteTextureFileNames;


        public TextureCache(String boardFileNameString, string spriteFileName, ContentManager Content)
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

                WriteOutStringRayAndLenthToFile(spriteFileName, texStringRay);

                this.LoadTheseTextures(Content, texStringRay, false);
            } // end else
        }

        // Don't use this publicly - This method is a helper method used by loadTextures.
        private void LoadTheseTextures(ContentManager Content, String[] texStringRay, bool forBoard)
        {
            if (forBoard)
            {
                boardTextures = new Texture2D[texStringRay.Length];
                boardTextureFileNames = new String[texStringRay.Length];

                for (int i = 0; i < boardTextureFileNames.Length; i++)
                {
                    boardTextureFileNames[i] = texStringRay[i];
                }

                for (int i = 0; i < boardTextures.Length; i++)
                {
                    boardTextures[i] = Content.Load<Texture2D>(boardTextureFileNames[i]);
                }
            }
            else
            {
                // Load sprite sheets for player and non-player characters.
                spriteTextures = new Texture2D[texStringRay.Length] ;
                spriteTextureFileNames = new String[texStringRay.Length];

                for (int i = 0; i < spriteTextureFileNames.Length; i++)
                {
                    spriteTextureFileNames[i] = texStringRay[i];
                }

                for (int i = 0; i < spriteTextures.Length; i++)
                {
                    spriteTextures[i] = Content.Load<Texture2D>(spriteTextureFileNames[i]);
                }
            }
        }

        public int GetLengthOfBoardTextureArray()
        {
            return boardTextures.Length;
        }

        public Texture2D GetFromTexture2DBoardArray(int index)
        {
            return boardTextures[index];
        }

        public String GetFromTextureBoardStringArray(int index)
        {
            return boardTextureFileNames[index];
        }

        public Texture2D GetTexture2DFromStringBoardArray(String str)
        {
            for (int i = 0; i < boardTextureFileNames.Length; i++)
            {
                if (boardTextureFileNames[i].Equals(str))
                {
                    return boardTextures[i];
                }
            }
            return null;
        }

        public String GetStringFilenameFromTexture2DForBoard(Texture2D text)
        {
            for (int i = 0; i < boardTextures.Length; i++)
            {
                if (boardTextures[i] == text)
                {
                    return boardTextureFileNames[i];
                }
            }
            return "null";
        }

        public Texture2D GetTexture2DFromStringSpriteArray(String str)
        {
            for (int i = 0; i < spriteTextureFileNames.Length; i++)
            {
                if (spriteTextureFileNames[i].Equals(str))
                {
                    return spriteTextures[i];
                }
            }
            return null;
        }

        public String GetStringFilenameFromTexture2DForSprite(Texture2D text)
        {
            for (int i = 0; i < spriteTextures.Length; i++)
            {
                if (spriteTextures[i] == text)
                {
                    return spriteTextureFileNames[i];
                }
            }
            return "null";
        }

        // This method is called by the EditBoardState class to change to the next "brush" under the mouse cursor.
        public void NextTexture()
        {
            currentTextureIndex = (currentTextureIndex + 1) % boardTextures.Length;
        }

        public Texture2D GetCurrentTexture()
        {
            // This is the DeleteBrush texture to delete what is under the mouse cursor brush.
            if (this.boardTextureFileNames[currentTextureIndex].Equals("Images/DeleteBrush"))
            {
                return null;
            }
            return boardTextures[currentTextureIndex];
        }

        // Use this called from the main Game Class.
        public void LoadBoardTextures(String boardsFileNameString, ContentManager Content)
        {
            currentTextureIndex = 0;

            if (File.Exists(boardsFileNameString))
            {
                // The next call also calls this.loadTheseTextures(Content, texStringRay, true)
                string[] texArray = ReadInTextureArrayFromAFile(boardsFileNameString, Content); // end using

                this.LoadTheseTextures(Content, texArray, true);
            }
            else
            {
                // Write out our default texture file for the board.
                String[] texStringRay = new String[3];
                texStringRay[0] = "Images/DeleteBrush";
                texStringRay[1] = "Images/tile";
                texStringRay[2] = "Images/tile2";

                WriteOutStringRayAndLenthToFile(boardsFileNameString, texStringRay);

                this.LoadTheseTextures(Content, texStringRay, true);
            } // end else
        } // end method

        private string[] ReadInTextureArrayFromAFile(String textureFileNameString, ContentManager Content)
        {
            using (FileStream fs = File.OpenRead(textureFileNameString))
            {
                byte[] b = new byte[1024];
                String configurationString = "";
                UTF8Encoding temp = new UTF8Encoding(true);

                while (fs.Read(b, 0, b.Length) > 0)
                {
                    configurationString += temp.GetString(b);
                }

                String[] configStringSplitRay = configurationString.Split('\n');
                Console.WriteLine("configStringRay == " + configStringSplitRay[0]);
                int numberOfTileTextures = Convert.ToInt32(configStringSplitRay[0].Split(':')[1]);
                String[] texStringRay = new String[numberOfTileTextures];
                for (int i = 0; i < texStringRay.Length; i++)
                {
                    texStringRay[i] = configStringSplitRay[1 + i];
                }

                return texStringRay;
            }
        }

        private static void WriteOutStringRayAndLenthToFile(String textureFileName, String[] texStringRay)
        {
            using (FileStream fs = File.Create(textureFileName))
            {
                AddText(fs, "numberOfTileTextures:" + texStringRay.Length);
                AddText(fs, "\n");

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
