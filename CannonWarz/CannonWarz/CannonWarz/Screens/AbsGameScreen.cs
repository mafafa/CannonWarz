using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz.Screens
{
    public abstract class AbsGameScreen : AbsScreen
    {
        protected AbsGameScreen(Game1 game, SpriteBatch spriteBatch)
            : base(game, spriteBatch)
        {            
            NumberOfPlayers = 4;
            CurrentPlayer = 0;

            RocketIsFlying = false;
            _instantiatedRocketList = new List<Rocket>();

            CompassScaling = 0.25f;
            PinScaling = 0.25f;

            _explosionList = new List<Explosion>();
        }

        /**
         * <summary>    Called when the screen is back on top of the game screen manager's stack. </summary>
         */
        public override void Activate()
        {
        }

        /**
         * <summary>    Called when the screen is no longer on top of the game screen manager's stack. </summary>
         */
        public override void Deactivate()
        {
        }

        /**
         * <summary>    Initializes the screen by creating the terrain and the players. </summary>
         */
        public override void Initialize()
        {
            // We create the terrain object and then create the terrain contour
            _terrain = new Terrain(Game, Game.GroundTexture);
            _terrain.GenerateTerrainContour();

            // We generate the wind direction for the first player
            _terrain.GenerateWindDirection();

            // We create the players
            CreatePlayers(Game.CarriageTexture, Game.CannonTexture, Game.LifeBarTexture, Game.GreenBarTexture, Game.YellowBarTexture, Game.RedBarTexture);

            // This must be call after CreatePlayers() as terrain.TerrainContour is modified by CreateForeground()
            _terrain.CreateForeground(Game.GraphicsDevice);

            base.Initialize();
        }

        /**
         * <summary>    Updates the screen's logic. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public override void Update(GameTime gameTime)
        {
            List<Explosion> explosionToDelete = new List<Explosion>();

            CheckKeyboardKeysState(gameTime);

            if (RocketIsFlying)
            {
                UpdateRocket(gameTime);
                CheckCollisions(gameTime);
            }

            foreach (Explosion explosion in _explosionList)
            {
                if (explosion.ParticleList.Count > 0)
                {
                    UpdateExplosionParticles(gameTime);
                }

                else
                {
                    explosionToDelete.Add(explosion);
                }
            }

            // We delete the explosions that no longer have particles. Must be done here as we cannot modify
            // explosionList while iterating in it
            foreach (Explosion explosion in explosionToDelete)
            {
                _explosionList.Remove(explosion);
            }

            // If the game is finished, we push the win screen
            if (GameFinished)
            {
                GameScreenManager gameScreenManager = GameScreenManager.Instance;
                gameScreenManager.Push(new WinScreen(Game, SpriteBatch, PlayersArray[CurrentPlayer]));
            }
        }

        #region -------------------- UPDATE FUNCTIONS --------------------

        /**
         * <summary>    Check keyboard keys state. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        protected override void CheckKeyboardKeysState(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameScreenManager gameScreenManager = GameScreenManager.Instance;
                gameScreenManager.Push(new PauseScreen(Game, SpriteBatch));
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                PlayersArray[CurrentPlayer].Angle -= 1;
                VerifyAngleRange();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                PlayersArray[CurrentPlayer].Angle += 1;
                VerifyAngleRange();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                PlayersArray[CurrentPlayer].Power += 1;
                VerifyPowerRange();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                PlayersArray[CurrentPlayer].Power -= 1;
                VerifyPowerRange();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                PlayersArray[CurrentPlayer].Power += 20;
                VerifyPowerRange();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                PlayersArray[CurrentPlayer].Power -= 20;
                VerifyPowerRange();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (RocketIsFlying == false)
                {
                    RocketIsFlying = true;

                    Rocket rocket = new Rocket(Game.RocketTexture, Game.SmokeTexture, PlayersArray[CurrentPlayer].Position, PlayersArray[CurrentPlayer].Angle, PlayersArray[CurrentPlayer].Power);
                    _instantiatedRocketList.Add(rocket);
                }
            }
        }

        /**
         * <summary>    Updates the rocket's position and direction and creates smoke particles. </summary>
         */
        protected void UpdateRocket(GameTime gameTime)
        {
            if (RocketIsFlying)
            {
                Vector2 gravity = new Vector2(0, Terrain.g);
                float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (Rocket rocket in _instantiatedRocketList)
                {
                    rocket.RocketSpeed += Vector2.Multiply(gravity, deltaT);
                    rocket.RocketPosition += Vector2.Multiply(rocket.RocketSpeed, deltaT) + Vector2.Multiply(Vector2.Multiply(Vector2.Multiply(gravity, (float)0.5), deltaT), deltaT);

                    // We update the angle of the rocket accordingly
                    rocket.RocketAngle = (int)Math.Round(MathHelper.ToDegrees((float)Math.Atan2(rocket.RocketSpeed.X, -rocket.RocketSpeed.Y)));

                    rocket.CreateSmokeParticles(3);
                }
            }
        }

        /**
         * <summary>    
         *  Updates the position of each of the explosion particles on the screen and deletes them if 
         *  they have exceeded their lifetime.   
         * </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        protected void UpdateExplosionParticles(GameTime gameTime)
        {
            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            List<Explosion> explosionToDelete = new List<Explosion>();

            foreach (Explosion explosion in _explosionList)
            {
                // We scroll through the list backward so that we do not skip items if we delete some items
                for (int i = explosion.ParticleList.Count - 1; i >= 0; i--)
                {
                    CannonWarz.Explosion.ParticleData particle = explosion.ParticleList[i];
                    float timeAlive = now - particle.birthTime;

                    if (timeAlive > particle.maxAge)
                    {
                        // We delete the particle
                        explosion.ParticleList.RemoveAt(i);

                        // We add the explosion to the delete list if it has no more particles
                        if (explosion.ParticleList.Count == 0)
                        {
                            explosionToDelete.Add(explosion);
                        }
                    }

                    else
                    {
                        float relAge = timeAlive / particle.maxAge;
                        particle.position = 0.5f * particle.accelaration * relAge * relAge + particle.direction * relAge + particle.orginalPosition;

                        float invAge = 1.0f - relAge;
                        particle.modColor = new Color(new Vector4(invAge, invAge, invAge, invAge));

                        Vector2 positionFromCenter = particle.position - particle.orginalPosition;
                        float distance = positionFromCenter.Length();
                        particle.scaling = (50.0f + distance) / 200.0f;

                        explosion.ParticleList[i] = particle;
                    }
                }
            }
        }

        /**
         * <summary>    
         *  Handles each case of collisions: with the terrain, with the player and "rocket out 
         *  of screen" collisions. 
         * </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        protected void CheckCollisions(GameTime gameTime)
        {
            List<Rocket> rocketsToKill = new List<Rocket>();

            foreach (Rocket rocket in _instantiatedRocketList.ToList())
            {
                Vector2 terrainCollisionPoint = CheckTerrainCollision(rocket);
                Vector2 playerCollisionPoint = CheckPlayersCollision(rocket);
                bool rocketOutOfScreen = CheckRocketOutOfScreen(rocket);
                bool collisionAlreadyDetected = false;  // Sometimes 2 collisions can be detected at once and will change 2 times the player (there
                                                        // will be 2 calls to NextPlayer()). This is to avoid this.

                // If there was a collision or the rocket goes out of the screen, we delete the smoke trail and change to the next player
                if (playerCollisionPoint.X > -1 )
                {
                    RocketIsFlying = false;
                    collisionAlreadyDetected = true;

                    CreateExplosion(playerCollisionPoint, 10, 80.0f, 2000.0f, gameTime);

                    // We delete the rocket and the smoke
                    rocket.DeleteSmokeParticles();
                    rocketsToKill.Add(rocket);

                    NextPlayer();
                }

                if (terrainCollisionPoint.X > -1 && !collisionAlreadyDetected)
                {
                    RocketIsFlying = false;
                    collisionAlreadyDetected = true;

                    CreateExplosion(terrainCollisionPoint, 4, 30.0f, 1000.0f, gameTime);

                    // We delete the rocket and the smoke
                    rocket.DeleteSmokeParticles();
                    rocketsToKill.Add(rocket);

                    NextPlayer();
                }

                if (rocketOutOfScreen && !collisionAlreadyDetected)
                {
                    RocketIsFlying = false;

                    // We delete the rocket and the smoke
                    rocket.DeleteSmokeParticles();
                    rocketsToKill.Add(rocket);

                    NextPlayer();
                }
            }

            // We delete each rocket that had a collision
            foreach (Rocket rocket in rocketsToKill)
            {
                _instantiatedRocketList.Remove(rocket);
            }
        }

        /**
         * <summary>    Verifies if two textures collide. </summary>
         *
         * <param name="textureImage1"> The first 2D color array. </param>
         * <param name="matrixImage1">  The first matrix. </param>
         * <param name="textureImage2"> The second 2D color array. </param>
         * <param name="matrixImage2">  The second matrix. </param>
         *
         * <returns>    
         *  The position (X and Y coordinates) on the screen where the collision occurs, or (-1,-1) 
         *  if there is no collisions.              
         * </returns>
         */
        protected Vector2 TexturesCollide(Color[,] textureImage1, Matrix matrixImage1, Color[,] textureImage2, Matrix matrixImage2)
        {
            Matrix mat1to2 = matrixImage1 * Matrix.Invert(matrixImage2);
            int width1 = textureImage1.GetLength(0);
            int height1 = textureImage1.GetLength(1);
            int width2 = textureImage2.GetLength(0);
            int height2 = textureImage2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (textureImage1[x1, y1].A > 0)
                            {
                                if (textureImage2[x2, y2].A > 0)
                                {
                                    Vector2 screenPos = Vector2.Transform(pos1, matrixImage1);

                                    return screenPos;
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }

        /**
         * <summary>    
         *  Creates the matrix to use to verify if there is a collision with the terrain and passes 
         *  it to TexturesCollide().               
         * </summary>
         *
         * <param name="rocket">    The rocket we wish to verify if a collision occurs with. </param>
         *
         * <returns>    
         *  The position (X and Y coordinates) on the screen where the collision occurs, or (-1,-1) 
         *  if there is no collisions.              
         * </returns>
         */
        protected Vector2 CheckTerrainCollision(Rocket rocket)
        {
            Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rocket.RocketAngle)) *
                Matrix.CreateScale(rocket.RocketScaling) *
                Matrix.CreateTranslation(rocket.RocketPosition.X, rocket.RocketPosition.Y, 0);
            Matrix terrainMat = Matrix.Identity;

            Vector2 terrainCollisionPoint = TexturesCollide(rocket.RocketColorArray, rocketMat, _terrain.ForegroundColorArray, terrainMat);

            return terrainCollisionPoint;
        }

        /**
         * <summary>    
         *  Creates the matrix to use to verify if there is a collision with the cannon or the carriage 
         *  and passes it to TexturesCollide().               
         * </summary>
         *
         * <param name="rocket">    The rocket we wish to verify if a collision occurs with. </param>
         *
         * <returns>    
         *  The position (X and Y coordinates) on the screen where the collision occurs, or (-1,-1) 
         *  if there is no collisions.              
         * </returns>
         */
        protected Vector2 CheckPlayersCollision(Rocket rocket)
        {
            Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rocket.RocketAngle)) *
                Matrix.CreateScale(rocket.RocketScaling) *
                Matrix.CreateTranslation(rocket.RocketPosition.X, rocket.RocketPosition.Y, 0);

            // We verify if there is a collision between the rocket and each player that is alive and not the current player
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                AbsPlayer player = PlayersArray[i];
                if (player.IsAlive)
                {
                    if (i != CurrentPlayer)
                    {
                        int xPos = (int)player.Position.X;
                        int yPos = (int)player.Position.Y;

                        Matrix carriageMat = Matrix.CreateTranslation(0, -player.CarriageTexture.Height, 0) *
                            Matrix.CreateScale(player.PlayerScaling) *
                            Matrix.CreateTranslation(xPos, yPos, 0);

                        // First we verify if there is a collision with the carriage
                        Vector2 carriageCollisionPoint = TexturesCollide(player.CarriageColorArray, carriageMat, rocket.RocketColorArray, rocketMat);

                        Matrix cannonMat = Matrix.CreateTranslation(-11, -50, 0) *
                            Matrix.CreateRotationZ(MathHelper.ToRadians(player.Angle)) *
                            Matrix.CreateScale(player.PlayerScaling) *
                            Matrix.CreateTranslation(xPos + 20, yPos - 10, 0);

                        // TODO: Fix cannon collision. Sometimes when the rocket hits the cannon, it does not register
                        // We also verify if there is a collision with the cannon
                        Vector2 cannonCollisionPoint = TexturesCollide(player.CannonColorArray, cannonMat, rocket.RocketColorArray, rocketMat);

                        // If there is a collision, the player dies and we return the collision point
                        if (carriageCollisionPoint.X > -1)
                        {
                            PlayersArray[i].HP--;

                            if (PlayersArray[i].HP <= 0)
                            {
                                PlayersArray[i].IsAlive = false;
                            }

                            return carriageCollisionPoint;
                        }
                        if (cannonCollisionPoint.X > -1)
                        {
                            PlayersArray[i].HP--;

                            if (PlayersArray[i].HP <= 0)
                            {
                                PlayersArray[i].IsAlive = false;
                            }

                            return cannonCollisionPoint;
                        }
                    }
                }
            }

            // If there is no collision, we return (-1, -1)
            return new Vector2(-1, -1);
        }

        /**
         * <summary>    Checks if the rocket is out of screen. </summary>
         *
         * <param name="rocket">    The rocket we wish to verify if is out of screen. </param>
         *
         * <returns>    true if it is out of screen, false if it is not. </returns>
         */
        protected bool CheckRocketOutOfScreen(Rocket rocket)
        {
            bool rocketOutOfScreen = rocket.RocketPosition.Y > Game.ScreenHeight;
            rocketOutOfScreen |= rocket.RocketPosition.X < 0;
            rocketOutOfScreen |= rocket.RocketPosition.X > Game.ScreenWidth;

            return rocketOutOfScreen;
        }

        #endregion

        /**
         * <summary>    Draws the screen components on the screen. </summary>
         *
         * <param name="gameTime">  Time of the game. </param>
         */
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            DrawScenery();
            DrawPlayers();
            DrawScreenOverlay();
            foreach (Rocket rocket in _instantiatedRocketList)
            {
                DrawRocket(rocket);
                DrawSmoke(rocket);
            }
            SpriteBatch.End();

            // We use additive alpha blending for the explosions
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            DrawExplosion();
            SpriteBatch.End();
        }

        #region --------------------- DRAW FUNCTIONS ---------------------

        /**
         * <summary>    Draws the terrain foreground scenery. </summary>
         */
        protected void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, Game.ScreenWidth, Game.ScreenHeight);
            SpriteBatch.Draw(_terrain.ForegroundTexture, screenRectangle, Color.White);
        }

        /**
         * <summary>    Draws players (cannons, carriages and HP bars). </summary>
         */
        protected void DrawPlayers()
        {
            Vector2 cannonOrigin = new Vector2(11, 50); // Rotation point for the cannon

            foreach (AbsPlayer player in PlayersArray)
            {
                if (player.IsAlive)
                {
                    // We position the cannon so it is well positioned on the carriage
                    float cannonPosX = player.Position.X + 30;
                    float cannonPosY = player.Position.Y - 15;

                    SpriteBatch.Draw(player.CarriageTexture, player.Position, null, player.Color, 0, new Vector2(0, player.CarriageTexture.Height), player.PlayerScaling, SpriteEffects.None, 0);
                    SpriteBatch.Draw(player.CannonTexture, new Vector2(cannonPosX, cannonPosY), null, player.Color, MathHelper.ToRadians(player.Angle), cannonOrigin, player.PlayerScaling, SpriteEffects.None, 1);

                    // We position the life bar
                    float lifeBarPosX = player.Position.X + 15;
                    float lifeBarPosY = player.Position.Y + 15;
                    float HPBarPosY = lifeBarPosY - 1;
                    Texture2D HPBarTexture = player.GreenBarTexture;

                    // Depending on the HP of the player, we use the green yellow or red texture
                    if (player.HP == 3)
                    {
                        HPBarTexture = player.GreenBarTexture;
                    }
                    else if (player.HP == 2)
                    {
                        HPBarTexture = player.YellowBarTexture;
                    }
                    else if (player.HP == 1)
                    {
                        HPBarTexture = player.RedBarTexture;
                    }

                    SpriteBatch.Draw(player.LifeBarTexture, new Vector2(lifeBarPosX, lifeBarPosY), null, player.Color, 0, new Vector2(0, player.LifeBarTexture.Height), player.LifeBarScaling, SpriteEffects.None, 0);
                    for (uint i = player.HP, j = 0; i > 0; i--, j++)
                    {
                        float HPBarPosX = (2 + lifeBarPosX) + j * (1 + (player.HPBarScaling * player.GreenBarTexture.Width)); // Will position the HP bars perfectly in the lifeBar
                        SpriteBatch.Draw(HPBarTexture, new Vector2(HPBarPosX, HPBarPosY), null, Color.White, 0, cannonOrigin, player.HPBarScaling, SpriteEffects.None, 1);
                    }
                }
            }
        }

        /**
         * <summary>    Draws the screen overlay. </summary>
         */
        protected void DrawScreenOverlay()
        {
            // We draw the power and angle strings
            int currentAngle = PlayersArray[CurrentPlayer].Angle;

            SpriteBatch.DrawString(Game.GeneralFont, "Cannon angle: " + currentAngle.ToString(), new Vector2(20, 20), PlayersArray[CurrentPlayer].Color);
            SpriteBatch.DrawString(Game.GeneralFont, "Cannon power: " + PlayersArray[CurrentPlayer].Power.ToString(), new Vector2(20, 45), PlayersArray[CurrentPlayer].Color);

            // We draw the compass and the pin
            Vector2 pinOrigin = new Vector2(90, 45); // Rotation point for the pin
            float windAngle = (float)((-Math.PI / 2) + Math.Atan2(_terrain.WindDirection.X, -_terrain.WindDirection.Y));
            // TODO: Make the pin scale in function of the power of the wind
            //float pinScale = (float)(terrain.WindDirection.Y / Math.Cos(windAngle));
            float compassPosX = Game.ScreenWidth - 100;
            float compassPosY = 20;
            float pinPosX = compassPosX + ((Game.CompassTexture.Width * CompassScaling) / 2);
            float pinPosY = compassPosY + ((Game.CompassTexture.Height * CompassScaling) / 2);

            SpriteBatch.Draw(Game.CompassTexture, new Vector2(compassPosX, compassPosY), null, Color.White, 0, new Vector2(0, 0), CompassScaling, SpriteEffects.None, 0);
            SpriteBatch.Draw(Game.PinTexture, new Vector2(pinPosX, pinPosY), null, PlayersArray[CurrentPlayer].Color, windAngle, pinOrigin, PinScaling/*pinScale*/, SpriteEffects.None, 1);
        }

        /**
         * <summary>    Draws the rocket. </summary>
         *
         * <param name="rocket">    The rocket we wish to draw. </param>
         */
        protected void DrawRocket(Rocket rocket)
        {
            if (RocketIsFlying)
            {
                SpriteBatch.Draw(Game.RocketTexture, rocket.RocketPosition, null, PlayersArray[CurrentPlayer].Color, MathHelper.ToRadians(rocket.RocketAngle), new Vector2(42, 240), rocket.RocketScaling, SpriteEffects.None, 1);
            }
        }

        /**
         * <summary>    Draws the rocket's smoke. </summary>
         *
         * <param name="rocket">    The rocket we wish to draw the smoke from. </param>
         */
        protected void DrawSmoke(Rocket rocket)
        {
            foreach (Vector2 smokePos in rocket.SmokeParticleList)
            {
                SpriteBatch.Draw(Game.SmokeTexture, smokePos, null, Color.White, 0, new Vector2(40, 35), rocket.SmokeScaling, SpriteEffects.None, 1);
            }
        }

        /**
         * <summary>    Draws the explosions when a collision occurs with the rocket. </summary>
         */
        protected void DrawExplosion()
        {
            foreach (Explosion explosion in _explosionList)
            {
                for (int i = 0; i < explosion.ParticleList.Count; i++)
                {
                    CannonWarz.Explosion.ParticleData particle = explosion.ParticleList[i];

                    SpriteBatch.Draw(Game.ExplosionParticleTextures, particle.position, null, particle.modColor, i, new Vector2(256, 256), particle.scaling, SpriteEffects.None, 1); // Each particles will be rotated i = 1 rad = 57 degrees relative to the previous particle
                }
            }
        }

        #endregion

        /**
         * <summary>    Verifies if the shot's power is out of range. If it is, it adjusts it. </summary>
         */
        protected void VerifyPowerRange()
        {
            if (PlayersArray[CurrentPlayer].Power > 1000)
            {
                PlayersArray[CurrentPlayer].Power = 1000;
            }
            if (PlayersArray[CurrentPlayer].Power < 0)
            {
                PlayersArray[CurrentPlayer].Power = 0;
            }
        }

        /**
         * <summary>    Verifies if the shot's angle is out of range. If it is, it adjusts it. </summary>
         */
        protected void VerifyAngleRange()
        {
            if (PlayersArray[CurrentPlayer].Angle > 90)
            {
                PlayersArray[CurrentPlayer].Angle = -90;
            }
            if (PlayersArray[CurrentPlayer].Angle < -90)
            {
                PlayersArray[CurrentPlayer].Angle = 90;
            }
        }

        /**
         * <summary>    Creates the players, calls PlacePlayer() and flattens the terrain under the players. </summary>
         *
         * <param name="carriageTexture">   The carriage texture. </param>
         * <param name="cannonTexture">     The cannon texture. </param>
         * <param name="lifeBarTexture">    The life bar texture. </param>
         * <param name="greenBarTexture">   The green bar texture. </param>
         * <param name="yellowBarTexture">  The yellow bar texture. </param>
         * <param name="redBarTexture">     The red bar texture. </param>
         */
        protected abstract void CreatePlayers(Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture);

        /**
         * <summary>    Switches to the next player's turn and calls GenerateWindDirection(). </summary>
         */
        protected abstract void NextPlayer();

        /**
         * <summary>    Creates an explosion and adds it to the explosion list. </summary>
         *
         * <param name="playerCollisionPoint">  The player's collision point. </param>
         * <param name="numberOfParticles">     Number of particles to generate. </param>
         * <param name="size">                  The size of the explosion. </param>
         * <param name="maxAge">                The particles' maximum age. </param>
         * <param name="gameTime">              Time of the game. </param>
         */
        protected void CreateExplosion(Vector2 playerCollisionPoint, int numberOfParticles, float size, float maxAge, GameTime gameTime)
        {
            Explosion explosion = new Explosion(Game.ExplosionParticleTextures);
            _explosionList.Add(explosion);

            explosion.AddExplosion(playerCollisionPoint, numberOfParticles, size, maxAge, gameTime);
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        protected Terrain _terrain;

        protected List<Rocket> _instantiatedRocketList;

        protected List<Explosion> _explosionList;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        /**
         * <summary>    Gets or sets the compass' pin scaling. </summary>
         *
         * <value>  The compass' pin scaling. </value>
         */
        public float PinScaling
        {
            get;
            protected set;
        }

        /**
         * <summary>    Gets or sets the compass scaling. </summary>
         *
         * <value>  The compass scaling. </value>
         */
        public float CompassScaling
        {
            get;
            protected set;
        }

        /**
         * <summary>    Gets or sets a value indicating whether the rocket is flying. </summary>
         *
         * <value>  true if rocket is flying, false if not. </value>
         */
        public bool RocketIsFlying
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets an array of active players. </summary>
         *
         * <value>  An Array of active players. </value>
         */
        public AbsPlayer[] PlayersArray
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the number of players to be drawn/updated. </summary>
         *
         * <value>  The total number of players. </value>
         */
        public int NumberOfPlayers
        {
            get;
            set;
        }

        /**
         * <summary>    Gets or sets the index of the current player. </summary>
         *
         * <value>  The index of the current player. </value>
         */
        public int CurrentPlayer
        {
            get;
            set;
        }

        /**
         * <summary>    Gets a value indicating whether the game is finished. </summary>
         *
         * <value>  true if finished, false if not. </value>
         */
        public bool GameFinished
        {
            get
            {
                int alivePlayer = 0;

                foreach (AbsPlayer player in PlayersArray)
                {
                    if (player.IsAlive)
                    {
                        alivePlayer++;
                    }
                }

                if (alivePlayer == 1)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
        }

        #endregion
    }
}
