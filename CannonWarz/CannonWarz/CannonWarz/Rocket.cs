using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CannonWarz
{
    public class Rocket
    {
        public Rocket(Texture2D rocketTexture, Texture2D smokeTexture, Vector2 initPlayerPosition, float initPlayerAngle, float initPlayerPower)
        {
            this.rocketTexture = rocketTexture;
            rocketColorArray = Game1.TextureTo2DArray(rocketTexture);
            
            rocketPosition = initPlayerPosition;
            // Adjustements so the rocket is positioned properly on the cannon at first
            rocketPosition.X += 30;
            rocketPosition.Y -= 15;
            
            rocketAngle = initPlayerAngle;
            
            // We create a vector "up" and multiply it by a rotation matrix to get the rocket's real direction
            Vector2 up = new Vector2(0, -1);
            Matrix rotMatrix = Matrix.CreateRotationZ(rocketAngle);
            rocketDirection = Vector2.Transform(up, rotMatrix);
            rocketDirection *= initPlayerPower / 50.0f;

            rocketScaling = 0.1f;
            
            this.smokeTexture = smokeTexture;
            smokeParticleList = new List<Vector2>();
            randSmokeDeviation = new Random();
            smokeScaling = 0.2f;
        }

        public Rocket(Texture2D rocketTexture, Texture2D smokeTexture, Vector2 initPosition, float initAngle, Vector2 initDirection)
        {
            this.rocketTexture = rocketTexture;
            rocketColorArray = Game1.TextureTo2DArray(rocketTexture);
            rocketPosition = initPosition;
            rocketAngle = initAngle;
            rocketDirection = initDirection;
            rocketScaling = 0.1f;

            this.smokeTexture = smokeTexture;
            smokeParticleList = new List<Vector2>();
            randSmokeDeviation = new Random();
            smokeScaling = 0.2f;
        }

        public void CreateSmokeParticles(int particleQuantity)
        {
            // We create smoke particles and generate a random pattern to them
            for (int i = 0; i < particleQuantity; i++) // Raise the value left of the < if you want more smoke
            {
                Vector2 smokePos = rocketPosition;
                smokePos.X += randSmokeDeviation.Next(10) - 5;
                smokePos.Y += randSmokeDeviation.Next(10) - 5;
                smokeParticleList.Add(smokePos);
            }
        }

        public void DeleteSmokeParticles()
        {
            smokeParticleList = new List<Vector2>();
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private Texture2D rocketTexture;
        private Color[,] rocketColorArray;
        private Vector2 rocketPosition;
        private Vector2 rocketDirection;
        private float rocketAngle;
        private float rocketScaling;

        private Texture2D smokeTexture;
        private List<Vector2> smokeParticleList;
        private Random randSmokeDeviation;
        private float smokeScaling;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public Vector2 RocketPosition
        {
            get { return rocketPosition; }
            set { rocketPosition = value; }
        }

        public Vector2 RocketDirection
        {
            get { return rocketDirection; }
            set { rocketDirection = value; }
        }

        public float RocketAngle
        {
            get { return rocketAngle; }
            set { rocketAngle = value; }
        }
        
        public Texture2D RocketTexture
        {
            get { return rocketTexture; }
            set
            {
                rocketTexture = value;
                rocketColorArray = Game1.TextureTo2DArray(rocketTexture);
            }
        }

        public Color[,] RocketColorArray
        {
            get { return rocketColorArray; }
        }

        public float RocketScaling
        {
            get { return rocketScaling; }
            set { rocketScaling = value; }
        }

        public Texture2D SmokeTexture
        {
            get { return smokeTexture; }
            set { smokeTexture = value; }
        }

        public List<Vector2> SmokeParticleList
        {
            get { return smokeParticleList; }
            set { smokeParticleList = value; }
        }

        public float SmokeScaling
        {
            get { return smokeScaling; }
            set { smokeScaling = value; }
        }

        #endregion
    }
}
