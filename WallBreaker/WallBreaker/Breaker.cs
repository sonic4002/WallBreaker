using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WallBreaker
{
    class Breaker
    {

        public Vector2 breakerPosition;
        public Rectangle breakerRectangle;
        public Texture2D BreakerSprite;

        public Breaker (Vector2 v , Texture2D pic)
        {
            breakerPosition = new Vector2 (v.X,v.Y);
            BreakerSprite = pic;
            breakerRectangle = new Rectangle((int)breakerPosition.X, (int)breakerPosition.Y, BreakerSprite.Width, BreakerSprite.Height);
        }
    }
}
