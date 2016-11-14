using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WallBreaker
{
    class Breaker : DrawableGameComponent
    {

        public Vector2 breakerPosition;
        public Rectangle breakerRectangle;
        public Texture2D BreakerSprite;
        Game game;
        int maxX;
        int maxY;
        SpriteBatch spriteBatch;
        KeyboardState KeyState;

        public Breaker (Game game) : base (game)
        {
            this.game = game;
            BreakerSprite = game.Content.Load<Texture2D>("breaker");
            breakerPosition = new Vector2(this.game.GraphicsDevice.Viewport.Width / 2, this.game.GraphicsDevice.Viewport.Height - BreakerSprite.Height * 2);
            breakerRectangle = new Rectangle((int)breakerPosition.X, (int)breakerPosition.Y, BreakerSprite.Width, BreakerSprite.Height);
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            maxX = game.GraphicsDevice.Viewport.Width - BreakerSprite.Width;
            maxY = game.GraphicsDevice.Viewport.Height - BreakerSprite.Height;
            breakerRectangle = new Rectangle((int)breakerPosition.X, (int)breakerPosition.Y, BreakerSprite.Width, BreakerSprite.Height);
        }

        public override void Update(GameTime gameTime)
        {
            KeyState = Keyboard.GetState();
            if (KeyState.IsKeyDown(Keys.Right))
                if (breakerPosition.X < maxX)
                    breakerPosition.X += 5;
            if (KeyState.IsKeyDown(Keys.Left))
                if (breakerPosition.X > 0)
                    breakerPosition.X -= 5;
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(BreakerSprite, breakerPosition, Color.White);
            base.Draw(gameTime);
        }
        protected override void Dispose(bool disposing)
        {
            BreakerSprite.Dispose();
            base.Dispose(disposing);
        }
    }
}
