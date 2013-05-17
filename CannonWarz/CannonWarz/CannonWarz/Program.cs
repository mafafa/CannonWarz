/**
// file:	Program.cs
//
// summary:	Implements the main entry point for the application
 */

using System;

namespace CannonWarz
{
#if WINDOWS || XBOX

    /**
     * <summary>    Program class. Main entry point for the game. </summary>
     */
    static class Program
    {
        /**
         * <summary>    Main entry-point for this application. </summary>
         *
         * <param name="args">  Array of command-line argument strings. </param>
         */
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

