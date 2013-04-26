using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    class Explosion
    {
         public struct ParticleData
         {
             public float birthTime;
             public float maxAge;
             public Vector2 orginalPosition;
             public Vector2 accelaration;
             public Vector2 direction;
             public Vector2 position;
             public float scaling;
             public Color modColor;
        }
        
        public Explosion(Texture2D explosionParticleTexture)
        {
            this.explosionParticleTexture = explosionParticleTexture;
            particleList = new List<ParticleData>();
        }

        public void AddExplosion(Vector2 explosionPos, int numberOfParticles, float size, float maxAge, GameTime gameTime)
        {
            for (int i = 0; i < numberOfParticles; i++)
            {
                AddExplosionParticle(explosionPos, size, maxAge, gameTime);
            }
        }

        private void AddExplosionParticle(Vector2 explosionPos, float explosionSize, float maxAge, GameTime gameTime)
        {
            ParticleData particle = new ParticleData();

            particle.orginalPosition = explosionPos;
            particle.position = particle.orginalPosition;

            particle.birthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            particle.maxAge = maxAge;
            particle.scaling = 0.25f;
            particle.modColor = Color.White;

            // Random particle direction
            Random randomizer = new Random();
            float particleDistance = (float)randomizer.NextDouble() * explosionSize;    // Maximum distance the particle should have travelled at the end of its life
            Vector2 displacement = new Vector2(particleDistance, 0);
            float angle = MathHelper.ToRadians(randomizer.Next(360));
            displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));

            particle.direction = displacement * 2.0f;
            particle.accelaration = -particle.direction;  

            particleList.Add(particle);
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private Texture2D explosionParticleTexture;
        private List<ParticleData> particleList;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public Texture2D ExplosionparticleTexture
        {
            get { return explosionParticleTexture; }
            set { explosionParticleTexture = value; }
        }

        public List<ParticleData> ParticleList
        {
            get { return particleList; }
            set { particleList = value; }
        }

        #endregion
    }
}
