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
            while (targetPlayerIndex == gameScreen.CurrentPlayer);

            // We determine the shot power value
            float finalXPos = playersArray[targetPlayerIndex].Position.X;
            float finalYPos = playersArray[targetPlayerIndex].Position.Y;

            for (float iniXSpeed = (game.ScreenWidth / gameScreen.NumberOfPlayers); iniXSpeed <= 1000; iniXSpeed++)
            {
                float t = (finalXPos - Position.X) / (iniXSpeed + windDirection.X);
                float iniYSpeed = ((finalYPos - Position.Y) / t) - (400 * t * (float)0.5);
                Power = (float)Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2));
                Angle = (float)Math.Atan2(iniYSpeed, iniXSpeed);

                if ((Angle >= (-Math.PI / 2) && Angle <= (Math.PI / 2)) && 
                    iniYSpeed <= 1000 &&
                    Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2)) <= 1000)
                {
                    break;
                }
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
