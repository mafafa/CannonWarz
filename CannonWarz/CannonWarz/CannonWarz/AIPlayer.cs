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
            float iniXPos = playersArray[gameScreen.CurrentPlayer].Position.X;
            float iniYPos = playersArray[gameScreen.CurrentPlayer].Position.Y;
            //float iniXSpeed = (game.ScreenWidth / gameScreen.NumberOfPlayers);
            float iniYSpeed = -500;
            float iniXSpeed = 0;

            // If the target is to the left of the AI player
            //if (targetPlayerIndex < gameScreen.CurrentPlayer)
            //{
            //    iniXSpeed = (float)((Terrain.g * (finalXPos - iniXPos)) / (-iniYSpeed - Math.Sqrt(Math.Pow(iniYSpeed, 2) - 2 * Terrain.g * (finalYPos - iniYPos))));
            //}

            // If the target is to the right of the AI player
            //else
            //{
                iniXSpeed = (float)((Terrain.g * (finalXPos - iniXPos)) / (-iniYSpeed + Math.Sqrt(Math.Pow(iniYSpeed, 2) - 2 * Terrain.g * (finalYPos - iniYPos))));
            //}

            Angle = (float)Math.Atan2(iniXSpeed, -iniYSpeed);
            Power = (float)Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2));

            
            
            
            
            
            
            
            
            
            
            //// If the target is to the left of the AI player
            //if (targetPlayerIndex < gameScreen.CurrentPlayer)
            //{
            //    // The X component for the speed is negative when it goes to the left
            //    iniXSpeed = -iniXSpeed;

            //    for (; iniXSpeed >= -1000; iniXSpeed--)
            //    {
            //        float t = (finalXPos - Position.X) / (iniXSpeed/* + windDirection.X*/);
            //        float iniYSpeed = ((finalYPos - Position.Y) / t) - (Terrain.g * t * (float)0.5);
            //        Power = (float)Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2));
            //        Angle = (float)Math.Atan2(iniYSpeed, iniXSpeed);

            //        if ((Angle >= (-Math.PI / 2) && Angle <= 0) &&
            //            (iniYSpeed >= -1000 && iniYSpeed <= 0) &&
            //            Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2)) <= 1000)
            //        {
            //            break;
            //        }
            //    }
            //}

            //// If the target is to the right of the AI player
            //else
            //{
            //    for (; iniXSpeed <= 1000; iniXSpeed++)
            //    {
            //        float t = (finalXPos - Position.X) / (iniXSpeed/* + windDirection.X*/);
            //        float iniYSpeed = ((finalYPos - Position.Y) / t) - (Terrain.g * t * (float)0.5);
            //        Power = (float)Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2));
            //        Angle = (float)Math.Atan2(iniYSpeed, iniXSpeed);

            //        if ((Angle >= 0 && Angle <= (Math.PI / 2)) &&
            //            (iniYSpeed >= -1000 && iniYSpeed <= 0) &&
            //            Math.Sqrt(Math.Pow(iniXSpeed, 2) + Math.Pow(iniYSpeed, 2)) <= 1000)
            //        {
            //            break;
            //        }
            //    }
            //}
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
