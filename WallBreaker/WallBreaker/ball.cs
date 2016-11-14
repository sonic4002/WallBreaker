using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WallBreaker
{
    class ball : DrawableGameComponent
    {
        public Texture2D ballSprite;
        public Rectangle ballRectangle;
        public Vector2 ballPosition;
        public Vector2 ballSpeed;
        Game game;
        int maxX;
        int maxY;
        SpriteBatch spriteBatch;
        Breaker breaker;

        public ball(Game game) : base (game)
        {
            ballSprite = game.Content.Load<Texture2D>("ball");
            ballSpeed = new Vector2(170, -170);
            this.game = game;
            maxX = this.game.GraphicsDevice.Viewport.Width - ballSprite.Width;
            maxY = this.game.GraphicsDevice.Viewport.Height - ballSprite.Height;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            breaker= (Breaker)game.Services.GetService(typeof(Breaker));
            ballPosition = new Vector2(breaker.breakerPosition.X + breaker.BreakerSprite.Width / 2, breaker.breakerPosition.Y - ballSprite.Height - 3);
            ballRectangle = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, ballSprite.Width, ballSprite.Height);
        }

        public override void Update(GameTime gameTime)
        {
            ballPosition.X += (ballSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            ballPosition.Y -= (ballSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

            ballRectangle = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, ballSprite.Width, ballSprite.Height);
            

            if (ballPosition.X > maxX || ballPosition.X < 0)
                ballSpeed.X *= -1;
            if (ballPosition.Y < 0)
                ballSpeed.Y *= -1;
            if (ballPosition.Y > maxY)
            {
                breaker.breakerPosition.X = (game.GraphicsDevice.Viewport.Width / 2);
                breaker.breakerPosition.Y = game.GraphicsDevice.Viewport.Height - breaker.BreakerSprite.Height * 2;
                ballPosition.X = (breaker.breakerPosition.X + breaker.BreakerSprite.Width / 2);
                ballPosition.Y = breaker.breakerPosition.Y - ballSprite.Height;
                ballSpeed.Y *= -1;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(ballSprite, ballPosition, Color.White);
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            ballSprite.Dispose();
            base.Dispose(disposing);
        }
    }
}
