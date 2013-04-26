using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    /// <summary>
    /// Terrain class. Represents the terrain in the game.
    /// </summary>
    public class Terrain
    {        
        public Terrain(Game1 gameInstance, Texture2D groundTexture)
        {
            GameInstance = gameInstance;
            
            ScreenHeight = gameInstance.ScreenHeight;
            ScreenWidth = gameInstance.ScreenWidth;

            this.groundTexture = groundTexture;
            groundColorArray = Game1.TextureTo2DArray(groundTexture);

            // We create the foregroundColorArray array and initialize it to black everywhere
            foregroundColorArray = new Color[groundTexture.Width, groundTexture.Height];
            for (int x = 0; x < groundTexture.Width; x++)
            {
                for (int y = 0; y < groundTexture.Height; y++)
                {
                    foregroundColorArray[x,y] = Color.Black;
                }
            }
            
            // We create the terrainContour array and initialize it to 0
            TerrainContour = new int[ScreenWidth];
            for (int x = 0; x < ScreenWidth; x++)
            {
                TerrainContour[x] = 0;
            }

            WindDirection = new Vector2();
        }

        public void GenerateTerrainContour()
        {
            TerrainContour = new int[ScreenWidth];
            
            float offset = ScreenHeight / 2;    // Raise this value to lower the ground
            float peakheight = 100;             // Raise this value to have higher mountains
            float flatness = 70;                // Raise this value to have a more flat terrain (mountains more "rounded")

            Random randWaveMaker = new Random();
            double randPlus1 = randWaveMaker.NextDouble() + 1;
            double randPlus2 = randWaveMaker.NextDouble() + 2;
            double randPlus3 = randWaveMaker.NextDouble() + 3;
            
            for (int x = 0; x < ScreenWidth; x++)
            {
                double height = peakheight / randPlus1 * Math.Sin((float)x / flatness * randPlus1 + randPlus1);
                height += peakheight / randPlus2 * Math.Sin((float)x / flatness * randPlus2 + randPlus2);
                height += peakheight / randPlus3 * Math.Sin((float)x / flatness * randPlus3 + randPlus3);
                height += offset;
                TerrainContour[x] = (int)height;
            }
        }

        public void CreateForeground(GraphicsDevice device)
        {
            Color[] foregroundColors = new Color[ScreenWidth * ScreenHeight];
            Texture2D foregroundTexture = new Texture2D(device, ScreenWidth, ScreenHeight, false, SurfaceFormat.Color);

            // We set the color of each pixel on the screen
            for (int x = 0; x < ScreenWidth; x++)
            {
                for (int y = 0; y < ScreenHeight; y++)
                {
                    if (y > TerrainContour[x])
                    {
                        // We use modulo so that the indexes so we are sure the groundColorInfo indexes don't exceed that of the array
                        // This will happen if the screen is bigger than the texture
                        foregroundColors[x + y * ScreenWidth] = groundColorArray[x % groundTexture.Width, y % groundTexture.Height];
                    }
                    else
                    {
                        foregroundColors[x + y * ScreenWidth] = Color.Transparent;
                    }
                }
            }

            // We create the texture out of the array's information
            foregroundTexture.SetData(foregroundColors);

            // We set the foregroundTexture field
            this.foregroundTexture = foregroundTexture;

            // We set the foregroundColorArrayField
            this.foregroundColorArray = Game1.TextureTo2DArray(foregroundTexture);
        }

        public void FlattenTerrainBelowPlayers(HumanPlayer[] playerArray)
        {
            foreach (HumanPlayer player in playerArray)
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

        public void GenerateWindDirection()
        {
            Random randomFactor = new Random();

            float rads = (float)(randomFactor.NextDouble() * Math.PI * 2);

            WindDirection = new Vector2((float)(Math.Cos(rads) / 50.0f), (float)(Math.Sin(rads) / 50.0f));
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private Texture2D foregroundTexture;
        private Color[,] foregroundColorArray;
        private Texture2D groundTexture;
        private Color[,] groundColorArray;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        private int ScreenHeight
        {
            get;
            set;
        }

        private int ScreenWidth
        {
            get;
            set;
        }

        private Game1 GameInstance
        {
            get;
            set;
        }

        public int[] TerrainContour
        {
            get;
            set;
        }

        public Vector2 WindDirection
        {
            get;
            set;
        }

        public Texture2D ForegroundTexture
        {
            get { return foregroundTexture; }
            set
            {
                foregroundTexture = value;
                foregroundColorArray = Game1.TextureTo2DArray(foregroundTexture);
            }
        }

        public Color[,] ForegroundColorArray
        {
            get { return foregroundColorArray; }
        }

        public Texture2D GroundTexture
        {
            get { return groundTexture; }
            set
            {
                groundTexture = value;
                groundColorArray = Game1.TextureTo2DArray(groundTexture);
            }
        }

        public Color[,] GroundColorArray
        {
            get { return groundColorArray; }
        }

        #endregion
    }
}
