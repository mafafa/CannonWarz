using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz.Screens
{
    public class SinglePlayerGameScreen : AbsGameScreen
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="game">          The game instance. </param>
         * <param name="spriteBatch">   The sprite batch used to draw. </param>
         */
        public SinglePlayerGameScreen(Game1 game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
            isAITurn = false;
        }

        /**
         * <summary>    Initializes the screen by creating the terrain and the players. </summary>
         */
        public override void Initialize()
        {
            
            
            base.Initialize();
        }

        /**
         * <summary>    Updates the screen's logic. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #region -------------------- UPDATE FUNCTIONS --------------------

        /**
         * <summary>    Check keyboard keys state. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        protected override void CheckKeyboardKeysState(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameScreenManager gameScreenManager = GameScreenManager.Instance;
                gameScreenManager.Push(new PauseScreen(Game, SpriteBatch));
            }

            // The player can only play when it is his turn
            else if (!isAITurn)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    PlayersArray[CurrentPlayer].Angle -= 0.01f;
                    VerifyAngleRange();
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    PlayersArray[CurrentPlayer].Angle += 0.01f;
                    VerifyAngleRange();
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    PlayersArray[CurrentPlayer].Power += 1;
                    VerifyPowerRange();
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    PlayersArray[CurrentPlayer].Power -= 1;
                    VerifyPowerRange();
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                {
                    PlayersArray[CurrentPlayer].Power += 20;
                    VerifyPowerRange();
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                {
                    PlayersArray[CurrentPlayer].Power -= 20;
                    VerifyPowerRange();
                }

                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (RocketIsFlying == false)
                    {
                        RocketIsFlying = true;

                        Rocket rocket = new Rocket(Game.RocketTexture, Game.SmokeTexture, PlayersArray[CurrentPlayer].Position, PlayersArray[CurrentPlayer].Angle, PlayersArray[CurrentPlayer].Power);
                        _instantiatedRocketList.Add(rocket);
                    }
                }
            }
        }

        #endregion

        protected override void CreatePlayers(Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
        {
            Color[] playerColors = new Color[10];
            playerColors[0] = Color.Red;
            playerColors[1] = Color.Green;
            playerColors[2] = Color.Blue;
            playerColors[3] = Color.Purple;
            playerColors[4] = Color.Orange;
            playerColors[5] = Color.Indigo;
            playerColors[6] = Color.Yellow;
            playerColors[7] = Color.SaddleBrown;
            playerColors[8] = Color.Tomato;
            playerColors[9] = Color.Turquoise;

            PlayersArray = new AbsPlayer[NumberOfPlayers];
            
            // We create and place the only human player first
            PlayersArray[0] = new HumanPlayer(playerColors[0], carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture);
            PlayersArray[0].PlacePlayer(NumberOfPlayers, 0, Game.ScreenWidth, _terrain);

            // Then we create and place the AI players
            for (int i = 1; i < NumberOfPlayers; i++)
            {
                // We create the player
                PlayersArray[i] = new AIPlayer(playerColors[i], carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture);

                // We set his position
                PlayersArray[i].PlacePlayer(NumberOfPlayers, i, Game.ScreenWidth, _terrain);
            }

            // We flatten the ground under their positions
            _terrain.FlattenTerrainBelowPlayers(PlayersArray);
        }

        protected override void NextPlayer()
        {
            // We increment the current player
            CurrentPlayer++;

            // To be sure that we do not go pass the NumberOfPlayers
            CurrentPlayer = CurrentPlayer % NumberOfPlayers;

            // We do the two last manipulations until we find a player that is alive
            while (!PlayersArray[CurrentPlayer].IsAlive)
            {
                CurrentPlayer = ++CurrentPlayer % NumberOfPlayers;
            }

            // We change the wind direction for the next player
            _terrain.GenerateWindDirection();

            // If it is the AI's turn, we call the PlayTurn() method
            if (PlayersArray[CurrentPlayer].GetType() == typeof(AIPlayer))
            {
                isAITurn = true;
                
                AIPlayer aiPlayer = (AIPlayer)PlayersArray[CurrentPlayer];
                aiPlayer.PlayTurn(PlayersArray, _terrain.WindDirection, this, Game);

                // We create the rocket
                RocketIsFlying = true;
                Rocket rocket = new Rocket(Game.RocketTexture, Game.SmokeTexture, PlayersArray[CurrentPlayer].Position, PlayersArray[CurrentPlayer].Angle, PlayersArray[CurrentPlayer].Power);
                _instantiatedRocketList.Add(rocket);
            }

            else
            {
                isAITurn = false;
            }
        }

        #region --------------------- PRIVATE FIELDS ---------------------



        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public bool isAITurn
        {
            get;
            private set;
        }

        #endregion
    }
}
