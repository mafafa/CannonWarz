/**
// file:	Screens\AbsScreen.cs
//
// summary:	Implements the abstract screen class
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz.Screens
{
    /**
     * <summary>    Abstract screen class. All screens derive from this class. </summary>
     */
    public abstract class AbsScreen
    {
        /**
         * <summary>    Specialised constructor for use only by derived classes. </summary>
         *
         * <param name="game">          The game instance. </param>
         * <param name="spriteBatch">   The sprite batch used to draw on the screen. </param>
         */
        protected AbsScreen(Game1 game, SpriteBatch spriteBatch)
        {
            Initialized = false;
            Game = game;
            SpriteBatch = spriteBatch;
            BreaksDraw = false;
        }

        /**
         * <summary>    
         *  Called when the screen is back on top of the game screen manager's stack. 
         *  Must be implemented by child class if needed.
         * </summary>
         */
        public abstract void Activate();

        /**
         * <summary>    
         *  Called when the screen is no longer on top of the game screen manager's stack. 
         *  Must be implemented by child class if needed.
         * </summary>
         */
        public abstract void Deactivate();

        /**
         * <summary>    
         *  Initializes the screen. Must be overridden by child class if an initizialization
         *  is needed.              
         * </summary>
         */
        public virtual void Initialize()
        {
            Initialized = true;
        }

        /**
         * <summary>    Updates the screen's logic. Must be overridden by child class. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public abstract void Update(GameTime gameTime);

        /**
         * <summary>    Check keyboard keys state. Must be overridden by child class. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        protected abstract void CheckKeyboardKeysState(GameTime gameTime);

        /**
         * <summary>    
         *  Draws the screen components on the screen. Must be overridden by child
         *  class.
         * </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public abstract void Draw(GameTime gameTime);

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the game instance. </summary>
         *
         * <value>  The game instance. </value>
         */
        public Game1 Game
        {
            get;
            protected set;
        }

        /**
         * <summary>    Gets or sets a value indicating whether the screen is initialized. </summary>
         *
         * <value>  true if initialized, false if not. </value>
         */
        public bool Initialized
        {
            get;
            protected set;
        }

        /**
         * <summary>    Gets or sets a value indicating whether the screens under this screen should be drawn or not. </summary>
         *
         * <value>  false if they should be drawn, true if not. </value>
         */
        public bool BreaksDraw
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the sprite batch used to draw on the screen. </summary>
         *
         * <value>  The sprite batch used to draw on the screen. </value>
         */
        protected SpriteBatch SpriteBatch
        {
            get;
            set;
        }

        #endregion
    }
}
