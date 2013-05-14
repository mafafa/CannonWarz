using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz.Screens
{
    public abstract class AbsScreen
    {
        protected AbsScreen(Game1 game, SpriteBatch spriteBatch)
        {
            Initialized = false;
            Game = game;
            SpriteBatch = spriteBatch;
        }

        public abstract void Activate();

        public abstract void Deactivate();

        public virtual void Initialize()
        {
            Initialized = true;
        }

        public abstract void Update(GameTime gameTime);

        protected abstract void CheckKeyboardKeysState(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        #region ----------------------- PROPERTIES -----------------------

        public Game1 Game
        {
            get;
            protected set;
        }
        
        public bool Initialized
        {
            get;
            protected set;
        }

        public bool BlocksDraw
        {
            get;
            protected set;
        }

        protected SpriteBatch SpriteBatch
        {
            get;
            set;
        }

        #endregion
    }
}
