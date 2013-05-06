using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CannonWarz.Screens;

namespace CannonWarz
{
    public class GameScreenManager
    {
        private GameScreenManager(Game1 game)
            //:base(game)
        {
            ScreenStack = new Stack<AbsScreen>();
            Game = game;
        }

        public static GameScreenManager Create(Game1 game)
        {
            if (uniqueInstance != null)
            {
                throw new Exception("GameScreenManager has already been created");
            }

            uniqueInstance = new GameScreenManager(game);

            return uniqueInstance;
        }

        public void Push(AbsScreen screen)
        {
            // If all the screens in the stack were already initialized, we initialize the
            // one we want to push first before pushing it
            if (!screen.Initialized && this.Initialized)
            {
                screen.Initialize();
            }

            // We deactivate the current active screen
            if (ActiveScreen != null)
            {
                ActiveScreen.Deactivate();
            }

            ScreenStack.Push(screen);
        }

        public AbsScreen Peek()
        {
            if (ScreenStack.Count < 1)
            {
                return null;
            }

            return ScreenStack.Peek();
        }

        public AbsScreen Pop()
        {
            if (ScreenStack.Count < 1)
            {
                return null;
            }

            AbsScreen previous = ScreenStack.Pop();

            // We activate the new active screen
            if (ActiveScreen != null)
            {
                ActiveScreen.Activate();
            }

            return previous;
        }

        public void Initialize()
        {
            foreach (AbsScreen screen in ScreenStack)
            {
                screen.Initialize();
            }
            
            this.Initialized = true;

            //base.Initialize();
        }
        
        public void Update(GameTime gameTime)
        {
            // We always only update the top screen on the stack
            ScreenStack.Peek().Update(gameTime);

            //base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            List<AbsScreen> screensToDraw = new List<AbsScreen>();

            // We iterate through all the screens and stop on the first one that has its
            // Visible attribute set to false
            foreach (AbsScreen screen in ScreenStack)
            {
                screensToDraw.Add(screen);

                if (screen.BlocksDraw)
                {
                    break;
                }
            }

            // We draw the screens backward (last screen first)
            for (int i = screensToDraw.Count - 1; i >= 0; i--)
            {
                screensToDraw[i].Draw(gameTime);
            }

            //base.Draw(gameTime);
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private static GameScreenManager uniqueInstance;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public static GameScreenManager Instance
        {
            get
            {
                if (uniqueInstance == null)
                {
                    throw new Exception("GameScreenManager has not been created yet");
                }

                return uniqueInstance;
            }
        }

        public bool Initialized
        {
            get;
            private set;
        }

        public AbsScreen ActiveScreen
        {
            get { return this.Peek(); }
        }

        public Stack<AbsScreen> ScreenStack
        {
            get;
            private set;
        }

        public Game1 Game
        {
            get;
            private set;
        }

        #endregion
    }
}