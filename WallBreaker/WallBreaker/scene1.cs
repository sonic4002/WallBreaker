using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WallBreaker
{
    class Scene1 : Scene
    {

        private SpriteBatch spriteBatch;
        public bool EndScene;
        private Game game;
         

        public Scene1 (Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }
    }
}
