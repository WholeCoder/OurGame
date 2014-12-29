using System;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace WindowsGameLibrary1
{
    // *** note *** First Texture is the delete texture!

    public class TextureCache
    {
        private Texture2D[] boardTextures;
        private String[] boardTextureFileNames;

        // This instance method is used in Board Editing mode to get the current "brush" under the mouse cursor.
        private int currentTextureIndex;

        public TextureCache(String boardFileNameString, ContentManager Content)
        {
            this.loadBoardTextures(boardFileNameString, Content);
        }

        // Don't use this publicly - This method is a helper method used by loadTextures.
        private void loadTheseBoardTextures(ContentManager Content, String[] texStringRay)
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
        public void loadBoardTextures(String boardsFileNameString, ContentManager Content)
        {
            currentTextureIndex = 0;

            if (File.Exists(boardsFileNameString))
            {

                using (FileStream fs = File.OpenRead(boardsFileNameString))
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
                    this.loadTheseBoardTextures(Content, texStringRay);
                } // end using
            }
            else
            {
                using (FileStream fs = File.Create(boardsFileNameString))
                {
                    AddText(fs, "numberOfTileTextures:" + 3);
                    AddText(fs, "\n");

                    AddText(fs, "Images/DeleteBrush");
                    AddText(fs, "\n");
                    AddText(fs, "Images/tile");
                    AddText(fs, "\n");
                    AddText(fs, "Images/tile2");
                    AddText(fs, "\n");

                    String[] texStringRay = new String[3];
                    texStringRay[0] = "Images/DeleteBrush";
                    texStringRay[1] = "Images/tile";
                    texStringRay[2] = "Images/tile2";
                    this.loadTheseBoardTextures(Content, texStringRay);
                } // end using
            } // end else
        } // end method

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        } // End method.

    } // End class.

}
