using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    public class AIPlayer : AbsPlayer
    {
        public AIPlayer(Color color, Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
            : base(color, carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture)
        {
            Random randomShotFactor = new Random(1);
            LastShotUncertaintyValue = randomShotFactor.Next(0, 50);
            
            Random randomAngleFactor = new Random(2);
            LastAngleUncertaintyValue = randomAngleFactor.Next((int)(-Math.PI / 8), (int)(Math.PI / 8));
        }

        public void PlayTurn(AbsPlayer[] playersArray, Vector2 windDirection)
        {
            // We select a random player
            Random randomTargetFactor = new Random(3);
            int targetPlayerIndex = randomTargetFactor.Next(0, 4);

            // We determine the shot power value
            

            // We determine the angle value
            
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
