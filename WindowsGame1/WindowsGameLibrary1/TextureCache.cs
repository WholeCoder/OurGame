using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsGameLibrary1
{
    public class TextureCache
    {
        private Texture2D[] textures;
        private String[] textureFileNames;

        private int currentTextureIndex;

        public TextureCache(String fileNameString, ContentManager Content)
        {
            // TODO: use this.Content to load your game content here
            this.loadTextures(fileNameString, Content);

            currentTextureIndex = 0;
        }

        private void loadTheseTextures(ContentManager Content, String[] texStringRay)
        {
            textures = new Texture2D[texStringRay.Length];
            textureFileNames = new String[texStringRay.Length];

            for (int i = 0; i < textureFileNames.Length; i++)
            {
                textureFileNames[i] = texStringRay[i];
            }

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = Content.Load<Texture2D>(textureFileNames[i]);
            }
        }

        public int GetLengthOfTextureArray()
        {
            return textures.Length;
        }

        public Texture2D GetFromTexture2DArray(int index)
        {
            return textures[index];
        }

        public String GetFromTextureStringArray(int index)
        {
            return textureFileNames[index];
        }

        public Texture2D GetTexture2DFromString(String str)
        {
            for (int i = 0; i < textureFileNames.Length; i++)
            {
                if (textureFileNames[i].Equals(str))
                {
                    return textures[i];
                }
            }
            return null;
        }

        public String GetStringFilenameFromTexture2D(Texture2D text)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                if (textures[i] == text)
                {
                    return textureFileNames[i];
                }
            }
            return "null";
        }

        public void NextTexture()
        {
            currentTextureIndex = (currentTextureIndex + 1) % textures.Length;
        }

        public Texture2D GetCurrentTexture()
        {
            return textures[currentTextureIndex];
        }

        // Use this called from the main Game Class.
        public void loadTextures(String fileNameString, ContentManager Content)
        {
            if (File.Exists(fileNameString))
            {

                using (FileStream fs = File.OpenRead(fileNameString))
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
                    this.loadTheseTextures(Content, texStringRay);
                } // end using
            }
            else
            {
                using (FileStream fs = File.Create(fileNameString))
                {
                    AddText(fs, "numberOfTileTextures:" + 2);
                    AddText(fs, "\n");

                    AddText(fs, "Images/tile");
                    AddText(fs, "\n");
                    AddText(fs, "Images/tile2");
                    AddText(fs, "\n");

                    String[] texStringRay = new String[2];
                    texStringRay[0] = "Images/tile";
                    texStringRay[1] = "Images/tile2";
                    this.loadTheseTextures(Content, texStringRay);
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
