using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace Firebirds
{
    class Player : CollidingObject
    {
        const int ANIM_PICS_HOR = 6;
        const int ANIM_PICS_VER = 2;

        const int SHOOTYOFFSET = 54 - 64 + 20;
        const int SHOOT1XOFFSET = (256 / 2 - 44) - (64 / 2);
        const int SHOOT2XOFFSET = (256 / 2 + 44) - (64 / 2);
        const int SHOOTRATIONPERSECONDS = 7;

        double ShootRatioTimer = 0;

        public List<Bullet> bulletList = new List<Bullet>();

        ContentManager content;
        GraphicsDevice graphics;
        Texture2D texPlayer;
        Texture2D texShadow;
        Color[,] colArrayPlayer;

        SoundEffect flySound;
        SoundEffectInstance flySoundInstance;

        SoundEffect bulletSound;

        public Rectangle recPlayerPositionAndDimension;
        public Rectangle recAnimationSource;

        float scalePlayer = 0.5f;

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            this.graphics = graphics;

            texPlayer = content.Load<Texture2D>("Graphics\\spitfire2");
            texShadow = content.Load<Texture2D>("Graphics\\spitfire_shadow");
            recPlayerPositionAndDimension.Width = texPlayer.Width / ANIM_PICS_HOR;
            recPlayerPositionAndDimension.Height = texPlayer.Height / ANIM_PICS_VER;
            recPlayerPositionAndDimension.X = graphics.Viewport.Width / 2
                                              - (int)(recPlayerPositionAndDimension.Width / 2 * scalePlayer);
            recPlayerPositionAndDimension.Y = graphics.Viewport.Height
                                              - (int)(recPlayerPositionAndDimension.Height * scalePlayer);

            recAnimationSource.X = 0;
            recAnimationSource.Y = 0;
            recAnimationSource.Width = texPlayer.Width / ANIM_PICS_HOR;
            recAnimationSource.Height = texPlayer.Height / ANIM_PICS_VER;

            flySound = content.Load<SoundEffect>("Sound\\flySound");
            flySoundInstance = flySound.CreateInstance();
            flySoundInstance.IsLooped = true;
            flySoundInstance.Play();

            bulletSound = content.Load<SoundEffect>("Sound\\bulletSound");

            //colArrayPlayer = TextureTo2DArray(texPlayer);
            colArrayPlayer = TextureTo2DArray(texPlayer, ANIM_PICS_HOR, ANIM_PICS_VER, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(texPlayer, recPlayerPositionAndDimension, Color.White);
            //spriteBatch.Draw(texPlayer, recPlayerPositionAndDimension, recAnimationSource, Color.White);
            spriteBatch.Draw(texShadow, new Vector2(recPlayerPositionAndDimension.X - 50, recPlayerPositionAndDimension.Y + 20), recAnimationSource, Color.White, 0f, new Vector2(0, 0), scalePlayer, SpriteEffects.None, 0);
            spriteBatch.Draw(texPlayer, new Vector2(recPlayerPositionAndDimension.X, recPlayerPositionAndDimension.Y), recAnimationSource, Color.White, 0f, new Vector2(0, 0), scalePlayer, SpriteEffects.None, 0);

            foreach (Bullet bullet in bulletList)
            {
                bullet.Draw(spriteBatch);
            }

        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) & recPlayerPositionAndDimension.X > 0)
            {
                recPlayerPositionAndDimension.X -= 5;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right) & recPlayerPositionAndDimension.X + (recPlayerPositionAndDimension.Width * scalePlayer) < graphics.Viewport.Width)
            {
                recPlayerPositionAndDimension.X += 5;

            }

            ShootRatioTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //Sobald der Timer grösser als 1/ die Anzahl der Schüsse pro Sekunde generieren wir 2 neue Schüsse
                if (ShootRatioTimer >= (1.0f / SHOOTRATIONPERSECONDS))
                {
                    Bullet newBullet = new Bullet(recPlayerPositionAndDimension.X + (int)(SHOOT1XOFFSET * scalePlayer), recPlayerPositionAndDimension.Y + (int)(SHOOTYOFFSET * scalePlayer));
                    newBullet.LoadContent(content, graphics);
                    bulletList.Add(newBullet);

                    Bullet newBullet1 = new Bullet(recPlayerPositionAndDimension.X + (int)(SHOOT2XOFFSET * scalePlayer), recPlayerPositionAndDimension.Y + (int)(SHOOTYOFFSET * scalePlayer));
                    newBullet1.LoadContent(content, graphics);
                    bulletList.Add(newBullet1);

                    ShootRatioTimer = 0;

                    //Der Schussound wird abgespielt.
                    bulletSound.Play();
                }
            }

            //Wir Aktualisieren jeden einzelnen Schuss
            foreach (Bullet bullet in bulletList)
            {
                bullet.Update();
            }


            //Wir zeigen das nächste Bild von unserem Player an
            recAnimationSource.X += (texPlayer.Width / ANIM_PICS_HOR);

            //Sobald das letzte Bild der Zeile 1 erreicht ist, springen wir auf die zweite Zeile. 
            if (recAnimationSource.X == texPlayer.Width)
            {
                recAnimationSource.X = 0;
                recAnimationSource.Y += texPlayer.Height / ANIM_PICS_VER;
            }
            //Haben wir das letzte Bild der Zeile 2 erreicht, springen wir auf die Anfangsposition der ersten Zeile zurück
            if (recAnimationSource.Y == texPlayer.Height)
            {
                recAnimationSource.Y = 0;
            }

        }

        public bool CheckCollision(Rectangle recEnemy, Color[,] colArrayEnemy, Matrix matEnemy)
        {
            if (CheckCollision(recEnemy))
            {
                //die BoundingRectangles überlagern sich, also genauer untersuchen
                if (CheckCollision(colArrayEnemy, matEnemy))
                {
                    //Pixel-Kollision
                    return true;
                }
            }
            return false;
        }

        public bool CheckCollision(Rectangle recEnemy)
        {
            if (recPlayerPositionAndDimension.Intersects(recEnemy))
            {
                return true;
            }
            return false;
        }

        public bool CheckCollision(Color[,] colArrayEnemy, Matrix matEnemy)
        {
            Matrix matPlayer = Matrix.CreateScale(scalePlayer) * Matrix.CreateTranslation(this.recPlayerPositionAndDimension.X, this.recPlayerPositionAndDimension.Y, 0);
            Matrix matPlayer2Enemy = matPlayer * Matrix.Invert(matEnemy);

            for (int x_player = 0; x_player < recPlayerPositionAndDimension.Width; x_player++)
            {
                for (int y_player = 0; y_player < recPlayerPositionAndDimension.Height; y_player++)
                {
                    if (colArrayPlayer[x_player, y_player].A > 0)
                    {
                        //Pixel x/y des Player-Sprite ist sichtbar
                        //Transformieren der Player-Koordinate in Enemy-Koordinate
                        Vector2 posPlayer = new Vector2(x_player, y_player);
                        Vector2 posEnemy = Vector2.Transform(posPlayer, matPlayer2Enemy);

                        int x_enemy = (int)posEnemy.X;
                        int y_enemy = (int)posEnemy.Y;

                        //ist das transformierte Pixel überhaupt Bestandteil des Enemy?
                        if ((x_enemy >= 0) && (x_enemy < colArrayEnemy.GetLength(0)))
                        {
                            if ((y_enemy >= 0) && (y_enemy < colArrayEnemy.GetLength(1)))
                            {
                                //Ist das Enemy-Pixel transparent?
                                if (colArrayEnemy[x_enemy, y_enemy].A > 0)
                                {
                                    //Pixel ist solid
                                    //Kollision!!!
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
