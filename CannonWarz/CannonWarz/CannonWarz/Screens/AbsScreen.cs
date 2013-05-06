using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz.Screens
{
    public abstract class AbsScreen
    {
        protected AbsScreen(Game1 game)
            //: base(game)
        {
            Initialized = false;
            Game = game;
        }

        public abstract void Activate();

        public abstract void Deactivate();

        public virtual void Initialize()
        {
            //base.Initialize();
            Initialized = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
        }

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
    }
}
