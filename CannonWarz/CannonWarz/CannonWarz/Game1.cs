/**
// file:	Game1.cs
//
// summary:	Implements the Game1 class
 */

using CannonWarz.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CannonWarz
{
    /**
     * <summary>    This is the main type of the game. </summary>
     */
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        /**
         * <summary>    Default constructor. </summary>
         */
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // We set the game's window attributes
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.IsFullScreen = false;
            Window.Title = "Cannon Warz";
        }

        /**
         * <summary>
         *     Allows the game to perform any initialization it needs to before starting to run.
         *     This is where it can query for any required services and load any non-graphic
         *     related content.  Calling base.Initialize will enumerate through any components
         *     and initialize them as well.
         * </summary>
         */
        protected override void Initialize()
        {            
            base.Initialize();

            // We create the GameScreenManager and push the first two screens to be drawn (the Background and Main Menu)
            // Must be called after base.Initialize() as that will call LoadContent() and we need the textures loaded.
            GameScreenManager gameScreenManager = GameScreenManager.Create(this);
            gameScreenManager.Push(new Background(this, _spriteBatch));
            gameScreenManager.Push(new MainMenu(this, _spriteBatch));
            gameScreenManager.Initialize();
        }

        /**
         * <summary>
         *     LoadContent will be called once per game and is the place to load
         *     all of your content.
         * </summary>
         */
        protected override void LoadContent()
        {
            // device cannot be initialized in the constructor for some reason
            _device = _graphics.GraphicsDevice;
            _screenWidth = _device.PresentationParameters.BackBufferWidth;
            _screenHeight = _device.PresentationParameters.BackBufferHeight;
            
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // We load the textures
            _opaqueTexture = _backgroundTexture = Content.Load<Texture2D>("Opaque");
            _backgroundTexture = Content.Load<Texture2D>("GameBackground");
            _groundTexture = Content.Load<Texture2D>("SandTexture");
            _carriageTexture = Content.Load<Texture2D>("Carriage");
            _cannonTexture = Content.Load<Texture2D>("Cannon");
            _lifeBarTexture = Content.Load<Texture2D>("LifeBar");
            _greenBarTexture = Content.Load<Texture2D>("LifeGreen");
            _yellowBarTexture = Content.Load<Texture2D>("LifeYellow");
            _redBarTexture = Content.Load<Texture2D>("LifeRed");
            _rocketTexture = Content.Load<Texture2D>("Rocket");
            _smokeTexture = Content.Load<Texture2D>("Smoke");
            _compassTexture = Content.Load<Texture2D>("Compass");
            _pinTexture = Content.Load<Texture2D>("Pin");
            _explosionParticleTextures = Content.Load<Texture2D>("Explosion");

            _generalFont = Content.Load<SpriteFont>("screenWritingFont");
            _menuFont = Content.Load<SpriteFont>("menuFont");
        }

        /**
         * <summary>
         *  Called when graphics resources need to be unloaded. Override this method to unload any game-
         *  specific graphics resources.
         * </summary>
         */
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /**
         * <summary>
         *  Allows the game to run logic such as updating the world, checking for collisions, gathering
         *  input, and playing audio.
         * </summary>
         *
         * <param name="gameTime">  Provides a snapshot of timing values. </param>
         */        
        protected override void Update(GameTime gameTime)
        {
            GameScreenManager gameScreenManager = GameScreenManager.Instance;

            if (gameScreenManager.Initialized)
            {
                gameScreenManager.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /**
         * <summary>    This is called when the game should draw itself. </summary>
         *
         * <param name="gameTime">  Provides a snapshot of timing values. </param>
         */        
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

        /**
         * <summary>    Transforms a texture to its 2D color array equivalent. </summary>
         *
         * <param name="texture">   The texture to transform. </param>
         *
         * <returns>    The 2D color array equivalent to the texture. </returns>
         */
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

        private GraphicsDeviceManager _graphics;
        private GraphicsDevice _device;
        private SpriteBatch _spriteBatch;

        private int _screenWidth;
        private int _screenHeight;

        private SpriteFont _generalFont;
        private SpriteFont _menuFont;

        private Texture2D _opaqueTexture;

        private Texture2D _groundTexture;
        private Texture2D _backgroundTexture;

        private Texture2D _compassTexture;
        private Texture2D _pinTexture;

        private Texture2D _rocketTexture;
        private Texture2D _smokeTexture;

        private Texture2D _carriageTexture;
        private Texture2D _cannonTexture;
        private Texture2D _lifeBarTexture;
        private Texture2D _greenBarTexture;
        private Texture2D _yellowBarTexture;
        private Texture2D _redBarTexture;

        private Texture2D _explosionParticleTextures;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets the height of the screen. </summary>
         *
         * <value>  The height of the screen in pixel. </value>
         */
        public int ScreenHeight
        {
            get { return _screenHeight; }
        }

        /**
         * <summary>    Gets the width of the screen. </summary>
         *
         * <value>  The width of the screen in pixel. </value>
         */
        public int ScreenWidth
        {
            get { return _screenWidth; }
        }

        /**
         * <summary>    Gets the general font used for the game. </summary>
         *
         * <value>  The general font used for the game. </value>
         */
        public SpriteFont GeneralFont
        {
            get { return _generalFont; }
        }

        /**
         * <summary>    Gets the font used for the menus. </summary>
         *
         * <value>  The font used for the menus. </value>
         */
        public SpriteFont MenuFont
        {
            get { return _menuFont; }
        }

        /**
         * <summary>    Gets the opaque texture. </summary>
         *
         * <value>  The opaque texture. </value>
         */
        public Texture2D OpaqueTexture
        {
            get { return _opaqueTexture; }
        }

        /**
         * <summary>    Gets the ground texture. </summary>
         *
         * <value>  The ground texture. </value>
         */
        public Texture2D GroundTexture
        {
            get { return _groundTexture; }
        }

        /**
         * <summary>    Gets the background texture. </summary>
         *
         * <value>  The background texture. </value>
         */
        public Texture2D BackgroundTexture
        {
            get { return _backgroundTexture; }
        }

        /**
         * <summary>    Gets the compass texture. </summary>
         *
         * <value>  The compass texture. </value>
         */
        public Texture2D CompassTexture
        {
            get { return _compassTexture; }
        }

        /**
         * <summary>    Gets the pin texture. </summary>
         *
         * <value>  The pin texture. </value>
         */
        public Texture2D PinTexture
        {
            get { return _pinTexture; }
        }

        /**
         * <summary>    Gets the rocket texture. </summary>
         *
         * <value>  The rocket texture. </value>
         */
        public Texture2D RocketTexture
        {
            get { return _rocketTexture; }
        }

        /**
         * <summary>    Gets the rocket's smoke texture. </summary>
         *
         * <value>  The rocket's smoke texture. </value>
         */
        public Texture2D SmokeTexture
        {
            get { return _smokeTexture; }
        }

        /**
         * <summary>    Gets the carriage texture. </summary>
         *
         * <value>  The carriage texture. </value>
         */
        public Texture2D CarriageTexture
        {
            get { return _carriageTexture; }
        }

        /**
         * <summary>    Gets the cannon texture. </summary>
         *
         * <value>  The cannon texture. </value>
         */
        public Texture2D CannonTexture
        {
            get { return _cannonTexture; }
        }

        /**
         * <summary>    Gets the life bar texture. </summary>
         *
         * <value>  The life bar texture. </value>
         */
        public Texture2D LifeBarTexture
        {
            get { return _lifeBarTexture; }
        }

        /**
         * <summary>    Gets the green health bar texture. </summary>
         *
         * <value>  The green health bar texture. </value>
         */
        public Texture2D GreenBarTexture
        {
            get { return _greenBarTexture; }
        }

        /**
         * <summary>    Gets the yellow health bar texture. </summary>
         *
         * <value>  The yellow health bar texture. </value>
         */
        public Texture2D YellowBarTexture
        {
            get { return _yellowBarTexture; }
        }

        /**
         * <summary>    Gets the red health bar texture. </summary>
         *
         * <value>  The red health bar texture. </value>
         */
        public Texture2D RedBarTexture
        {
            get { return _redBarTexture; }
        }

        /**
         * <summary>    Gets the explosion particles texture. </summary>
         *
         * <value>  The explosion particles texture. </value>
         */
        public Texture2D ExplosionParticleTextures
        {
            get { return _explosionParticleTextures; }
        }

        #endregion
    }
}
