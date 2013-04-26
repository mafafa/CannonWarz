CannonWarz
==========

This is a small 2D cannon game I made with the XNA framework by following a tutorial to learn how to use that framework. I am planning on adding several new features in the future. The game was created in Windows using Visual Studio 2010.

Features for now include:
- Randomly generated maps each time you open the game (though the cannons' position are fixed).
- Randomly generated wind that changes and the end of each player's turn.
- Particles (when the rocket explodes)

Controls (until I make them configurable)
- Left and Right arrows to modify the cannon's angle.
- Up and down arrows to raise/lower the cannon's shot strenght slowly.
- Pageup and pagedown to raise/lower the cannon's shot strenght fast.

Dependencies:
- Microsoft .NET 4: http://www.microsoft.com/en-ca/download/details.aspx?id=17851
- Microsoft XNA Game Studio 4: http://www.microsoft.com/en-ca/download/details.aspx?id=23714
- Inno Setup 5.4.3 or higher (only used for the installation executable): http://www.jrsoftware.org/isdl.php

How to compile the game's executable:
- Simply open Microsoft Visual Studio and compile it in release or debug configuration for the x86 platform.
- The files needed to play the game will be in CannonWarz\CannonWarz\CannonWarz\bin\x86\Release\ or Debug\ folder. You do not need the .pdb file contained in those folders to run the game.

How to compile the game's installation package:
- If you wish to compile the automatic installation executable for the game, you must have Inno Setup 5.4.3 or higher installed.
- Compile the game's executable in release configuration for the x86 platform.
- Make sure that both dotNetFx40_Client_x86_x64 and xnafx40_redist.msi are present in the CannonWarz\CannonWarz\Installation\Include\ folder. Those files are needed so that the game's installation executable can install both those dependencies if the client PC does not already have them.
- Open CannonWarz\CannonWarz\InstallPackGen.iss and compile the installation package.
- The game's installation executable will be in CannonWarz\CannonWarz\Installation\.
