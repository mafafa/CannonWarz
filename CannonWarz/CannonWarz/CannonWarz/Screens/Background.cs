using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CannonWarz.Screens
{
    class Background : AbsScreen
    {
        public Background(Game1 game, SpriteBatch spriteBatch)
            : base(game)
        {
            SpriteBatch = spriteBatch;
        }

        public override void Activate()
        {

        }

        public override void Deactivate()
        {

        }

        public override void Update(GameTime gameTime)
        {
            // We don't need to update the background as it is a static image
        }

        public override void Draw(GameTime gameTime)
        {         
            Rectangle screenRectangle = new Rectangle(0, 0, Game.ScreenWidth, Game.ScreenHeight);
            SpriteBatch.Begin();
            SpriteBatch.Draw(Game.BackgroundTexture, screenRectangle, Color.White);
            SpriteBatch.End();
        }
    }
}
