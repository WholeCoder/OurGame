using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects
{
    public enum eEffectType
    {
        smoke,
        fire,
        explosion,
        snow,
        spiral
    }

    public class Effect
    {
        public static Texture2D snowflakeTexture;
        public static Texture2D circleTexture;
        public static Texture2D starTexture;
        public List<Particle> m_allParticles;
        public BlendState m_eBlendType;
        public eEffectType m_eType;
        public int m_iBurstCountdownMS;
        public int m_iBurstFrequencyMS;
        public int m_iEffectDuration;
        public int m_iNewParticleAmmount;
        public int m_iRadius;
        public Vector2 m_vOrigin;
        public Random myRandom;
        public Texture2D particleTexture;

        public Effect()
        {
            m_allParticles = new List<Particle>();
            myRandom = new Random();
        }

        public static void LoadContent(ContentManager content)
        {
            snowflakeTexture = content.Load<Texture2D>("snowFlake");
            circleTexture = content.Load<Texture2D>("whiteCircle");
            starTexture = content.Load<Texture2D>("whiteStar");
        }

        public bool isAlive()
        {
            if (m_iEffectDuration > 0)
                return true;
            if (m_allParticles.Count() > 0)
                return true;
            return false;
        }

        public void Initialize(eEffectType pType)
        {
            m_eType = pType;

            switch (m_eType)
            {
                case eEffectType.fire:
                    FireInitialize();
                    break;
                case eEffectType.smoke:
                    SmokeInitialize();
                    break;
                case eEffectType.explosion:
                    ExplosionInitialize();
                    break;
                case eEffectType.snow:
                    SnowInitialize();
                    break;
                case eEffectType.spiral:
                    SpiralInitialize();
                    break;
            }
        }

        public void SpiralInitialize()
        {
            //Explosion
            particleTexture = starTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 1;
            m_iBurstFrequencyMS = 64;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 100);
            m_iRadius = 50;
            m_eBlendType = BlendState.NonPremultiplied;
        }

        public void SnowInitialize()
        {
            //Explosion
            particleTexture = snowflakeTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 1;
            m_iBurstFrequencyMS = 64;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, -50);
            m_iRadius = 50;
            m_eBlendType = BlendState.NonPremultiplied;
        }

        public void FireInitialize()
        {
            //Fire
            particleTexture = circleTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 15;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 400);
            m_iRadius = 15;
            m_eBlendType = BlendState.Additive;
        }

        public void SmokeInitialize()
        {
            //Smoke
            particleTexture = circleTexture;
            m_iEffectDuration = 60000;
            m_iNewParticleAmmount = 4;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(640, 640);
            m_iRadius = 50;
            m_eBlendType = BlendState.Additive;
        }

        public void ExplosionInitialize()
        {
            //Explosion
            particleTexture = starTexture;
            m_iEffectDuration = 16;
            m_iNewParticleAmmount = 800;
            m_iBurstFrequencyMS = 16;
            m_iBurstCountdownMS = m_iBurstFrequencyMS;

            m_vOrigin = new Vector2(200, 720);
            m_iRadius = 20;
            m_eBlendType = BlendState.NonPremultiplied;
        }

        public void createParticle()
        {
            switch (m_eType)
            {
                case eEffectType.fire:
                    createFireParticle();
                    break;
                case eEffectType.smoke:
                    createSmokeParticle();
                    break;
                case eEffectType.explosion:
                    createExplosionParticle();
                    break;
                case eEffectType.snow:
                    createSnowParticle();
                    break;
                case eEffectType.spiral:
                    createSpiralParticle();
                    break;
            }
        }

        public void createSpiralParticle()
        {
            var initAge = 3000; //3 seconds

            var initPos = m_vOrigin;


            var initVel = new Vector2(((float) (100.0f*Math.Cos(m_iEffectDuration))),
                ((float) (100.0f*Math.Sin(m_iEffectDuration))));

            var initAcc = new Vector2(0, 75);
            var initDamp = 1.0f;

            var initRot = 0.0f;
            var initRotVel = 2.0f;
            var initRotDamp = 0.99f;

            var initScale = 0.2f;
            var initScaleVel = 0.2f;
            var initScaleAcc = -0.1f;
            var maxScale = 1.0f;

            var initColor = Color.DarkRed;
            var finalColor = Color.DarkRed;
            finalColor *= 0;
            //finalColor.A = 0;
            var fadeAge = initAge;

            var tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel,
                initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createFireParticle()
        {
            var initAge = 500 + myRandom.Next(500); //3 seconds
            var fadeAge = initAge - myRandom.Next(100);

            var initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float) (myRandom.Next(m_iRadius)*Math.Cos(myRandom.Next(360))));
            offset.Y = ((float) (myRandom.Next(m_iRadius)*Math.Sin(myRandom.Next(360))));
            initPos += offset;

            var offset2 = Vector2.Zero;
            offset2.X += (float) (200*Math.Cos(m_iEffectDuration/500.0f));
            initPos += offset2;

            var initVel = Vector2.Zero;
            initVel.X = -(offset.X);
            initVel.Y = -500;

            var initAcc = new Vector2(0, -myRandom.Next(300));

            var initDamp = 0.96f;

            var initRot = 0.0f;
            var initRotVel = 2.0f;
            var initRotDamp = 0.99f;

            var initScale = 0.5f;
            var initScaleVel = -0.1f;
            var initScaleAcc = 0.0f;
            var maxScale = 1.0f;

            var initColor = Color.DarkBlue;
            var finalColor = Color.DarkOrange;
            finalColor.A = 0;


            var tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel,
                initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createSmokeParticle()
        {
            var initAge = 5000 + myRandom.Next(5000);
            var fadeAge = initAge - myRandom.Next(5000);

            var initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float) (myRandom.Next(m_iRadius)*Math.Cos(myRandom.Next(360))));
            offset.Y = ((float) (myRandom.Next(m_iRadius)*Math.Sin(myRandom.Next(360))));
            initPos += offset;

            var offset2 = Vector2.Zero;
            offset2.X += (float) (400*Math.Cos(m_iEffectDuration/500.0f));
            initPos += offset2;

            var initVel = Vector2.Zero;
            initVel.X = 0; //
            initVel.Y = -30 - myRandom.Next(30);

            var initAcc = new Vector2(10 + myRandom.Next(10), 0);

            var initDamp = 1.0f;

            var initRot = 0.0f;
            var initRotVel = 0.0f;
            var initRotDamp = 1.0f;

            var initScale = 0.6f;
            var initScaleVel = myRandom.Next(10)/50.0f;
            var initScaleAcc = 0.0f;
            var maxScale = 3.0f;

            var initColor = Color.Black;
            initColor.A = 128;
            var finalColor = new Color(32, 32, 32);
            finalColor.A = 0;


            var tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel,
                initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createExplosionParticle()
        {
            var initAge = 3000 + myRandom.Next(5000);
            var fadeAge = initAge/2;

            var initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float) (myRandom.Next(m_iRadius)*Math.Cos(myRandom.Next(360))));
            offset.Y = ((float) (myRandom.Next(m_iRadius)*Math.Sin(myRandom.Next(360))));
            initPos += offset;

            var initVel = Vector2.Zero;
            initVel.X = myRandom.Next(500) + (offset.X*30);
            initVel.Y = -60*Math.Abs(offset.Y);

            var initAcc = new Vector2(0, 400);

            var initDamp = 1.0f;

            var initRot = 0.0f;
            var initRotVel = initVel.X/50.0f;
            var initRotDamp = 0.97f;

            var initScale = 0.1f + myRandom.Next(10)/50.0f;
            var initScaleVel = ((float) myRandom.Next(10) - 5)/50.0f;
            var initScaleAcc = 0.0f;
            var maxScale = 1.0f;

            var randomGray = (byte) (myRandom.Next(128) + 128);
            var initColor = new Color(randomGray, 0, 0);

            var finalColor = new Color(32, 32, 32);
            finalColor = Color.Black;

            var tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel,
                initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void createSnowParticle()
        {
            var initScale = 0.1f + myRandom.Next(10)/20.0f;
            var initScaleVel = 0.0f;
            var initScaleAcc = 0.0f;
            var maxScale = 1.0f;

            var initAge = (int) (10000/initScale);
            var fadeAge = initAge;

            var initPos = m_vOrigin;
            Vector2 offset;
            offset.X = ((float) (myRandom.Next(m_iRadius)*Math.Cos(myRandom.Next(360))));
            offset.Y = ((float) (myRandom.Next(m_iRadius)*Math.Sin(myRandom.Next(360))));
            initPos += offset;

            var offset2 = Vector2.Zero;
            offset2.X += (float) (600*Math.Cos(m_iEffectDuration/500.0));
            initPos += offset2;


            var initVel = Vector2.Zero;
            initVel.X = myRandom.Next(10) - 5;
            initVel.Y = 100*initScale;

            var initAcc = new Vector2(0, 0);

            var initDamp = 1.0f;

            var initRot = 0.0f;
            var initRotVel = initVel.X/5.0f;
            ;
            var initRotDamp = 1.0f;

            var initColor = Color.White;
            var finalColor = Color.White;
            finalColor.A = 0;

            var tempParticle = new Particle();
            tempParticle.Create(particleTexture, initAge, initPos, initVel, initAcc, initDamp, initRot, initRotVel,
                initRotDamp, initScale, initScaleVel, initScaleAcc, maxScale, initColor, finalColor, fadeAge);
            m_allParticles.Add(tempParticle);
        }

        public void Update(GameTime gameTime)
        {
            m_iEffectDuration -= gameTime.ElapsedGameTime.Milliseconds;
            m_iBurstCountdownMS -= gameTime.ElapsedGameTime.Milliseconds;

            if ((m_iBurstCountdownMS <= 0) && (m_iEffectDuration >= 0))
            {
                for (var i = 0; i < m_iNewParticleAmmount; i++)
                    createParticle();
                m_iBurstCountdownMS = m_iBurstFrequencyMS;
            }

            for (var i = m_allParticles.Count() - 1; i >= 0; i--)
            {
                m_allParticles[i].Update(gameTime);

                if (m_allParticles[i].m_iAge <= 0)
                    m_allParticles.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            //batch.Begin(SpriteSortMode.BackToFront, m_eBlendType);
            foreach (var p in m_allParticles)
            {
                p.Draw(batch);
            }
            //batch.End();
        }
    }
}