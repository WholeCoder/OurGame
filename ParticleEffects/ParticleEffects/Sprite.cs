using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects
{
    public class Sprite
    {
        public Color m_cColor;
        public float m_fDepth;
        public float m_fRotation;
        public float m_fScale;
        public Rectangle m_rSrcRect;
        public Texture2D m_tTexture;
        public Vector2 m_vOrigin;
        public Vector2 m_vPos;

        public void Initialize(Texture2D texture)
        {
            m_tTexture = texture;
            m_rSrcRect.X = 0;
            m_rSrcRect.Y = 0;
            m_rSrcRect.Width = m_tTexture.Width;
            m_rSrcRect.Height = m_tTexture.Height;

            m_vOrigin.X = m_rSrcRect.Width/2;
            m_vOrigin.Y = m_rSrcRect.Height/2;

            m_fScale = 1.0f;
            m_fRotation = 0.0f;
            m_fDepth = 1.0f;
            m_cColor = Color.White;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch batch)
        {
             batch.Draw(m_tTexture, m_vPos, m_rSrcRect, m_cColor, m_fRotation, m_vOrigin, m_fScale, SpriteEffects.None,
                m_fDepth);
        }
    }
}