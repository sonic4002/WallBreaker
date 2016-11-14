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
        Texture2D HeartSprite;

        Scene1 scene1;
        Scene2 scene2;
        

        SpriteFont font;
        public static int score { get; set; }
        public static int life { get; set; }

        
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
            base.Initialize();
            scene1.Initialize();
            score = 0;
            life = 3;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);          
            HeartSprite = Content.Load<Texture2D>("heart");
            font = Content.Load<SpriteFont>("Master");
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            scene1 = new Scene1(this);
            scene1.Show();
            scene2 = new Scene2(this);
            scene2.Hide();
            Components.Add(scene1);
            Components.Add(scene2);
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
            if (scene1.EndScene)
            {
                scene1.Hide();
                
                scene2.Initialize();
                scene2.Show();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int i;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

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
