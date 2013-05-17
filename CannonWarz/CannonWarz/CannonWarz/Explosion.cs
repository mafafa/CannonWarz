/**
// file:	Explosion.cs
//
// summary:	Implements the explosion class
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    /**
     * <summary>    Explosion class. Generates and keeps track of explosion particles. </summary>
     */
    public class Explosion
    {
        /**
         * <summary>    Particle data structure. </summary>
         */
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

        /**
         * <summary>    Constructor for the explosion. </summary>
         *
         * <param name="explosionParticleTexture">  The explosion particle texture. </param>
         */
        public Explosion(Texture2D explosionParticleTexture)
        {
            this._explosionParticleTexture = explosionParticleTexture;
            _particleList = new List<ParticleData>();
        }

        /**
         * <summary>    Adds an explosion on the screen. </summary>
         *
         * <param name="explosionPos">      The explosion position. </param>
         * <param name="numberOfParticles"> Number of particles. </param>
         * <param name="size">              The size of the explosion. </param>
         * <param name="maxAge">            The maximum age for the particles. </param>
         * <param name="gameTime">          Time of the game. </param>
         */
        public void AddExplosion(Vector2 explosionPos, int numberOfParticles, float size, float maxAge, GameTime gameTime)
        {
            for (int i = 0; i < numberOfParticles; i++)
            {
                AddExplosionParticle(explosionPos, size, maxAge, gameTime);
            }
        }

        /**
         * <summary>    Creates particles for an explosion. </summary>
         *
         * <param name="explosionPos">  The explosion position. </param>
         * <param name="explosionSize"> The size of the explosion. </param>
         * <param name="maxAge">        The maximum age for the particles. </param>
         * <param name="gameTime">      Time of the game. </param>
         */
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

            _particleList.Add(particle);
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private Texture2D _explosionParticleTexture;
        private List<ParticleData> _particleList;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the explosionparticle texture. </summary>
         *
         * <value>  The explosionparticle texture. </value>
         */
        public Texture2D ExplosionparticleTexture
        {
            get { return _explosionParticleTexture; }
            set { _explosionParticleTexture = value; }
        }

        /**
         * <summary>    Gets or sets the list of currently active particles. </summary>
         *
         * <value>  A List of currently active particles. </value>
         */
        public List<ParticleData> ParticleList
        {
            get { return _particleList; }
            set { _particleList = value; }
        }

        #endregion
    }
}
