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

namespace Firebirds
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player = new Player();
        List<Enemy> enemyList = new List<Enemy>();
        List<Enemy> enemy2DeleteList = new List<Enemy>();
        List<Bullet> bullet2DeleteList = new List<Bullet>();

        Color[,] colorArrayEnemy;

        ScrollingBackground scrollingBackground = new ScrollingBackground();

        SoundEffect soundBackground;

        double timer = 0.0;
        Random randomGenerator = new Random();

///        private FPScounter fpsCounter;


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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //dummy Enemy laden um Color-Array zu erstellen
            Enemy dummyEnemy = new Enemy();
            dummyEnemy.LoadContent(Content, GraphicsDevice);
            colorArrayEnemy = dummyEnemy.TextureTo2DArray(dummyEnemy.texEnemy, dummyEnemy.getCountHorAnimPics(), dummyEnemy.getCountVerAnimPics(), 0);

            scrollingBackground.LoadContent(Content, GraphicsDevice);
            soundBackground = Content.Load<SoundEffect>("Sound\\Fire_Birds");
            soundBackground.Play();
            player.LoadContent(Content, GraphicsDevice);
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();


            // TODO: Add your update logic here
            timer += gameTime.ElapsedGameTime.TotalSeconds;

            player.Update(gameTime);

            scrollingBackground.Update();

            if (timer > 2.0)
            {
                timer = 0.0;
                Enemy newEnemy = new Enemy();
                newEnemy.LoadContent(Content, GraphicsDevice);
                newEnemy.recEnemyPositionAndDimension.X = randomGenerator.Next(0, GraphicsDevice.Viewport.Width - newEnemy.recEnemyPositionAndDimension.Width);
                enemyList.Add(newEnemy);
            }

            Matrix matEnemy;
            enemy2DeleteList.Clear();
            bullet2DeleteList.Clear();

            foreach (Enemy enemy in enemyList)
            {
                if (enemy.isActive)
                {
                    enemy.Update();
                    matEnemy = Matrix.CreateScale(enemy.scaleEnemy) * Matrix.CreateTranslation(enemy.recEnemyPositionAndDimension.X, enemy.recEnemyPositionAndDimension.Y, 0);
                    if (player.CheckCollision(enemy.recEnemyPositionAndDimension, colorArrayEnemy, matEnemy))
                    //if (player.CheckCollision(enemy.recEnemyPositionAndDimension, enemy.colArrayEnemy, matEnemy))
                    //if (player.CheckCollision(enemy.colArrayEnemy, matEnemy))
                    //if (player.CheckCollision(enemy.recEnemyPositionAndDimension))
                    {
                        Exit();
                    }

                    foreach (Bullet bullet in player.bulletList)
                    {
                        if (!bullet.IsActive)
                        {
                            bullet2DeleteList.Add(bullet);
                        }
                        else
                        {
                            //Prüfen ob Kugel was getroffen hat
                        }
                    }

                }
                else
                {
                    enemy2DeleteList.Add(enemy);
                }
            }

            foreach (Enemy enemy in enemy2DeleteList)
            {
                enemyList.Remove(enemy);
            }
            foreach (Bullet bullet in bullet2DeleteList)
            {
                player.bulletList.Remove(bullet);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            scrollingBackground.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
