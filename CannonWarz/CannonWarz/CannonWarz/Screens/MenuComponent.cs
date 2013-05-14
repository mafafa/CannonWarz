using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CannonWarz.Screens
{
    class MenuComponent
    {
        public MenuComponent(AbsScreen owner, String buttonName, SpriteBatch spriteBatch, SpriteFont font)
        {
            ButtonName = buttonName;
            Font = font;
            SpriteBatch = spriteBatch;
            Owner = owner;
            buttonScale = 1.0f;

            // The position will be set manually after creation of the MenuComponent
            Position = new Vector2(0, 0);

            // Default color for the text
            UnselectedColor = Color.White;
            SelectedColor = Color.Yellow;

            // The first button in the list will be set to true manually after the creation of the MenuComponent
            Selected = false;
        }

        public void Update(GameTime gameTime)
        {
            double time = gameTime.TotalGameTime.TotalSeconds;

            // Pulsate the size of the selected menu component
            float pulsate = (float)Math.Sin(time * 6) + 1;
            buttonScale = 1 + pulsate * 0.05f;
        }

        public void Draw(GameTime gameTime)
        {            
            SpriteBatch.Begin();

            if (Selected)
            {
                SpriteBatch.DrawString(Font, ButtonName, Position, SelectedColor, 0, new Vector2(0, 0), buttonScale, SpriteEffects.None, 0);
            }

            else
            {
                SpriteBatch.DrawString(Font, ButtonName, Position, UnselectedColor, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
            }

            SpriteBatch.End();
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private float buttonScale;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public AbsScreen Owner
        {
            get;
            private set;
        }
        
        public String ButtonName
        {
            get;
            private set;
        }

        public bool Selected
        {
            get;
            set;
        }

        public Color UnselectedColor
        {
            get;
            set;
        }

        public Color SelectedColor
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }

        public int TextHeight
        {
            get
            {
                return Owner.Game.MenuFont.LineSpacing;
            }
        }

        public int TextWidth
        {
            get
            {
                return (int)Owner.Game.MenuFont.MeasureString(ButtonName).X;
            }
        }

        public SpriteFont Font
        {
            get;
            private set;
        }

        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }

        #endregion
    }
}
