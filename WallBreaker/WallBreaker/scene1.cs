﻿using Microsoft.Xna.Framework;
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
        private ball ballSprite;
        private Breaker breakerSprite;
        Break[,] breaksPos;
        int matrixX;
        int matrixY;
        Texture2D Break1Sprite;
        Texture2D Break2Sprite;
        Texture2D Break3Sprite;
        int i, j;

        public Scene1 (Game game) : base(game)
        {
            this.game = game;
            spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
            breakerSprite = new Breaker(game);
            ballSprite = new ball(game);
            ballSprite.breaker = breakerSprite;
            SceneComponents.Add(ballSprite);
            SceneComponents.Add(breakerSprite);
            EndScene = false;
            Break1Sprite = game.Content.Load<Texture2D>("break1");
            Break2Sprite = game.Content.Load<Texture2D>("break2");
            Break3Sprite = game.Content.Load<Texture2D>("break3");
            matrixX = 2;
            matrixY = 2;
            breaksPos = new Break[matrixX, matrixY];

        }


        public override void Initialize()
        {
            ballSprite.Initialize();
            breakerSprite.Initialize();
            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if ((i == 0) && (j == 0))
                        breaksPos[i, j] = new Break(new Vector2(80, 60));

                    else if ((i > 0) && (j == 0))
                        breaksPos[i, j] = new Break(new Vector2(breaksPos[i - 1, j].breakPosition.X, breaksPos[i - 1, j].breakPosition.Y + Break1Sprite.Height + 15));
                    else
                        breaksPos[i, j] = new Break(new Vector2(breaksPos[i, j - 1].breakPosition.X + Break1Sprite.Width + 5, breaksPos[i, j - 1].breakPosition.Y));
                    if (i % 3 == 0)
                    {
                        breaksPos[i, j].breakPic = Break1Sprite;
                        breaksPos[i, j].score = 10;
                    }
                    if (i % 3 == 1)
                    {
                        breaksPos[i, j].breakPic = Break2Sprite;
                        breaksPos[i, j].score = 20;
                    }
                    if (i % 3 == 2)
                    {
                        breaksPos[i, j].breakPic = Break3Sprite;
                        breaksPos[i, j].score = 30;
                    }
                    breaksPos[i, j].setRec();
                }
            }

            base.Initialize();
        }


        protected override void Dispose(bool disposing)
        {
            ballSprite.Dispose();
            breakerSprite.Dispose();
            base.Dispose(disposing);
        }

        public override void Update(GameTime gameTime)
        {   

            if (ballSprite.ballRectangle.Intersects(breakerSprite.breakerRectangle))
                ballSprite.ballSpeed.Y *= -1;

            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    EndScene = false;
                    if (ballSprite.ballRectangle.Intersects(breaksPos[i, j].breakBounds) && (breaksPos[i, j].alive == true))
                    {
                        EndScene = true;
                        breaksPos[i, j].alive = false;
                        ballSprite.ballSpeed.Y *= -1;
                        Game1.score += breaksPos[i, j].score;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            EndScene = true;
            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if (breaksPos[i, j].alive == true)
                        spriteBatch.Draw(breaksPos[i, j].breakPic, breaksPos[i, j].breakPosition, Color.White);
                }
            }
            base.Draw(gameTime);
        }



           
    }
}
