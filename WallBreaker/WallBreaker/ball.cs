using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WallBreaker
{
    class ball
    {
        public Texture2D ballSprite;
        public Rectangle ballRectangle;
        public Vector2 ballPosition;
        public Vector2 ballSpeed = new Vector2(170, -170);

        public ball(Vector2 v , Vector2 s , Texture2D pic)
        {
            ballSprite = pic;
            ballPosition = new Vector2(v.X , v.Y);
            ballSpeed = new Vector2(s.X, s.Y);
            ballRectangle = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, ballSprite.Width, ballSprite.Height);
        }
    }
}
