using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CannonWarz
{
    public abstract class AbsPlayer
    {
        protected AbsPlayer(Color color, Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
        {
            IsAlive = true;
            Angle = MathHelper.ToRadians(90);
            Power = 100;
            HP = 3;
            Color = color;

            this.playerScaling = 60.0f / (float)carriageTexture.Width; ;
            this.carriageTexture = carriageTexture;
            this.cannonTexture = cannonTexture;
            carriageColorArray = Game1.TextureTo2DArray(carriageTexture);
            cannonColorArray = Game1.TextureTo2DArray(cannonTexture);
            
            this.lifeBarTexture = lifeBarTexture;
            this.greenBarTexture = greenBarTexture;
            this.yellowBarTexture = yellowBarTexture;
            this.redBarTexture = redBarTexture;
            lifeBarScaling = 0.1f;
            hpBarScaling = 0.1f;
        }

        public abstract void PlacePlayer(int numberOfPlayers, int numberOfCurrPlayer, int screenWidth, Terrain terrain);

        #region --------------------- PRIVATE FIELDS ---------------------

        protected Texture2D carriageTexture;
        protected Color[,] carriageColorArray;
        protected Texture2D cannonTexture;
        protected Color[,] cannonColorArray;
        protected Texture2D lifeBarTexture;
        protected float lifeBarScaling;
        protected Texture2D greenBarTexture;
        protected Texture2D yellowBarTexture;
        protected Texture2D redBarTexture;
        protected float hpBarScaling;
        protected float playerScaling;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /// <summary>
        /// The position of the player on the terrain
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Represents if the player is alive or not
        /// </summary>
        public bool IsAlive
        {
            get;
            set;
        }

        public uint HP
        {
            get;
            set;
        }

        /// <summary>
        /// The color of the player
        /// </summary>
        public Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// The angle of the player's shot
        /// </summary>
        public float Angle
        {
            get;
            set;
        }

        /// <summary>
        /// The power of the player's shot
        /// </summary>
        public float Power
        {
            get;
            set;
        }

        public Texture2D CarriageTexture
        {
            get { return carriageTexture; }
            set
            {
                carriageTexture = value;
                carriageColorArray = Game1.TextureTo2DArray(carriageTexture);
            }
        }

        public Color[,] CarriageColorArray
        {
            get { return carriageColorArray; }
        }

        public Texture2D CannonTexture
        {
            get { return cannonTexture; }
            set
            {
                cannonTexture = value;
                cannonColorArray = Game1.TextureTo2DArray(cannonTexture);
            }
        }

        public Color[,] CannonColorArray
        {
            get { return cannonColorArray; }
        }

        public Texture2D LifeBarTexture
        {
            get { return lifeBarTexture; }
            set { lifeBarTexture = value; }
        }

        public float LifeBarScaling
        {
            get { return lifeBarScaling; }
            set { lifeBarScaling = value; }
        }

        public Texture2D GreenBarTexture
        {
            get { return greenBarTexture; }
            set { greenBarTexture = value; }
        }

        public Texture2D YellowBarTexture
        {
            get { return yellowBarTexture; }
            set { yellowBarTexture = value; }
        }

        public Texture2D RedBarTexture
        {
            get { return redBarTexture; }
            set { redBarTexture = value; }
        }

        public float HPBarScaling
        {
            get { return hpBarScaling; }
            set { hpBarScaling = value; }
        }

        public float PlayerScaling
        {
            get { return playerScaling; }
            set { playerScaling = value; }
        }

        #endregion
    }
}
