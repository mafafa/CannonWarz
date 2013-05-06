﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CannonWarz.Screens
{
    class GameScreen : AbsScreen
    {
        public GameScreen(Game1 game, SpriteBatch spriteBatch)
            : base(game)
        {
            SpriteBatch = spriteBatch;

            NumberOfPlayers = 4;
            CurrentPlayer = 0;

            RocketIsFlying = false;
            instantiatedRocketList = new List<Rocket>();

            CompassScaling = 0.25f;
            PinScaling = 0.25f;

            explosionList = new List<Explosion>();
        }

        public override void Activate()
        {

        }

        public override void Deactivate()
        {

        }

        public override void Initialize()
        {
            // We create the terrain object and then create the terrain contour
            terrain = new Terrain(Game, Game.GroundTexture);
            terrain.GenerateTerrainContour();

            // We generate the wind direction for the first player
            terrain.GenerateWindDirection();

            // We create the players
            CreatePlayers(Game.CarriageTexture, Game.CannonTexture, Game.LifeBarTexture, Game.GreenBarTexture, Game.YellowBarTexture, Game.RedBarTexture);

            // This must be call after CreatePlayers() as terrain.TerrainContour is modified by CreateForeground()
            terrain.CreateForeground(Game.GraphicsDevice);
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            List<Explosion> explosionToDelete = new List<Explosion>();

            CheckKeyboardKeysState(gameTime);

            if (RocketIsFlying)
            {
                UpdateRocket();
                CheckCollisions(gameTime);
            }

            foreach (Explosion explosion in explosionList)
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
                explosionList.Remove(explosion);
            }

            base.Update(gameTime);
        }

        #region -------------------- UPDATE FUNCTIONS --------------------

        private void UpdateRocket()
        {
            if (RocketIsFlying)
            {
                // Gravity effect on the rocket
                Vector2 gravity = new Vector2(0, 1);

                foreach (Rocket rocket in instantiatedRocketList)
                {
                    rocket.RocketDirection += gravity / 10.0f;
                    rocket.RocketDirection += terrain.WindDirection;
                    rocket.RocketPosition += rocket.RocketDirection;

                    // We update the angle of the rocket accordingly
                    rocket.RocketAngle = (float)Math.Atan2(rocket.RocketDirection.X, -rocket.RocketDirection.Y);

                    rocket.CreateSmokeParticles(3);
                }
            }
        }

        private void UpdateExplosionParticles(GameTime gameTime)
        {
            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            List<Explosion> explosionToDelete = new List<Explosion>();

            foreach (Explosion explosion in explosionList)
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

        private void CheckKeyboardKeysState(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                PlayersArray[CurrentPlayer].Angle -= 0.01f;
                VerifyAngleRange();
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                PlayersArray[CurrentPlayer].Angle += 0.01f;
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
                    instantiatedRocketList.Add(rocket);
                }
            }
        }

        private void CheckCollisions(GameTime gameTime)
        {
            List<Rocket> rocketsToKill = new List<Rocket>();

            foreach (Rocket rocket in instantiatedRocketList)
            {
                Vector2 terrainCollisionPoint = CheckTerrainCollision(rocket);
                Vector2 playerCollisionPoint = CheckPlayersCollision(rocket);
                bool rocketOutOfScreen = CheckRocketOutOfScreen(rocket);

                // If there was a collision or the rocket goes out of the screen, we delete the smoke trail and change to the next player
                if (playerCollisionPoint.X > -1)
                {
                    RocketIsFlying = false;

                    CreateExplosion(playerCollisionPoint, 10, 80.0f, 2000.0f, gameTime);

                    // We delete the rocket and the smoke
                    rocket.DeleteSmokeParticles();
                    rocketsToKill.Add(rocket);

                    NextPlayer();
                }

                if (terrainCollisionPoint.X > -1)
                {
                    RocketIsFlying = false;

                    CreateExplosion(terrainCollisionPoint, 4, 30.0f, 1000.0f, gameTime);

                    // We delete the rocket and the smoke
                    rocket.DeleteSmokeParticles();
                    rocketsToKill.Add(rocket);

                    NextPlayer();
                }

                if (rocketOutOfScreen)
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
                instantiatedRocketList.Remove(rocket);
            }
        }

        private Vector2 TexturesCollide(Color[,] textureImage1, Matrix matrixImage1, Color[,] textureImage2, Matrix matrixImage2)
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

        private Vector2 CheckTerrainCollision(Rocket rocket)
        {
            Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) *
                Matrix.CreateRotationZ(rocket.RocketAngle) *
                Matrix.CreateScale(rocket.RocketScaling) *
                Matrix.CreateTranslation(rocket.RocketPosition.X, rocket.RocketPosition.Y, 0);
            Matrix terrainMat = Matrix.Identity;

            Vector2 terrainCollisionPoint = TexturesCollide(rocket.RocketColorArray, rocketMat, terrain.ForegroundColorArray, terrainMat);

            return terrainCollisionPoint;
        }

        private Vector2 CheckPlayersCollision(Rocket rocket)
        {
            Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) *
                Matrix.CreateRotationZ(rocket.RocketAngle) *
                Matrix.CreateScale(rocket.RocketScaling) *
                Matrix.CreateTranslation(rocket.RocketPosition.X, rocket.RocketPosition.Y, 0);

            // We verify if there is a collision between the rocket and each player that is alive and not the current player
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                HumanPlayer player = PlayersArray[i];
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
                            Matrix.CreateRotationZ(player.Angle) *
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

        private bool CheckRocketOutOfScreen(Rocket rocket)
        {
            bool rocketOutOfScreen = rocket.RocketPosition.Y > Game.ScreenHeight;
            rocketOutOfScreen |= rocket.RocketPosition.X < 0;
            rocketOutOfScreen |= rocket.RocketPosition.X > Game.ScreenWidth;

            return rocketOutOfScreen;
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            DrawScenery();
            DrawPlayers();
            DrawScreenOverlay();
            foreach (Rocket rocket in instantiatedRocketList)
            {
                DrawRocket(rocket);
                DrawSmoke(rocket);
            }
            SpriteBatch.End();

            // We use additive alpha blending for the explosions
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            DrawExplosion();
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        #region --------------------- DRAW FUNCTIONS ---------------------

        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, Game.ScreenWidth, Game.ScreenHeight);
            SpriteBatch.Draw(terrain.ForegroundTexture, screenRectangle, Color.White);
        }

        private void DrawPlayers()
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
                    SpriteBatch.Draw(player.CannonTexture, new Vector2(cannonPosX, cannonPosY), null, player.Color, player.Angle, cannonOrigin, player.PlayerScaling, SpriteEffects.None, 1);

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

        private void DrawScreenOverlay()
        {
            // We draw the power and angle strings
            int currentAngle = (int)MathHelper.ToDegrees(PlayersArray[CurrentPlayer].Angle);

            SpriteBatch.DrawString(Game.GeneralFont, "Cannon angle: " + currentAngle.ToString(), new Vector2(20, 20), PlayersArray[CurrentPlayer].Color);
            SpriteBatch.DrawString(Game.GeneralFont, "Cannon power: " + PlayersArray[CurrentPlayer].Power.ToString(), new Vector2(20, 45), PlayersArray[CurrentPlayer].Color);

            // We draw the compass and the pin
            Vector2 pinOrigin = new Vector2(90, 45); // Rotation point for the pin
            float windAngle = (float)((-Math.PI / 2) + Math.Atan2(terrain.WindDirection.X, -terrain.WindDirection.Y));
            // TODO: Make the pin scale in function of the power of the wind
            //float pinScale = (float)(terrain.WindDirection.Y / Math.Cos(windAngle));
            float compassPosX = Game.ScreenWidth - 100;
            float compassPosY = 20;
            float pinPosX = compassPosX + ((Game.CompassTexture.Width * CompassScaling) / 2);
            float pinPosY = compassPosY + ((Game.CompassTexture.Height * CompassScaling) / 2);

            SpriteBatch.Draw(Game.CompassTexture, new Vector2(compassPosX, compassPosY), null, Color.White, 0, new Vector2(0, 0), CompassScaling, SpriteEffects.None, 0);
            SpriteBatch.Draw(Game.PinTexture, new Vector2(pinPosX, pinPosY), null, PlayersArray[CurrentPlayer].Color, windAngle, pinOrigin, PinScaling/*pinScale*/, SpriteEffects.None, 1);
        }

        private void DrawRocket(Rocket rocket)
        {
            if (RocketIsFlying)
            {
                SpriteBatch.Draw(Game.RocketTexture, rocket.RocketPosition, null, PlayersArray[CurrentPlayer].Color, rocket.RocketAngle, new Vector2(42, 240), rocket.RocketScaling, SpriteEffects.None, 1);
            }
        }

        private void DrawSmoke(Rocket rocket)
        {
            foreach (Vector2 smokePos in rocket.SmokeParticleList)
            {
                SpriteBatch.Draw(Game.SmokeTexture, smokePos, null, Color.White, 0, new Vector2(40, 35), rocket.SmokeScaling, SpriteEffects.None, 1);
            }
        }

        private void DrawExplosion()
        {
            foreach (Explosion explosion in explosionList)
            {
                for (int i = 0; i < explosion.ParticleList.Count; i++)
                {
                    CannonWarz.Explosion.ParticleData particle = explosion.ParticleList[i];

                    SpriteBatch.Draw(Game.ExplosionParticleTextures, particle.position, null, particle.modColor, i, new Vector2(256, 256), particle.scaling, SpriteEffects.None, 1); // Each particles will be rotated i = 1 rad = 57 degrees relative to the previous particle
                }
            }
        }

        #endregion

        private void VerifyPowerRange()
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

        private void VerifyAngleRange()
        {
            if (PlayersArray[CurrentPlayer].Angle > MathHelper.PiOver2)
            {
                PlayersArray[CurrentPlayer].Angle = -MathHelper.PiOver2;
            }
            if (PlayersArray[CurrentPlayer].Angle < -MathHelper.PiOver2)
            {
                PlayersArray[CurrentPlayer].Angle = MathHelper.PiOver2;
            }
        }

        private void CreatePlayers(Texture2D carriageTexture, Texture2D cannonTexture, Texture2D lifeBarTexture, Texture2D greenBarTexture, Texture2D yellowBarTexture, Texture2D redBarTexture)
        {
            Color[] playerColors = new Color[10];
            playerColors[0] = Color.Red;
            playerColors[1] = Color.Green;
            playerColors[2] = Color.Blue;
            playerColors[3] = Color.Purple;
            playerColors[4] = Color.Orange;
            playerColors[5] = Color.Indigo;
            playerColors[6] = Color.Yellow;
            playerColors[7] = Color.SaddleBrown;
            playerColors[8] = Color.Tomato;
            playerColors[9] = Color.Turquoise;

            PlayersArray = new HumanPlayer[NumberOfPlayers];
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                // We create the player
                PlayersArray[i] = new HumanPlayer(playerColors[i], carriageTexture, cannonTexture, lifeBarTexture, greenBarTexture, yellowBarTexture, redBarTexture);

                // We set his position
                PlayersArray[i].PlacePlayer(NumberOfPlayers, i, Game.ScreenWidth, terrain);
            }

            // We flatten the ground under their positions
            terrain.FlattenTerrainBelowPlayers(PlayersArray);
        }

        private void NextPlayer()
        {
            // We increment the current player
            CurrentPlayer++;

            // To be sure that we do not go pass the NumberOfPlayers
            CurrentPlayer = CurrentPlayer % NumberOfPlayers;

            // We do the two last manipulations until we find a player that is alive
            while (!PlayersArray[CurrentPlayer].IsAlive)
            {
                CurrentPlayer = ++CurrentPlayer % NumberOfPlayers;
            }

            // We change the wind direction for the next player
            terrain.GenerateWindDirection();
        }

        private void CreateExplosion(Vector2 playerCollisionPoint, int numberOfParticles, float size, float maxAge, GameTime gameTime)
        {
            Explosion explosion = new Explosion(Game.ExplosionParticleTextures);
            explosionList.Add(explosion);

            explosion.AddExplosion(playerCollisionPoint, numberOfParticles, size, maxAge, gameTime);
        }

        #region --------------------- PRIVATE FIELDS ---------------------

        //private SpriteFont font;

        private Terrain terrain;

        /*private Texture2D compassTexture;
        private float compassScaling;
        private Texture2D pinTexture;
        private float pinScaling;*/

        private List<Rocket> instantiatedRocketList;
        //private Texture2D rocketTexture;
        //private Texture2D smokeTexture;

        //private Texture2D explosionParticleTextures;
        private List<Explosion> explosionList;

        #endregion

        #region ----------------------- PROPERTIES -----------------------

        public float PinScaling
        {
            get;
            private set;
        }

        public float CompassScaling
        {
            get;
            private set;
        }
        
        public bool RocketIsFlying
        {
            get;
            set;
        }

        public HumanPlayer[] PlayersArray
        {
            get;
            set;
        }

        public int NumberOfPlayers
        {
            get;
            set;
        }

        public int CurrentPlayer
        {
            get;
            set;
        }

        #endregion
    }
}