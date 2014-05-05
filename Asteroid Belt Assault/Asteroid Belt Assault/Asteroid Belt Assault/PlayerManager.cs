using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroid_Belt_Assault
{
    class PlayerManager
    {
        public Sprite newPlayer;
        private float playerSpeed = 160.0f;
        private Rectangle playerAreaLimit;

        public long PlayerScore = 0;
        public int LivesRemaining = 3;
        public bool Destroyed = false;

        private Vector2 gunOffset = new Vector2(25, 10);
        private float shotTimer = 0.0f;
        private float minShotTimer = -0.8f;
        private int playerRadius = 15;
        public ShotManager PlayerShotManager;

        public PlayerManager(
            Texture2D texture,  
            Rectangle initialFrame,
            int frameCount,
            Rectangle screenBounds)
        {
            newPlayer = new Sprite(
                new Vector2(500, 500),
                texture,
                initialFrame,
                Vector2.Zero);

            PlayerShotManager = new ShotManager(
                texture,
                new Rectangle(0, 300, 5, 5),
                4,
                2,
                250f,
                screenBounds);

            playerAreaLimit =
                new Rectangle(
                    0,
                    screenBounds.Height / 2,
                    screenBounds.Width,
                    screenBounds.Height / 2);

            for (int x = 1; x < frameCount; x++)
            {
                newPlayer.AddFrame(
                    new Rectangle(
                        initialFrame.X + (initialFrame.Width * x),
                        initialFrame.Y,
                        initialFrame.Width,
                        initialFrame.Height));
            }
            newPlayer.CollisionRadius = playerRadius;
        }

        private void FireShot()
        {
            if (shotTimer >= minShotTimer)
            {
                PlayerShotManager.FireShot(
                    newPlayer.Center,
                    new Vector2(0, 0),
                    true);
                shotTimer = 0.0f;
            }
        }
        private void HandleMouseInput(MouseState mouseState)
        {
             
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                FireShot();
            }
        }


        private void HandleKeyboardInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Up))
            {
                newPlayer.Velocity += new Vector2(0, -1);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                newPlayer.Velocity += new Vector2(0, 1);
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                newPlayer.Velocity += new Vector2(-1, 0);
                //newPlayer.Rotation -= 0.04f;
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                newPlayer.Velocity += new Vector2(1, 0);
               // newPlayer.Rotation += 0.04f;
            }

            if (keyState.IsKeyDown(Keys.Space))
            {
                FireShot();
            }

            if (keyState.IsKeyDown(Keys.Right) && keyState.IsKeyDown(Keys.Up))
            {

                newPlayer.Rotation += 0.04f;
            }

            if (keyState.IsKeyDown(Keys.Left) && keyState.IsKeyDown(Keys.Up))
            {

                newPlayer.Rotation -= 0.04f;
            }


        }

        private void HandleGamepadInput(GamePadState gamePadState)
        {
            newPlayer.Velocity +=
                new Vector2(
                    gamePadState.ThumbSticks.Left.X,
                    -gamePadState.ThumbSticks.Left.Y);

            if (gamePadState.Buttons.A == ButtonState.Pressed)
            {
                FireShot();
            }
        }

        private void imposeMovementLimits()
        {
            Vector2 location = newPlayer.Location;

            if (location.X < playerAreaLimit.X)
                location.X = playerAreaLimit.X;

            if (location.X >
                (playerAreaLimit.Right - newPlayer.Source.Width))
                location.X =
                    (playerAreaLimit.Right - newPlayer.Source.Width);

            if (location.Y < playerAreaLimit.Y)
                location.Y = playerAreaLimit.Y;

            if (location.Y >
                (playerAreaLimit.Bottom - newPlayer.Source.Height))
                location.Y =
                    (playerAreaLimit.Bottom - newPlayer.Source.Height);

            newPlayer.Location = location;
        }

        public void Update(GameTime gameTime)
        {
            PlayerShotManager.Update(gameTime);

            if (!Destroyed)
            {
                newPlayer.Velocity = Vector2.Zero;

                shotTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                HandleKeyboardInput(Keyboard.GetState());
                HandleGamepadInput(GamePad.GetState(PlayerIndex.One));

                newPlayer.Velocity.Normalize();
                newPlayer.Velocity *= playerSpeed;

                newPlayer.Update(gameTime);
                imposeMovementLimits();
            }

            if (PlayerScore == 200)
            {
                newPlayer.Velocity *= (playerSpeed * 300);
 
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerShotManager.Draw(spriteBatch);

            if (!Destroyed)
            {
                newPlayer.Draw(spriteBatch);
            }
        }

    }
}
