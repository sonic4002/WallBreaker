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
        Texture2D BallSprite;
        Texture2D HeartSprite;


        Break[,] breaksPos;
        Breaker breaker;
        ball ball;

        Vector2 ballPosition = Vector2.Zero;
        Vector2 ballSpeed = new Vector2(170, -170);
        Vector2 breakerPosition = Vector2.Zero;

        KeyboardState KeyState;
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

            breaker = new Breaker(new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - BreakerSprite.Height * 2), BreakerSprite);
            ball = new ball(new Vector2(breaker.breakerPosition.X + breaker.BreakerSprite.Width / 2, breaker.breakerPosition.Y - BallSprite.Height + 3), new Vector2(170, -170), BallSprite);

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
            BallSprite = Content.Load<Texture2D>("ball");
            HeartSprite = Content.Load<Texture2D>("heart");
            font = Content.Load<SpriteFont>("Master");


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

            ball.ballPosition.X += (ball.ballSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            ball.ballPosition.Y -= (ball.ballSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

            ball.ballRectangle = new Rectangle((int)ball.ballPosition.X, (int)ball.ballPosition.Y, ball.ballSprite.Width, ball.ballSprite.Height);
            breaker.breakerRectangle = new Rectangle((int)breaker.breakerPosition.X, (int)breaker.breakerPosition.Y, breaker.BreakerSprite.Width, breaker.BreakerSprite.Height);

            if (ball.ballPosition.X > maxX || ball.ballPosition.X < 0)
                ball.ballSpeed.X *= -1;
            if (ball.ballPosition.Y < 0)
                ball.ballSpeed.Y *= -1;
            if (ball.ballPosition.Y > maxY)
            {
                breaker.breakerPosition.X = (GraphicsDevice.Viewport.Width / 2);
                breaker.breakerPosition.Y = GraphicsDevice.Viewport.Height - breaker.BreakerSprite.Height * 2;
                ball.ballPosition.X = (breaker.breakerPosition.X + breaker.BreakerSprite.Width / 2);
                ball.ballPosition.Y = breaker.breakerPosition.Y - ball.ballSprite.Height;
                ball.ballSpeed.Y *= -1;
                life--;
            }

            KeyState = Keyboard.GetState();
            if (KeyState.IsKeyDown(Keys.Right))
                if (breaker.breakerPosition.X + breaker.BreakerSprite.Width < maxX)
                    breaker.breakerPosition.X += 5;
            if (KeyState.IsKeyDown(Keys.Left))
                if (breaker.breakerPosition.X > 0)
                    breaker.breakerPosition.X -= 5;


            if (breaker.breakerRectangle.Intersects(ball.ballRectangle))
                ball.ballSpeed.Y *= -1;

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
            spriteBatch.Draw(ball.ballSprite, ball.ballPosition, Color.White);
            spriteBatch.Draw(breaker.BreakerSprite, breaker.breakerPosition, Color.White);
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
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
