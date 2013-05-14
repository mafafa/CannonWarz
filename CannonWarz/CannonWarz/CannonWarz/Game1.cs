using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using CannonWarz.Screens;

namespace CannonWarz
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // We set the game's window attributes
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
            Window.Title = "My First Game";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {            
            base.Initialize();

            // We create the GameScreenManager and push the first two screens to be drawn (the Background and Main Menu)
            // Must be called after base.Initialize() as that will call LoadContent() and we need the textures loaded.
            GameScreenManager gameScreenManager = GameScreenManager.Create(this);
            gameScreenManager.Push(new Background(this, spriteBatch));
            //gameScreenManager.Push(new MainMenu(this, spriteBatch));
            gameScreenManager.Push(new GameScreen(this, spriteBatch));
            gameScreenManager.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // device cannot be initialized in the constructor for some reason
            device = graphics.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // We load the textures
            opaqueTexture = backgroundTexture = Content.Load<Texture2D>("Opaque");
            backgroundTexture = Content.Load<Texture2D>("GameBackground");
            groundTexture = Content.Load<Texture2D>("SandTexture");
            carriageTexture = Content.Load<Texture2D>("Carriage");
            cannonTexture = Content.Load<Texture2D>("Cannon");
            lifeBarTexture = Content.Load<Texture2D>("LifeBar");
            greenBarTexture = Content.Load<Texture2D>("LifeGreen");
            yellowBarTexture = Content.Load<Texture2D>("LifeYellow");
            redBarTexture = Content.Load<Texture2D>("LifeRed");
            rocketTexture = Content.Load<Texture2D>("Rocket");
            smokeTexture = Content.Load<Texture2D>("Smoke");
            compassTexture = Content.Load<Texture2D>("Compass");
            pinTexture = Content.Load<Texture2D>("Pin");
            explosionParticleTextures = Content.Load<Texture2D>("Explosion");

            generalFont = Content.Load<SpriteFont>("screenWritingFont");
            menuFont = Content.Load<SpriteFont>("menuFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameScreenManager gameScreenManager = GameScreenManager.Instance;

            if (gameScreenManager.Initialized)
            {
                gameScreenManager.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GameScreenManager gameScreenManager = GameScreenManager.Instance;

            if (gameScreenManager.Initialized)
            {
                gameScreenManager.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public static Color[,] TextureTo2DArray(Texture2D texture)
        {
            // We get the texture information and store it in a 1D array
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            // For convenience, we then rearrange that information in a 2D array so that the
            // indexes correspond to each pixel in the texture
            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    colors2D[x, y] = colors1D[x + y * texture.Width];
                }
            }

            return colors2D;
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private GraphicsDeviceManager graphics;
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;

        private int screenWidth;
        private int screenHeight;

        private SpriteFont generalFont;
        private SpriteFont menuFont;

        private Texture2D opaqueTexture;

        private Texture2D groundTexture;
        private Texture2D backgroundTexture;

        private Texture2D compassTexture;
        private Texture2D pinTexture;

        private Texture2D rocketTexture;
        private Texture2D smokeTexture;

        private Texture2D carriageTexture;
        private Texture2D cannonTexture;
        private Texture2D lifeBarTexture;
        private Texture2D greenBarTexture;
        private Texture2D yellowBarTexture;
        private Texture2D redBarTexture;

        private Texture2D explosionParticleTextures;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public int ScreenHeight
        {
            get { return screenHeight; }
        }

        public int ScreenWidth
        {
            get { return screenWidth; }
        }

        public SpriteFont GeneralFont
        {
            get { return generalFont; }
        }

        public SpriteFont MenuFont
        {
            get { return menuFont; }
        }

        public Texture2D OpaqueTexture
        {
            get { return opaqueTexture; }
        }

        public Texture2D GroundTexture
        {
            get { return groundTexture; }
        }

        public Texture2D BackgroundTexture
        {
            get { return backgroundTexture; }
        }

        public Texture2D CompassTexture
        {
            get { return compassTexture; }
        }

        public Texture2D PinTexture
        {
            get { return pinTexture; }
        }

        public Texture2D RocketTexture
        {
            get { return rocketTexture; }
        }

        public Texture2D SmokeTexture
        {
            get { return smokeTexture; }
        }

        public Texture2D CarriageTexture
        {
            get { return carriageTexture; }
        }

        public Texture2D CannonTexture
        {
            get { return cannonTexture; }
        }

        public Texture2D LifeBarTexture
        {
            get { return lifeBarTexture; }
        }

        public Texture2D GreenBarTexture
        {
            get { return greenBarTexture; }
        }

        public Texture2D YellowBarTexture
        {
            get { return yellowBarTexture; }
        }

        public Texture2D RedBarTexture
        {
            get { return redBarTexture; }
        }

        public Texture2D ExplosionParticleTextures
        {
            get { return explosionParticleTextures; }
        }

        #endregion
    }
}
