﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects
{
    public class Particle
    {
        public Color m_cColor;
        public Color m_cFinalColor;
        public Color m_cInitColor;
        public Sprite m_cSprite;
        public float m_fDampening;
        public float m_fRot;
        public float m_fRotDampening;
        public float m_fRotVel;
        public float m_fScale;
        public float m_fScaleAcc;
        public float m_fScaleMax;
        public float m_fScaleVel;
        public int m_iAge;
        public int m_iFadeAge;
        public Vector2 m_vAcc;
        public Vector2 m_vPos;
        public Vector2 m_vVel;

        public Particle()
        {
            m_cSprite = new Sprite();
        }

        public void Initialize()
        {
        }

        public void Create(Texture2D spriteTexture, int ageInMS, Vector2 pos, Vector2 vel, Vector2 acc, float damp,
            float rot, float rotVel, float rotDamp, float scale, float scaleVel, float scaleAcc, float maxScale,
            Color initColor, Color finalColor, int fadeAge)
        {
            m_cSprite.Initialize(spriteTexture);

            m_iAge = ageInMS;
            m_vPos = pos;
            m_vVel = vel;
            m_vAcc = acc;
            m_fDampening = damp;

            m_fRot = rot;
            m_fRotVel = rotVel;
            m_fRotDampening = rotDamp;

            m_fScale = scale;
            m_fScaleVel = scaleVel;
            m_fScaleAcc = scaleAcc;
            m_fScaleMax = maxScale;

            m_cInitColor = initColor;
            m_cFinalColor = finalColor;
            m_iFadeAge = fadeAge;
        }

        public void UpdatePos(GameTime gameTime)
        {
            m_vVel *= m_fDampening;
            m_vVel += (m_vAcc*(float) gameTime.ElapsedGameTime.TotalSeconds);
            m_vPos += (m_vVel*(float) gameTime.ElapsedGameTime.TotalSeconds);

            if (m_vPos.Y >= 720)
            {
                m_vPos.Y = 720;
                m_vVel.X = 0;
            }

            m_cSprite.m_vPos = m_vPos;
        }

        public void UpdateRot(GameTime gameTime)
        {
            m_fRot *= m_fRotDampening;
            m_fRot += (m_fRotVel*(float) gameTime.ElapsedGameTime.TotalSeconds);

            m_cSprite.m_fRotation = m_fRot;
        }

        public void UpdateScale(GameTime gameTime)
        {
            m_fScaleVel += (m_fScaleAcc*(float) gameTime.ElapsedGameTime.TotalSeconds);
            m_fScale += (m_fScaleVel*(float) gameTime.ElapsedGameTime.TotalSeconds);
            m_fScale = MathHelper.Clamp(m_fScale, 0.0f, m_fScaleMax);

            m_cSprite.m_fScale = m_fScale;
        }

        public void UpdateColor(GameTime gameTime)
        {
            if ((m_iAge > m_iFadeAge) && (m_iFadeAge != 0))
            {
                m_cColor = m_cInitColor;
            }
            else
            {
                var amtInit = m_iAge/(float) m_iFadeAge;
                var amtFinal = 1.0f - amtInit;

                m_cColor.R = (byte) ((amtInit*m_cInitColor.R) + (amtFinal*m_cFinalColor.R));
                m_cColor.G = (byte) ((amtInit*m_cInitColor.G) + (amtFinal*m_cFinalColor.G));
                m_cColor.B = (byte) ((amtInit*m_cInitColor.B) + (amtFinal*m_cFinalColor.B));
                m_cColor.A = (byte) ((amtInit*m_cInitColor.A) + (amtFinal*m_cFinalColor.A));
            }

            m_cSprite.m_cColor = m_cColor;
        }

        public void Update(GameTime gameTime)
        {
            if (m_iAge < 0)
                return;
            m_iAge -= gameTime.ElapsedGameTime.Milliseconds;

            UpdatePos(gameTime);
            UpdateRot(gameTime);
            UpdateScale(gameTime);
            UpdateColor(gameTime);
        }

        public void Draw(SpriteBatch batch)
        {
            if (m_iAge < 0)
                return;

            m_cSprite.Draw(batch);
        }
    }
}