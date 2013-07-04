/**
// file:	AbsPlayer.cs
//
// summary:	Implements the abstract player class
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    /**
     * <summary>    Abstract player class. Human and AI players derive from this class. </summary>
     */
    public abstract class AbsPlayer
    {
        /**
         * <summary>    Specialised constructor for use only by derived classes. </summary>
         *
         * <param name="color">             The color of the player. </param>
         * <param name="carriageTexture">   The carriage texture. </param>
         * <param name="cannonTexture">     The cannon texture. </param>
         * <param name="lifeBarTexture">    The life bar texture. </param>
         * <param name="greenBarTexture">   The green bar texture. </param>
         * <param name="yellowBarTexture">  The yellow bar texture. </param>
         * <param name="redBarTexture">     The red bar texture. </param>
         */
        protected AbsPlayer(Color color, Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
        {
            IsAlive = true;
            Angle = 90;
            Power = 100;
            HP = 3;
            Color = color;

            this._playerScaling = 60.0f / (float)carriageTexture.Width; ;
            this._carriageTexture = carriageTexture;
            this._cannonTexture = cannonTexture;
            _carriageColorArray = Game1.TextureTo2DArray(carriageTexture);
            _cannonColorArray = Game1.TextureTo2DArray(cannonTexture);
            
            this._lifeBarTexture = lifeBarTexture;
            this._greenBarTexture = greenBarTexture;
            this._yellowBarTexture = yellowBarTexture;
            this._redBarTexture = redBarTexture;
            _lifeBarScaling = 0.1f;
            _hpBarScaling = 0.1f;

            if (_colorNameDictionary == null)
            {
                CreateColorNameDictionary();
            }
        }

        /**
         * <summary>    Places the players on the screen. </summary>
         *
         * <param name="numberOfPlayers">       Number of players. </param>
         * <param name="numberOfCurrPlayer">    Current player's number. </param>
         * <param name="screenWidth">           Width of the screen. </param>
         * <param name="terrain">               The terrain. </param>
         */
        public void PlacePlayer(int numberOfPlayers, int numberOfCurrPlayer, int screenWidth, Terrain terrain)
        {
            Vector2 position = new Vector2();

            // We divide the screen in (numberOfPlayers + 1) sections and place the player there at the Y height of that position
            position.X = (screenWidth / (numberOfPlayers + 1)) * (numberOfCurrPlayer + 1);
            position.Y = terrain.TerrainContour[(int)position.X];

            Position = position;
        }

        /**
         * <summary>    Gets the player's color name. </summary>
         *
         * <returns>    The player's color name. </returns>
         */
        public String GetPlayerColorName()
        {
            String colorName;

            if (_colorNameDictionary.TryGetValue(Color.PackedValue, out colorName))
            {
                return colorName;
            }

            else
            {
                return "";
            }
        }

        /**
         * <summary>    
         *  Creates the _colorNameDictionary field. The dictionary has the R,G,B,A packed value as key
         *  and the color's name as value.
         * </summary>
         */
        private void CreateColorNameDictionary()
        {
            _colorNameDictionary = new Dictionary<uint, String>(10);

            // We create the dictionnary with the possible player colors
            _colorNameDictionary.Add(Color.Red.PackedValue, "Red");
            _colorNameDictionary.Add(Color.Green.PackedValue, "Green");
            _colorNameDictionary.Add(Color.Blue.PackedValue, "Blue");
            _colorNameDictionary.Add(Color.Purple.PackedValue, "Purple");
            _colorNameDictionary.Add(Color.Orange.PackedValue, "Orange");
            _colorNameDictionary.Add(Color.Indigo.PackedValue, "Indigo");
            _colorNameDictionary.Add(Color.Yellow.PackedValue, "Yellow");
            _colorNameDictionary.Add(Color.SaddleBrown.PackedValue, "SaddleBrown");
            _colorNameDictionary.Add(Color.Tomato.PackedValue, "Tomato");
            _colorNameDictionary.Add(Color.Turquoise.PackedValue, "Turquoise");
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        protected Texture2D _carriageTexture;
        protected Color[,] _carriageColorArray;      
        protected Texture2D _cannonTexture;
        protected Color[,] _cannonColorArray;
        protected Texture2D _lifeBarTexture;
        protected float _lifeBarScaling;
        protected Texture2D _greenBarTexture;
        protected Texture2D _yellowBarTexture;
        protected Texture2D _redBarTexture;
        protected float _hpBarScaling;
        protected float _playerScaling;

        private static Dictionary<uint, String> _colorNameDictionary;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the position of the player. </summary>
         *
         * <value>  The position of the player on the terrain. </value>
         */
        public Vector2 Position
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets a value indicating whether this player is alive. </summary>
         *
         * <value>  true if this player is alive, false if not. </value>
         */
        public bool IsAlive
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the current hp of the player. </summary>
         *
         * <value>  The hp value. </value>
         */
        public uint HP
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the color of the player. </summary>
         *
         * <value>  The color of the player. </value>
         */
        public Color Color
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the angle of the player's shot in degrees. </summary>
         *
         * <value>  The angle of the player's shot in degrees. </value>
         */
        public int Angle
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the power of the player's shot. </summary>
         *
         * <value>  The power of the player's shot. </value>
         */
        public float Power
        {
            get;
            set;
        }

        /**
         * <summary>    
         *     Gets or sets the carriage texture. This will also modify the CarriageColorArray
         *     property. 
         * </summary>
         *
         * <value>  The carriage texture. </value>
         */
        public Texture2D CarriageTexture
        {
            get { return _carriageTexture; }
            set
            {
                _carriageTexture = value;
                _carriageColorArray = Game1.TextureTo2DArray(_carriageTexture);
            }
        }

        /**
         * <summary>    Gets the 2D array of colors representing the carriage's texture. </summary>
         *
         * <value>  A 2D array of colors. </value>
         */
        public Color[,] CarriageColorArray
        {
            get { return _carriageColorArray; }
        }

        /**
         * <summary>    
         *     Gets or sets the cannon texture. This will also modify the CannonColorArray 
         *     property.
         * </summary>
         *
         * <value>  The cannon texture. </value>
         */
        public Texture2D CannonTexture
        {
            get { return _cannonTexture; }
            set
            {
                _cannonTexture = value;
                _cannonColorArray = Game1.TextureTo2DArray(_cannonTexture);
            }
        }

        /**
         * <summary>    Gets the 2D array of colors representing the cannon's texture. </summary>
         *
         * <value>  An Array of cannon colors. </value>
         */
        public Color[,] CannonColorArray
        {
            get { return _cannonColorArray; }
        }

        /**
         * <summary>    Gets or sets the life bar texture. </summary>
         *
         * <value>  The life bar texture. </value>
         */
        public Texture2D LifeBarTexture
        {
            get { return _lifeBarTexture; }
            set { _lifeBarTexture = value; }
        }

        /**
         * <summary>    Gets or sets the life bar scaling. </summary>
         *
         * <value>  The life bar scaling. </value>
         */
        public float LifeBarScaling
        {
            get { return _lifeBarScaling; }
            set { _lifeBarScaling = value; }
        }

        /**
         * <summary>    Gets or sets the green bar texture. </summary>
         *
         * <value>  The green bar texture. </value>
         */
        public Texture2D GreenBarTexture
        {
            get { return _greenBarTexture; }
            set { _greenBarTexture = value; }
        }

        /**
         * <summary>    Gets or sets the yellow bar texture. </summary>
         *
         * <value>  The yellow bar texture. </value>
         */
        public Texture2D YellowBarTexture
        {
            get { return _yellowBarTexture; }
            set { _yellowBarTexture = value; }
        }

        /**
         * <summary>    Gets or sets the red bar texture. </summary>
         *
         * <value>  The red bar texture. </value>
         */
        public Texture2D RedBarTexture
        {
            get { return _redBarTexture; }
            set { _redBarTexture = value; }
        }

        /**
         * <summary>    Gets or sets the hp bar scaling. </summary>
         *
         * <value>  The hp bar scaling. </value>
         */
        public float HPBarScaling
        {
            get { return _hpBarScaling; }
            set { _hpBarScaling = value; }
        }

        /**
         * <summary>    Gets or sets the player scaling. </summary>
         *
         * <value>  The player scaling. </value>
         */
        public float PlayerScaling
        {
            get { return _playerScaling; }
            set { _playerScaling = value; }
        }

        #endregion
    }
}
