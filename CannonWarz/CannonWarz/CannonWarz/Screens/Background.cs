/**
// file:	Screens\Background.cs
//
// summary:	Implements the background class
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz.Screens
{
    /**
     * <summary>    Background screen class. </summary>
     */
    public class Background : AbsScreen
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="game">          The game instance. </param>
         * <param name="spriteBatch">   The sprite batch used to draw. </param>
         */
        public Background(Game1 game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
        }

        /**
         * <summary>    Called when the screen is back on top of the game screen manager's stack. </summary>
         */
        public override void Activate()
        {
        }

        /**
         * <summary>    Called when the screen is no longer on top of the game screen manager's stack. </summary>
         */
        public override void Deactivate()
        {
        }

        /**
         * <summary>    Updates the screen's logic. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public override void Update(GameTime gameTime)
        {
            // We don't need to update the background as it is a static image
        }

        /**
         * <summary>    Check keyboard keys state. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        protected override void CheckKeyboardKeysState(GameTime gameTime)
        {
            // We don't need to check for keyboard state as it is the background image
        }

        /**
         * <summary>    Draws the screen components on the screen. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public override void Draw(GameTime gameTime)
        {         
            Rectangle screenRectangle = new Rectangle(0, 0, Game.ScreenWidth, Game.ScreenHeight);
            SpriteBatch.Begin();
            SpriteBatch.Draw(Game.BackgroundTexture, screenRectangle, Color.White);
            SpriteBatch.End();
        }
    }
}
