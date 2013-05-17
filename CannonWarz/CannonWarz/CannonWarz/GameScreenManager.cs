/**
// file:	GameScreenManager.cs
//
// summary:	Implements the game screen manager class
 */

using System;
using System.Collections.Generic;
using CannonWarz.Screens;
using Microsoft.Xna.Framework;

namespace CannonWarz
{
    /**
     * <summary>    Singleton that manages the various game screens and their transitions. </summary>
     */
    public class GameScreenManager
    {
        /**
         * <summary>    Private constructor. </summary>
         *
         * <param name="game">  The game instance. </param>
         */
        private GameScreenManager(Game1 game)
        {
            ScreenStack = new Stack<AbsScreen>();
            Game = game;
        }

        /**
         * <summary>    Creates the screen manager. </summary>
         *
         * <exception cref="Exception"> Thrown when the game screen manager has already been created. </exception>
         *
         * <param name="game">  The game instance. </param>
         *
         * <returns>    The instance of the created screen manager. </returns>
         */
        public static GameScreenManager Create(Game1 game)
        {
            if (_uniqueInstance != null)
            {
                throw new Exception("GameScreenManager has already been created");
            }

            _uniqueInstance = new GameScreenManager(game);

            return _uniqueInstance;
        }

        /**
         * <summary>    
         *  Pushes an object onto the screen stack. Also calls the Deactivate() method
         *  of the screen that was previously on the top of the stack. 
         * </summary>
         *
         * <param name="screen">    The screen to push. </param>
         */
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

        /**
         * <summary>    Returns the top-of-stack screen without removing it. </summary>
         *
         * <returns>    The current top-of-stack screen. </returns>
         */
        public AbsScreen Peek()
        {
            if (ScreenStack.Count < 1)
            {
                return null;
            }

            return ScreenStack.Peek();
        }

        /**
         * <summary>    
         *  Removes and returns the top-of-stack screen. Also calls the Activate() method of 
         *  the screen that is now on the top of the stack.
         * </summary>
         *
         * <returns>    The previous top-of-stack screen. </returns>
         */
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

        /**
         * <summary>   
         *  Calls Initialize() on all of the screen that are currently in the stack. 
         *  Sets the Initialized property of the screen manager to true once it is done.
         * </summary>
         */
        public void Initialize()
        {
            foreach (AbsScreen screen in ScreenStack)
            {
                screen.Initialize();
            }
            
            this.Initialized = true;
        }

        /**
         * <summary>    Updates top-of-stack screen. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public void Update(GameTime gameTime)
        {
            // We always only update the top screen on the stack
            ScreenStack.Peek().Update(gameTime);
        }

        /**
         * <summary>    
         *  Draws the all screen in the stack until a screen with the BlocksDraw property set to true 
         *  is found in the stack. No screen under that screen in the stack will be drawn.
         * </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public void Draw(GameTime gameTime)
        {
            List<AbsScreen> screensToDraw = new List<AbsScreen>();

            // We iterate through all the screens and stop on the first one that has its
            // BlocksDraw attribute set to true
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
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        private static GameScreenManager _uniqueInstance;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets the single instance of the game screen manager. </summary>
         * 
         * <exception cref="Exception"> Thrown when the game screen manager has not been created yet. </exception>
         *
         * <value>  The single instance of the game screen manager. </value>
         */
        public static GameScreenManager Instance
        {
            get
            {
                if (_uniqueInstance == null)
                {
                    throw new Exception("GameScreenManager has not been created yet");
                }

                return _uniqueInstance;
            }
        }

        /**
         * <summary>    
         *  Gets or sets a value indicating whether the game screen manager is initialized. 
         *  The game screen manager is considered initialized when all of its current screens
         *  in the stack are initialized.
         * </summary>
         *
         * <value>  true if initialized, false if not. </value>
         */
        public bool Initialized
        {
            get;
            private set;
        }

        /**
         * <summary>    Gets the active screen in the stack. </summary>
         *
         * <value>  The active screen. </value>
         */
        public AbsScreen ActiveScreen
        {
            get { return this.Peek(); }
        }

        /**
         * <summary>    Gets or sets the stack of game screens. </summary>
         *
         * <value>  The stack of game screens. </value>
         */
        public Stack<AbsScreen> ScreenStack
        {
            get;
            private set;
        }

        /**
         * <summary>    Gets or sets the game instance. </summary>
         *
         * <value>  The game instance. </value>
         */
        public Game1 Game
        {
            get;
            private set;
        }

        #endregion
    }
}