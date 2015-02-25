using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleEffects
{
    public class EffectManager
    {
        public List<Effect> m_lAllEffects;

        public EffectManager()
        {
            m_lAllEffects = new List<Effect>();
        }

        public void LoadContent(ContentManager Content)
        {
            Effect.LoadContent(Content);
        }

        public void AddEffect(eEffectType type)
        {
            var tempEffect = new Effect();
            tempEffect.Initialize(type);
            m_lAllEffects.Add(tempEffect);
        }

        public void Update(GameTime gameTime)
        {
            for (var i = m_lAllEffects.Count() - 1; i >= 0; i--)
            {
                m_lAllEffects[i].Update(gameTime);

                if (!m_lAllEffects[i].isAlive())
                    m_lAllEffects.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var e in m_lAllEffects)
            {
                e.Draw(batch);
            }
        }
    }
}