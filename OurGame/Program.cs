﻿#region Using Statements

using System;

#endregion

using OurGame.WindowsGame1;

// Our usings.

namespace OurGame
{
#if WINDOWS || LINUX
    /// <summary>
    ///     The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}