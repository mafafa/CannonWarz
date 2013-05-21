/**
// file:	Screens\MenuButton.cs
//
// summary:	Implements the menu button class
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz.Screens
{
    /**
     * <summary>    Menu button class. A menu button is the basic button found in any menu type screen. </summary>
     */
    public class MenuButton
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="owner">         The screen that owns the component. </param>
         * <param name="buttonName">    Name of the button. This is the string that will appear on screen. </param>
         * <param name="spriteBatch">   The sprite batch used to draw the component. </param>
         * <param name="font">          The font used to draw the string. </param>
         * <param name="popIn">         true to enable the pop in effect, false otherwise. </param>
         */
        public MenuButton(AbsScreen owner, String buttonName, SpriteBatch spriteBatch, SpriteFont font, bool popIn)
        {
            ButtonName = buttonName;
            Font = font;
            SpriteBatch = spriteBatch;
            Owner = owner;

            if (popIn)
            {
                _unselectedButtonScale = 0;
                _selectedButtonScale = 0;
            }

            else
            {
                _unselectedButtonScale = 1.0f;
                _selectedButtonScale = 1.0f;
            }

            // The position will be set manually after creation of the MenuComponent
            Position = new Vector2(0, 0);

            // Default color for the text
            UnselectedColor = Color.White;
            SelectedColor = Color.Yellow;

            // The first button in the list will be set to true manually after the creation of the MenuComponent
            Selected = false;
        }

        /**
         * <summary>    Updates the menu button. Pop in and pulsate logic is found here. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public void Update(GameTime gameTime)
        {
            // "Pop in" effect
            if (Appearing)
            {
                _unselectedButtonScale += 0.05f;
                _selectedButtonScale += 0.05f;
            }

            else
            {
                double time = gameTime.TotalGameTime.TotalSeconds;

                // Pulsate the size of the selected menu button
                float pulsate = (float)Math.Sin(time * 6) + 1;
                _selectedButtonScale = 1 + pulsate * 0.05f;
            }
        }

        /**
         * <summary>    Draws the menu button. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public void Draw(GameTime gameTime)
        {            
            SpriteBatch.Begin();

            if (Selected)
            {
                SpriteBatch.DrawString(Font, ButtonName, Position, SelectedColor, 0, new Vector2(TextWidth / 2, TextHeight / 2), _selectedButtonScale, SpriteEffects.None, 0);
            }

            else
            {
                SpriteBatch.DrawString(Font, ButtonName, Position, UnselectedColor, 0, new Vector2(TextWidth / 2, TextHeight / 2), _unselectedButtonScale, SpriteEffects.None, 0);
            }

            SpriteBatch.End();
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private float _unselectedButtonScale;
        private float _selectedButtonScale;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the owner of the menu button. </summary>
         *
         * <value>  The owner of the menu button. </value>
         */
        public AbsScreen Owner
        {
            get;
            private set;
        }

        /**
         * <summary>    
         *  Gets or sets the name of the button. This is the string that will appear 
         *  on screen.              
         * </summary>
         *
         * <value>  The name of the button. </value>
         */
        public String ButtonName
        {
            get;
            private set;
        }

        /**
         * <summary>    
         *  Gets or sets a value indicating whether the button is currently 
         *  selected. 
         * </summary>
         *
         * <value>  true if selected, false if not. </value>
         */
        public bool Selected
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the color of the button if it is unselected. </summary>
         *
         * <value>  The color of the button if it is unselected. </value>
         */
        public Color UnselectedColor
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the color of the button if it is selected. </summary>
         *
         * <value>  The color of the button if it is selected. </value>
         */
        public Color SelectedColor
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the position of the button. </summary>
         *
         * <value>  The position of the button. </value>
         */
        public Vector2 Position
        {
            get;
            set;
        }

        /**
         * <summary>    
         *  Gets a value indicating whether the component is currently appearing. The button is appearing 
         *  if the pop in effect is not finished.              
         * </summary>
         *
         * <value>  true if appearing, false if not. </value>
         */
        public bool Appearing
        {
            get
            {
                // Once the button scale is at 1.0f, it is considered appeared on screen
                if (_unselectedButtonScale > 1.0f)
                {
                    return false;
                }
                
                else
                {
                    return true;
                }
            }
        }

        /**
         * <summary>    Gets the height of the text. </summary>
         *
         * <value>  The height of the text. </value>
         */
        public int TextHeight
        {
            get
            {
                return Owner.Game.MenuFont.LineSpacing;
            }
        }

        /**
         * <summary>    Gets the width of the text. </summary>
         *
         * <value>  The width of the text. </value>
         */
        public int TextWidth
        {
            get
            {
                return (int)Owner.Game.MenuFont.MeasureString(ButtonName).X;
            }
        }

        /**
         * <summary>    Gets or sets the font used to draw the component. </summary>
         *
         * <value>  The font used to draw the component. </value>
         */
        public SpriteFont Font
        {
            get;
            private set;
        }

        /**
         * <summary>    Gets or sets the sprite batch used to draw the button. </summary>
         *
         * <value>  The sprite batch used to draw the button. </value>
         */
        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        #endregion
    }
}
