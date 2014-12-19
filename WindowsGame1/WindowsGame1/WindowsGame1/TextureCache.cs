using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WindowsGame1
{
    class TextureCache
    {
        private Texture2D[] textures;
        private String[] textureFileNames;

        private int currentTextureIndex;

        public TextureCache(ContentManager Content)
        {
            // TODO: use this.Content to load your game content here
            textures = new Texture2D[2];
            textureFileNames = new String[2];

            textureFileNames[0] = @"Images/tile";
            textureFileNames[1] = @"Images/tile2";

            textures[0] = Content.Load<Texture2D>(textureFileNames[0]);
            textures[1] = Content.Load<Texture2D>(textureFileNames[1]);

            currentTextureIndex = 0;
        }

        public void loadTheseTextures(ContentManager Content, String[] texStringRay)
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

        public int GetLengthOfTexuterArray()
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
    }

}
