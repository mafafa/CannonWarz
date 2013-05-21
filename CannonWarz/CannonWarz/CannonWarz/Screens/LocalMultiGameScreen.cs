/**
// file:	Screens\LocalMultiGameScreen.cs
//
// summary:	Implements the local multiplayer game screen class
 */

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz.Screens
{
    /**
     * <summary>    Local multiplayer game screen. </summary>
     */
    public class LocalMultiGameScreen : AbsGameScreen
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="game">          The game instance. </param>
         * <param name="spriteBatch">   The sprite batch used to draw. </param>
         */
        public LocalMultiGameScreen(Game1 game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {
        }

        /**
         * <summary>
         *  Creates the players, calls PlacePlayer() and flattens the terrain under the players.
         * </summary>
         *
         * <param name="carriageTexture">   The carriage texture. </param>
         * <param name="cannonTexture">     The cannon texture. </param>
         * <param name="lifeBarTexture">    The life bar texture. </param>
         * <param name="greenBarTexture">   The green bar texture. </param>
         * <param name="yellowBarTexture">  The yellow bar texture. </param>
         * <param name="redBarTexture">     The red bar texture. </param>
         */
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

            PlayersArray = new HumanPlayer[NumberOfPlayers];
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                // We create the player
                PlayersArray[i] = new HumanPlayer(playerColors[i], carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture);

                // We set his position
                PlayersArray[i].PlacePlayer(NumberOfPlayers, i, Game.ScreenWidth, _terrain);
            }

            // We flatten the ground under their positions
            _terrain.FlattenTerrainBelowPlayers(PlayersArray);
        }

        /**
         * <summary>    Switches to the next player's turn and calls GenerateWindDirection(). </summary>
         */
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
        }
    }
}
