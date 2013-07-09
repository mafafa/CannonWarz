/**
// file:	AIPlayer.cs
//
// summary:	Implements the AI player class
 */

using System;
using CannonWarz.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    /**
     * <summary>    AI player class. </summary>
     */
    public class AIPlayer : AbsPlayer
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="color">                 The color of the AI player. </param>
         * <param name="randomTargetingFactor"> The random targeting factor for the random generator that selects which player to target next. </param>
         * <param name="carriageTexture">       The carriage texture. </param>
         * <param name="cannonTexture">         The cannon texture. </param>
         * <param name="lifeBarTexture">        The life bar texture. </param>
         * <param name="greenBarTexture">       The green bar texture. </param>
         * <param name="yellowBarTexture">      The yellow bar texture. </param>
         * <param name="redBarTexture">         The red bar texture. </param>
         */
        public AIPlayer(Color color, int randomTargetingFactor, Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
            : base(color, carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture)
        {
            RandomTargetingFactor = randomTargetingFactor;
        }

        /**
         * <summary>    Plays the AI's turn. </summary>
         *
         * <param name="playersArray">  Array containing all of the players. </param>
         * <param name="windDirection"> The wind direction. </param>
         * <param name="gameScreen">    The game screen. </param>
         * <param name="game">          The game instance. </param>
         */
        public void PlayTurn(AbsPlayer[] playersArray, Vector2 windDirection, AbsGameScreen gameScreen, Game1 game)
        {
            // We select a random player
            Random randomTargetFactor = new Random(RandomTargetingFactor);
            int targetPlayerIndex;
            do
            {
                targetPlayerIndex = randomTargetFactor.Next(0, 4);
            }
            while ((targetPlayerIndex == gameScreen.CurrentPlayer) || (playersArray[targetPlayerIndex].IsAlive == false));

            float finalXPos = playersArray[targetPlayerIndex].Position.X;
            float finalYPos = playersArray[targetPlayerIndex].Position.Y;
            float iniXPos = playersArray[gameScreen.CurrentPlayer].Position.X + 30; // The +30 is because the iniXPos of the rocket is 30 pixels right of the cannon
            float iniYPos = playersArray[gameScreen.CurrentPlayer].Position.Y - 15; // The -15 is because the iniYPos of the rocket is 15 pixels up of the cannon
            int t = 5;  // This is a random value in order to remove the t variable from Euler's equations. With this value, the AI player will
                        // miss about 40-50% of its shots which is reasonable

            float iniXSpeed = (finalXPos - iniXPos - (float)0.5 * windDirection.X * (float)Math.Pow(t, 2)) / t;
            float iniYSpeed = (finalYPos - iniYPos - (float)0.5 * Terrain.g * (float)Math.Pow(t, 2)) / t;

            Angle = (int)Math.Round(MathHelper.ToDegrees((float)Math.Atan2(iniXSpeed, -iniYSpeed)));
            Power = (float)Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2));

            // We verify that the Angle and Power values and in the good range. If they are not, since we correct them, the AI will likely miss and 
            // it kind of simulates a missed shot for the AI. This should not happen though, but is here in case it does.
            /*if (Angle < -90)
            {
                Angle = -90;
            }

            else if (Angle > 90)
            {
                Angle = 90;
            }

            if (Power < 0)
            {
                Power = 0;
            }

            else if (Power > 1000)
            {
                Power = 1000;
            }*/
        }

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    
         *  Gets or sets the random targeting factor that is passed to the random generator that is
         *  used to select which player to target next. 
         * </summary>
         *
         * <value>  The random targeting factor. </value>
         */
        public int RandomTargetingFactor
        {
            get;
            private set;
        }

        #endregion
    }
}
