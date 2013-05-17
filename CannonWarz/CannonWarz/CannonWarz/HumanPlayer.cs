/**
// file:	HumanPlayer.cs
//
// summary:	Implements the human player class
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CannonWarz
{
    /**
     * <summary>    
     *  Player class. Each instantiated object of this class represents a player in the game.           
     * </summary>
     */
    public class HumanPlayer : AbsPlayer
    {
        /**
         * <summary>    Constructor. </summary>
         *
         * <param name="color">             The color of the player's carriage/cannon. </param>
         * <param name="carriageTexture">   The carriage texture. </param>
         * <param name="cannonTexture">     The cannon texture. </param>
         * <param name="lifeBarTexture">    The life bar texture. </param>
         * <param name="greenBarTexture">   The green bar texture. </param>
         * <param name="yellowBarTexture">  The yellow bar texture. </param>
         * <param name="redBarTexture">     The red bar texture. </param>
         */
        public HumanPlayer(Color color, Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
            : base(color, carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture)
        {
        }
    }
}
