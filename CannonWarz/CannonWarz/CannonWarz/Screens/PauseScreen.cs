using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz.Screens
{
    class PauseScreen : AbsScreen
    {
        public PauseScreen(Game1 game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            oldState = Keyboard.GetState();
            
            MenuComponents = new MenuComponent[2];
           
            MenuComponent resumeButton = new MenuComponent(this, "Resume", SpriteBatch, Game.MenuFont);
            MenuComponent exitButton = new MenuComponent(this, "Exit", SpriteBatch, Game.MenuFont);
            MenuComponents[0] = resumeButton;
            MenuComponents[1] = exitButton;

            // We set the position of the buttons
            Vector2 position = new Vector2(0f, Game.GraphicsDevice.Viewport.Height / 3);    // The starting height of the components is the third of the screen
            foreach (MenuComponent component in MenuComponents)
            {
                position.X = (Game.GraphicsDevice.Viewport.Width / 2) - (component.TextWidth / 2);
                component.Position = position;
                position.Y += component.TextHeight;
            }          

            // The first button is selected by default
            resumeButton.Selected = true;

            activeComponentIndex = 0;
        }

        public override void Activate()
        {

        }

        public override void Deactivate()
        {

        }

        public override void Update(GameTime gameTime)
        {          
            foreach (MenuComponent component in MenuComponents)
            {
                component.Update(gameTime);
            }

            CheckKeyboardKeysState(gameTime);
        }

        protected override void CheckKeyboardKeysState(GameTime gameTime)
        {
            // We use the new keyboard state in order to block continuous scrolling when the user keeps on pressing a key as
            // it can cause problems in the menu
            KeyboardState newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down))
            {
                MenuComponents[activeComponentIndex].Selected = false;

                activeComponentIndex--;

                if (activeComponentIndex < 0)
                {
                    activeComponentIndex = MenuComponents.Length - 1;
                }

                MenuComponents[activeComponentIndex].Selected = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up))
            {
                MenuComponents[activeComponentIndex].Selected = false;

                activeComponentIndex++;

                if (activeComponentIndex >= MenuComponents.Length)
                {
                    activeComponentIndex = 0;
                }

                MenuComponents[activeComponentIndex].Selected = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                switch (MenuComponents[activeComponentIndex].ButtonName)
                {
                    case "Resume":
                        GameScreenManager gameScreenManager = GameScreenManager.Instance;
                        gameScreenManager.Pop();
                        break;

                    case "Exit":
                        Game.Exit();
                        break;
                }
            }

            oldState = newState;
        }

        public override void Draw(GameTime gameTime)
        {
            // We want to draw the opaque texture all over the screen
            Rectangle opaqueRectangle = new Rectangle(0, 0, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);

            // We set the position, color and scale for the pause title
            String pauseTitle = "Pause";
            Vector2 titlePosition = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 6);
            Vector2 titleOrigin = Game.MenuFont.MeasureString(pauseTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * 0.6f; // The * 0.6f makes the title transparent at 60%
            float titleScale = 1.30f;

            SpriteBatch.Begin();

            // We draw the opaque overlay
            SpriteBatch.Draw(Game.OpaqueTexture, opaqueRectangle, Color.White);

            // We draw the pause title
            SpriteBatch.DrawString(Game.MenuFont, pauseTitle, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

            SpriteBatch.End();
            
            foreach (MenuComponent component in MenuComponents)
            {
                component.Draw(gameTime);
            }
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private int activeComponentIndex;
        private KeyboardState oldState;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public MenuComponent[] MenuComponents
        {
            get;
            private set;
        }

        #endregion
    }
}
