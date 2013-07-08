/**
// file:	Screens\MainMenu.cs
//
// summary:	Implements the main menu class
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz.Screens
{
    /**
     * <summary>    Main menu class. </summary>
     */
    public class MainMenu : AbsScreen
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="game">          The game instance. </param>
         * <param name="spriteBatch">   The sprite batch used to draw. </param>
         */
        public MainMenu(Game1 game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            _oldState = Keyboard.GetState();

            MenuComponents = new MenuButton[3];

            MenuButton newSingleGameButton = new MenuButton(this, "Single Player Game", SpriteBatch, Game.MenuFont, true);
            MenuButton newLocalMultiGameButton = new MenuButton(this, "Local Multi Player Game", SpriteBatch, Game.MenuFont, true);
            MenuButton exitButton = new MenuButton(this, "Exit", SpriteBatch, Game.MenuFont, true);
            MenuComponents[0] = newSingleGameButton;
            MenuComponents[1] = newLocalMultiGameButton;
            MenuComponents[2] = exitButton;

            // We set the position of the buttons
            Vector2 position = new Vector2(0f, Game.GraphicsDevice.Viewport.Height / 3);    // The starting height of the components is the third of the screen
            foreach (MenuButton component in MenuComponents)
            {
                position.X = (Game.GraphicsDevice.Viewport.Width / 2);
                component.Position = position;
                position.Y += component.TextHeight;
            }

            // The first button is selected by default
            newSingleGameButton.Selected = true;

            _activeComponentIndex = 0;
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
         * <summary>    Updates the screen's logic. Calls update on all of the menu components. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public override void Update(GameTime gameTime)
        {
            foreach (MenuButton component in MenuComponents)
            {
                component.Update(gameTime);
            }

            CheckKeyboardKeysState(gameTime);
        }

        /**
         * <summary>    Check keyboard keys state. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        protected override void CheckKeyboardKeysState(GameTime gameTime)
        {
            // We use the new keyboard state in order to block continuous scrolling when the user keeps on pressing a key as
            // it can cause problems in the menu
            KeyboardState newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !_oldState.IsKeyDown(Keys.Down))
            {
                MenuComponents[_activeComponentIndex].Selected = false;

                _activeComponentIndex++;

                if (_activeComponentIndex >= MenuComponents.Length)
                {
                    _activeComponentIndex = 0;
                }

                MenuComponents[_activeComponentIndex].Selected = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_oldState.IsKeyDown(Keys.Up))
            {
                MenuComponents[_activeComponentIndex].Selected = false;

                _activeComponentIndex--;

                if (_activeComponentIndex < 0)
                {
                    _activeComponentIndex = MenuComponents.Length - 1;
                }

                MenuComponents[_activeComponentIndex].Selected = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_oldState.IsKeyDown(Keys.Enter))
            {
                switch (MenuComponents[_activeComponentIndex].ButtonName)
                {
                    case "Single Player Game":
                        GameScreenManager gameScreenManager = GameScreenManager.Instance;

                        // We push a new background screen with its BreaksDraw set to true
                        gameScreenManager.Push(new Background(Game, SpriteBatch));
                        Background backgroundScreen = (Background)gameScreenManager.ActiveScreen;
                        backgroundScreen.BreaksDraw = true;

                        gameScreenManager.Push(new SinglePlayerGameScreen(Game, SpriteBatch));
                        break;

                    case "Local Multi Player Game":
                        gameScreenManager = GameScreenManager.Instance;

                        // We push a new background screen with its BreaksDraw set to true
                        gameScreenManager.Push(new Background(Game, SpriteBatch));
                        backgroundScreen = (Background)gameScreenManager.ActiveScreen;
                        backgroundScreen.BreaksDraw = true;
                        
                        gameScreenManager.Push(new LocalMultiGameScreen(Game, SpriteBatch));
                        break;

                    case "Exit":
                        Game.Exit();
                        break;
                }
            }

            _oldState = newState;
        }

        /**
         * <summary>    Draws the screen components on the screen. Calls draw on each components. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public override void Draw(GameTime gameTime)
        {
            // We set the position, color and scale for the pause title
            String menuTitle = "Main Menu";
            Vector2 titlePosition = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 6);
            Vector2 titleOrigin = Game.MenuFont.MeasureString(menuTitle) / 2;
            Color titleColor = Color.Black * 0.6f; // The * 0.6f makes the title transparent at 60%
            float titleScale = 1.50f;

            SpriteBatch.Begin();

            // We draw the pause title
            SpriteBatch.DrawString(Game.MenuFont, menuTitle, titlePosition, titleColor, 0, titleOrigin, titleScale, SpriteEffects.None, 0);

            SpriteBatch.End();

            foreach (MenuButton component in MenuComponents)
            {
                component.Draw(gameTime);
            }
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private int _activeComponentIndex;
        private KeyboardState _oldState;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the array of menu components. </summary>
         *
         * <value>  The array of menu components. </value>
         */
        public MenuButton[] MenuComponents
        {
            get;
            private set;
        }

        #endregion
    }
}
