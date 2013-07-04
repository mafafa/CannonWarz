using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CannonWarz.Screens;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz
{
    public class AIPlayer : AbsPlayer
    {
        public AIPlayer(Color color, Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
            : base(color, carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture)
        {
            Random randomShotFactor = new Random(1);
            LastShotUncertaintyValue = randomShotFactor.Next(-25, 25);
            
            Random randomAngleFactor = new Random(2);
            LastAngleUncertaintyValue = randomAngleFactor.Next((int)(-Math.PI / 8), (int)(Math.PI / 8));
        }

        public void PlayTurn(AbsPlayer[] playersArray, Vector2 windDirection, AbsGameScreen gameScreen, Game1 game)
        {
            // We select a random player
            Random randomTargetFactor = new Random(3);
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
            float iniYSpeed = -600; // Random value. With this value, we should be able to find an iniXSpeed value to be able to hit anyone on the map
            float iniXSpeed = 0;

            // Found with Euler's equations of a parabolic trajectory
            iniXSpeed = (float)((Terrain.g * (finalXPos - iniXPos)) / (-iniYSpeed + Math.Sqrt(Math.Pow(iniYSpeed, 2) - 2 * Terrain.g * (finalYPos - iniYPos))));

            Angle = (int)MathHelper.ToDegrees((float)Math.Atan2(iniXSpeed, -iniYSpeed));
            Power = (float)Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2));

            // We verify that the Angle and Power values and in the good range. If they are not, since we correcte them, the AI will likely miss and 
            // it kind of simulates a missed shot for the AI. This should not happen though, but is here in case it does.
            if (Angle < -90)
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
            }
        }

        public int LastShotUncertaintyValue
        {
            get;
            private set;
        }

        public int LastAngleUncertaintyValue
        {
            get;
            private set;
        }
    }
}
