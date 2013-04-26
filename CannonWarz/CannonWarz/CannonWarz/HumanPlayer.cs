using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{    
    /// <summary>
    /// Player class. Each instantiated object of this class represents a player in the game.
    /// </summary>
    public class HumanPlayer : AbsPlayer
    {
        public HumanPlayer(Color color, Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
            : base(color, carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture)
        {
        }

        public override void PlacePlayer(int numberOfPlayers, int numberOfCurrPlayer, int screenWidth, Terrain terrain)
        {
            Vector2 position = new Vector2();

            // We divide the screen in (numberOfPlayers + 1) sections and place the player there at the Y height of that position
            position.X = (screenWidth / (numberOfPlayers + 1)) * (numberOfCurrPlayer + 1);
            position.Y = terrain.TerrainContour[(int)position.X];

            Position = position;
        }
    }
}
