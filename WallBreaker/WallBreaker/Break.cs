using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WallBreaker
{
    class Break
    {
        public Rectangle breakBounds;
        public Vector2 breakPosition;
        public bool alive;
        public Texture2D breakPic;
        public int score;

        public Break (Vector2 pos)
        {
            score = 0;
            this.breakPosition = pos;
            this.alive = true;
        }
        public void setRec()
        {
            breakBounds = new Rectangle((int)breakPosition.X, (int)breakPosition.Y, breakPic.Width, breakPic.Height);
        }
    }
}
