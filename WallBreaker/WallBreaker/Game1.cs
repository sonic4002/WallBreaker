using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WallBreaker
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D BreakerSprite;
        Texture2D Break1Sprite;
        Texture2D Break2Sprite;
        Texture2D Break3Sprite;
        Texture2D HeartSprite;


        Break[,] breaksPos;
        Breaker breaker;
        ball ball;
        Vector2 ballPosition = Vector2.Zero;
        Vector2 ballSpeed = new Vector2(170, -170);
        Vector2 breakerPosition = Vector2.Zero;

        SpriteFont font;

        int maxX;
        int maxY;
        int matrixX;
        int matrixY;
        int score;
        int life;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            int i, j;
            base.Initialize();
            matrixX = 6;
            matrixY = 6;
            score = 0;
            life = 3;

            breaksPos = new Break[matrixX, matrixY];
            maxX = GraphicsDevice.Viewport.Width - ball.ballSprite.Width;
            maxY = GraphicsDevice.Viewport.Height - ball.ballSprite.Height;

            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if ((i == 0) && (j == 0))
                        breaksPos[i, j] = new Break(new Vector2(80, 40));

                    else if ((i > 0) && (j == 0))
                        breaksPos[i, j] = new Break(new Vector2(breaksPos[i-1, j].breakPosition.X, breaksPos[i-1,j].breakPosition.Y+Break1Sprite.Height + 15));
                    else
                        breaksPos[i, j] = new Break(new Vector2(breaksPos[i, j-1].breakPosition.X +Break1Sprite.Width + 5, breaksPos[i, j-1].breakPosition.Y));
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

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BreakerSprite = Content.Load<Texture2D>("breaker");
            Break1Sprite = Content.Load<Texture2D>("break1");
            Break2Sprite = Content.Load<Texture2D>("break2");
            Break3Sprite = Content.Load<Texture2D>("break3");
            HeartSprite = Content.Load<Texture2D>("heart");
            font = Content.Load<SpriteFont>("Master");
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            breaker = new Breaker(this);
            this.Components.Add(breaker);
            Services.AddService(typeof(Breaker), breaker);
            ball = new ball(this);
            this.Components.Add(ball);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            int i, j;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (ball.ballRectangle.Intersects(breaker.breakerRectangle))
                ball.ballSpeed.Y *= -1;
            //if (breaker.breakerRectangle.Intersects(ball.ballRectangle))
            //    ball.ballSpeed.Y *= -1;

            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if (ball.ballRectangle.Intersects(breaksPos[i,j].breakBounds) && (breaksPos[i, j].alive == true))
                    {
                        breaksPos[i, j].alive = false;
                        ball.ballSpeed.Y *= -1;
                        score += breaksPos[i, j].score;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int i, j;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if (breaksPos[i,j].alive == true)
                        spriteBatch.Draw(breaksPos[i, j].breakPic, breaksPos[i, j].breakPosition, Color.White);
                }
            }
            spriteBatch.DrawString(font, "Score : "+score, new Vector2(2, 30), Color.Red);
            spriteBatch.DrawString(font, "Life : " + life, new Vector2(2, 15), Color.Red);
            for (i=0; i<life; i++)
            {
                spriteBatch.Draw(HeartSprite, new Vector2((int)(100 + HeartSprite.Width*i*0.15) , 15), null,Color.White,0, new Vector2(0, 0),(float)0.1, SpriteEffects.None, 0); 
            }
            
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
