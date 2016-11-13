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


        Break[,] breaksPos;

        Vector2[,] breaksPositions ;
        Rectangle ballRectangle;
        Rectangle breakerRectangle;

        Vector2 ballPosition = Vector2.Zero;
        Vector2 ballSpeed = new Vector2(170, 170);
        Vector2 breakerPosition = Vector2.Zero;

        KeyboardState KeyState;
        SpriteFont font;

        int maxX;
        int maxY;
        int matrixX;
        int matrixY;
        int score;

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

            breaksPos = new Break[matrixX, matrixY];
            breaksPositions = new Vector2[matrixX, matrixY];

            breakerPosition.X = (GraphicsDevice.Viewport.Width/2) ;
            breakerPosition.Y = GraphicsDevice.Viewport.Height - BreakerSprite.Height*2;
            ballPosition.X = (breakerPosition.X + BreakerSprite.Width / 2);
            ballPosition.Y = breakerPosition.Y - BallSprite.Height+2;

            maxX = GraphicsDevice.Viewport.Width - BallSprite.Width;
            maxY = GraphicsDevice.Viewport.Height - BallSprite.Height;


            ballRectangle = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, BallSprite.Width, BallSprite.Height);
            breakerRectangle = new Rectangle((int)breakerPosition.X,(int)breakerPosition.Y,BreakerSprite.Width,BreakerSprite.Height);

            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if ((i == 0) && (j == 0))
                        breaksPos[i, j] = new Break(new Vector2(80, 20));

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

            ballPosition.X += (ballSpeed.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
            ballPosition.Y -= (ballSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);

            ballRectangle = new Rectangle((int)ballPosition.X, (int)ballPosition.Y, BallSprite.Width, BallSprite.Height);
            breakerRectangle = new Rectangle((int)breakerPosition.X, (int)breakerPosition.Y, BreakerSprite.Width, BreakerSprite.Height);

            if (ballPosition.X > maxX || ballPosition.X < 0)
                ballSpeed.X *= -1;
            if (ballPosition.Y < 0)
                ballSpeed.Y *= -1;
            if (ballPosition.Y > maxY)
            {
                breakerPosition.X = (GraphicsDevice.Viewport.Width / 2);
                breakerPosition.Y = GraphicsDevice.Viewport.Height - BreakerSprite.Height * 2;
                ballPosition.X = (breakerPosition.X + BreakerSprite.Width / 2);
                ballPosition.Y = breakerPosition.Y - BallSprite.Height;
                ballSpeed.Y *= -1;
            }

            KeyState = Keyboard.GetState();
            if (KeyState.IsKeyDown(Keys.Right))
                if (breakerPosition.X + BreakerSprite.Width < maxX)
                    breakerPosition.X += 5;
            if (KeyState.IsKeyDown(Keys.Left))
                if (breakerPosition.X > 0)
                    breakerPosition.X -= 5;


            if (breakerRectangle.Intersects(ballRectangle))
                ballSpeed.Y *= -1;

            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if (ballRectangle.Intersects(breaksPos[i,j].breakBounds) && (breaksPos[i, j].alive == true))
                    {
                        breaksPos[i, j].alive = false;
                        ballSpeed.Y *= -1;
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
            spriteBatch.Draw(BallSprite, ballPosition, Color.White);
            spriteBatch.Draw(BreakerSprite, breakerPosition, Color.White);
            for (i = 0; i < matrixX; i++)
            {
                for (j = 0; j < matrixY; j++)
                {
                    if (breaksPos[i,j].alive == true)
                        spriteBatch.Draw(breaksPos[i, j].breakPic, breaksPos[i, j].breakPosition, Color.White);
                }
            }
            spriteBatch.DrawString(font, "Score : "+score, new Vector2(2, 45), Color.Red);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
