/**
// file:	Terrain.cs
//
// summary:	Implements the terrain class
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    /**
     * <summary>    Terrain class. Represents the terrain in the game. </summary>
     */
    public class Terrain
    {        
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="gameInstance">  The game instance. </param>
         * <param name="groundTexture"> The ground texture. </param>
         */
        public Terrain(Game1 gameInstance, Texture2D groundTexture)
        {
            Game = gameInstance;

            this._groundTexture = groundTexture;
            _groundColorArray = Game1.TextureTo2DArray(groundTexture);

            // We create the foregroundColorArray array and initialize it to black everywhere
            _foregroundColorArray = new Color[groundTexture.Width, groundTexture.Height];
            for (int x = 0; x < groundTexture.Width; x++)
            {
                for (int y = 0; y < groundTexture.Height; y++)
                {
                    _foregroundColorArray[x,y] = Color.Black;
                }
            }
            
            // We create the terrainContour array and initialize it to 0
            TerrainContour = new int[Game.ScreenWidth];
            for (int x = 0; x < Game.ScreenWidth; x++)
            {
                TerrainContour[x] = 0;
            }

            WindDirection = new Vector2();
        }

        /**
         * <summary>    Generates the terrain's contour. </summary>
         */
        public void GenerateTerrainContour()
        {
            TerrainContour = new int[Game.ScreenWidth];

            float offset = Game.ScreenHeight / 2;    // Raise this value to lower the ground
            float peakheight = 100;             // Raise this value to have higher mountains
            float flatness = 70;                // Raise this value to have a more flat terrain (mountains more "rounded")

            Random randWaveMaker = new Random();
            double randPlus1 = randWaveMaker.NextDouble() + 1;
            double randPlus2 = randWaveMaker.NextDouble() + 2;
            double randPlus3 = randWaveMaker.NextDouble() + 3;

            for (int x = 0; x < Game.ScreenWidth; x++)
            {
                double height = peakheight / randPlus1 * Math.Sin((float)x / flatness * randPlus1 + randPlus1);
                height += peakheight / randPlus2 * Math.Sin((float)x / flatness * randPlus2 + randPlus2);
                height += peakheight / randPlus3 * Math.Sin((float)x / flatness * randPlus3 + randPlus3);
                height += offset;
                TerrainContour[x] = (int)height;
            }
        }

        /**
         * <summary>    Creates the terrain's foreground. </summary>
         *
         * <param name="device">    The graphics device. </param>
         */
        public void CreateForeground(GraphicsDevice device)
        {
            Color[] foregroundColors = new Color[Game.ScreenWidth * Game.ScreenHeight];
            Texture2D foregroundTexture = new Texture2D(device, Game.ScreenWidth, Game.ScreenHeight, false, SurfaceFormat.Color);

            // We set the color of each pixel on the screen
            for (int x = 0; x < Game.ScreenWidth; x++)
            {
                for (int y = 0; y < Game.ScreenHeight; y++)
                {
                    if (y > TerrainContour[x])
                    {
                        // We use modulo so that the indexes so we are sure the groundColorInfo indexes don't exceed that of the array
                        // This will happen if the screen is bigger than the texture
                        foregroundColors[x + y * Game.ScreenWidth] = _groundColorArray[x % _groundTexture.Width, y % _groundTexture.Height];
                    }
                    else
                    {
                        foregroundColors[x + y * Game.ScreenWidth] = Color.Transparent;
                    }
                }
            }

            // We create the texture out of the array's information
            foregroundTexture.SetData(foregroundColors);

            // We set the foregroundTexture field
            this._foregroundTexture = foregroundTexture;

            // We set the foregroundColorArrayField
            this._foregroundColorArray = Game1.TextureTo2DArray(foregroundTexture);
        }

        /**
         * <summary>    Flatten the terrain's contour below the players' positions. </summary>
         *
         * <param name="playerArray">   Array of players. </param>
         */
        public void FlattenTerrainBelowPlayers(AbsPlayer[] playerArray)
        {
            foreach (AbsPlayer player in playerArray)
            {
                if (player.IsAlive)
                {
                    // TODO: Make x < 65 vary in function of the player scaling
                    for (int x = 0; x < 65; x++)    // The cannon with the carriage are 60 pixels wide + 5 pixels of "free space"
                    {
                        this.TerrainContour[(int)player.Position.X + x] = this.TerrainContour[(int)player.Position.X];
                    }
                }
            }
        }

        /**
         * <summary>    Generates the wind's direction randomply. </summary>
         */
        public void GenerateWindDirection()
        {
            Random randomFactor = new Random();

            float rads = (float)(randomFactor.NextDouble() * Math.PI * 2);

            WindDirection = new Vector2((float)(Math.Cos(rads) * 50.0f), 0);
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private Texture2D _foregroundTexture;
        private Color[,] _foregroundColorArray;
        private Texture2D _groundTexture;
        private Color[,] _groundColorArray;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the game's instance. </summary>
         *
         * <value>  The game's instance. </value>
         */
        private Game1 Game
        {
            get;
            set;
        }

        /**
         * <summary>    
         *  Gets or sets the terrain contour array. Each index of the array represents an X coordinate
         *  of the screen. The value found at that index in the array is the height (Y coordinate) 
         *  of the terrain contour at that X coordinate of the screen.
         * </summary>
         *
         * <value>  The terrain contour array. </value>
         */
        public int[] TerrainContour
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the wind direction. </summary>
         *
         * <value>  The wind direction. </value>
         */
        public Vector2 WindDirection
        {
            get;
            set;
        }

        /**
         * <summary>    
         *  Gets or sets the foreground texture. Modifying this property will also modify the
         *  ForegroundColorArray property.
         * </summary>
         *
         * <value>  The foreground texture. </value>
         */
        public Texture2D ForegroundTexture
        {
            get { return _foregroundTexture; }
            set
            {
                _foregroundTexture = value;
                _foregroundColorArray = Game1.TextureTo2DArray(_foregroundTexture);
            }
        }

        /**
         * <summary>    Gets the 2D array of color representing the foreground texture. </summary>
         *
         * <value>  A 2D color array equivalent to the foreground texture. </value>
         */
        public Color[,] ForegroundColorArray
        {
            get { return _foregroundColorArray; }
        }

        /**
         * <summary>    
         *  Gets or sets the ground texture. Modifying this property will also modify the
         *  GroundColorArray property.            
         * </summary>
         *
         * <value>  The ground texture. </value>
         */
        public Texture2D GroundTexture
        {
            get { return _groundTexture; }
            set
            {
                _groundTexture = value;
                _groundColorArray = Game1.TextureTo2DArray(_groundTexture);
            }
        }

        /**
         * <summary>    Gets the 2D array of color representing the ground texture. </summary>
         *
         * <value>  A 2D color array equivalent to the ground texture. </value>
         */
        public Color[,] GroundColorArray
        {
            get { return _groundColorArray; }
        }

        #endregion
    }
}
