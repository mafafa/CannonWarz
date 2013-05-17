/**
// file:	Rocket.cs
//
// summary:	Implements the rocket class
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    /**
     * <summary>    The rocket class. </summary>
     */
    public class Rocket
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="rocketTexture">         The rocket texture. </param>
         * <param name="smokeTexture">          The rocket's smoke texture. </param>
         * <param name="initPlayerPosition">    The initial player's position. </param>
         * <param name="initPlayerAngle">       The initial player's shot angle. </param>
         * <param name="initPlayerPower">       The initial player's shot power. </param>
         */
        public Rocket(Texture2D rocketTexture, Texture2D smokeTexture, Vector2 initPlayerPosition, float initPlayerAngle, float initPlayerPower)
        {
            this._rocketTexture = rocketTexture;
            _rocketColorArray = Game1.TextureTo2DArray(rocketTexture);
            
            _rocketPosition = initPlayerPosition;
            // Adjustements so the rocket is positioned properly on the cannon at first
            _rocketPosition.X += 30;
            _rocketPosition.Y -= 15;
            
            _rocketAngle = initPlayerAngle;
            
            // We create a vector "up" and multiply it by a rotation matrix to get the rocket's real direction
            Vector2 up = new Vector2(0, -1);
            Matrix rotMatrix = Matrix.CreateRotationZ(_rocketAngle);
            _rocketDirection = Vector2.Transform(up, rotMatrix);
            _rocketDirection *= initPlayerPower / 50.0f;

            _rocketScaling = 0.1f;
            
            this._smokeTexture = smokeTexture;
            _smokeParticleList = new List<Vector2>();
            _randSmokeDeviation = new Random();
            _smokeScaling = 0.2f;
        }

        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="rocketTexture"> The rocket texture. </param>
         * <param name="smokeTexture">  The rocket's smoke texture. </param>
         * <param name="initPosition">  The initial player's position. </param>
         * <param name="initAngle">     The initial player's shot angle. </param>
         * <param name="initDirection"> The initial player's shot direction. </param>
         */
        public Rocket(Texture2D rocketTexture, Texture2D smokeTexture, Vector2 initPosition, float initAngle, Vector2 initDirection)
        {
            this._rocketTexture = rocketTexture;
            _rocketColorArray = Game1.TextureTo2DArray(rocketTexture);
            _rocketPosition = initPosition;
            _rocketAngle = initAngle;
            _rocketDirection = initDirection;
            _rocketScaling = 0.1f;

            this._smokeTexture = smokeTexture;
            _smokeParticleList = new List<Vector2>();
            _randSmokeDeviation = new Random();
            _smokeScaling = 0.2f;
        }

        /**
         * <summary>    Creates smoke particles for the rocket. </summary>
         *
         * <param name="particleQuantity">  The particle quantity. </param>
         */
        public void CreateSmokeParticles(int particleQuantity)
        {
            // We create smoke particles and generate a random pattern to them
            for (int i = 0; i < particleQuantity; i++) // Raise the value left of the < if you want more smoke
            {
                Vector2 smokePos = _rocketPosition;
                smokePos.X += _randSmokeDeviation.Next(10) - 5;
                smokePos.Y += _randSmokeDeviation.Next(10) - 5;
                _smokeParticleList.Add(smokePos);
            }
        }

        /**
         * <summary>    Deletes the smoke particles for the rocket. </summary>
         */
        public void DeleteSmokeParticles()
        {
            _smokeParticleList = new List<Vector2>();
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private Texture2D _rocketTexture;
        private Color[,] _rocketColorArray;
        private Vector2 _rocketPosition;
        private Vector2 _rocketDirection;
        private float _rocketAngle;
        private float _rocketScaling;

        private Texture2D _smokeTexture;
        private List<Vector2> _smokeParticleList;
        private Random _randSmokeDeviation;
        private float _smokeScaling;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the rocket position. </summary>
         *
         * <value>  The rocket position. </value>
         */
        public Vector2 RocketPosition
        {
            get { return _rocketPosition; }
            set { _rocketPosition = value; }
        }

        /**
         * <summary>    Gets or sets the rocket direction. </summary>
         *
         * <value>  The rocket direction. </value>
         */
        public Vector2 RocketDirection
        {
            get { return _rocketDirection; }
            set { _rocketDirection = value; }
        }

        /**
         * <summary>    Gets or sets the rocket angle. </summary>
         *
         * <value>  The rocket angle. </value>
         */
        public float RocketAngle
        {
            get { return _rocketAngle; }
            set { _rocketAngle = value; }
        }

        /**
         * <summary>    
         *  Gets or sets the rocket texture. This will also modify the RocketColorArray
         *  property.
         * </summary>
         *
         * <value>  The rocket texture. </value>
         */
        public Texture2D RocketTexture
        {
            get { return _rocketTexture; }
            set
            {
                _rocketTexture = value;
                _rocketColorArray = Game1.TextureTo2DArray(_rocketTexture);
            }
        }

        /**
         * <summary>    Gets the 2D array of colors representing the rocket's texture. </summary>
         *
         * <value>  A 2D array of colors. </value>
         */
        public Color[,] RocketColorArray
        {
            get { return _rocketColorArray; }
        }

        /**
         * <summary>    Gets or sets the rocket scaling. </summary>
         *
         * <value>  The rocket scaling. </value>
         */
        public float RocketScaling
        {
            get { return _rocketScaling; }
            set { _rocketScaling = value; }
        }

        /**
         * <summary>    Gets or sets the rocket's smoke texture. </summary>
         *
         * <value>  The rocket's smoke texture. </value>
         */
        public Texture2D SmokeTexture
        {
            get { return _smokeTexture; }
            set { _smokeTexture = value; }
        }

        /**
         * <summary>    Gets or sets a list of active smoke particles. </summary>
         *
         * <value>  A list of active smoke particles. </value>
         */
        public List<Vector2> SmokeParticleList
        {
            get { return _smokeParticleList; }
            set { _smokeParticleList = value; }
        }

        /**
         * <summary>    Gets or sets the smoke scaling. </summary>
         *
         * <value>  The smoke scaling. </value>
         */
        public float SmokeScaling
        {
            get { return _smokeScaling; }
            set { _smokeScaling = value; }
        }

        #endregion
    }
}
