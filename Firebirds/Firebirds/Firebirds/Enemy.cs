using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Firebirds
{
    class Enemy : CollidingObject
    {
        const int ANIM_PICS_HOR = 6;
        const int ANIM_PICS_VER = 2;

        GraphicsDevice graphics;
        public Texture2D texEnemy;

        //public Color[,] colArrayEnemy;
        public Rectangle recEnemyPositionAndDimension;
        public Rectangle recAnimationSource;
        public bool isActive = true;

        public float scaleEnemy = 0.5f;

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            this.graphics = graphics;

            texEnemy = content.Load<Texture2D>("Graphics\\enemy");
            recEnemyPositionAndDimension.Width = texEnemy.Width / ANIM_PICS_HOR;
            recEnemyPositionAndDimension.Height = texEnemy.Height / ANIM_PICS_VER;
            recEnemyPositionAndDimension.X = graphics.Viewport.Width / 2
                                              - (int)(recEnemyPositionAndDimension.Width / 2 * scaleEnemy);
            recEnemyPositionAndDimension.Y = 0
                                              - (int)(recEnemyPositionAndDimension.Height * scaleEnemy);

            recAnimationSource.X = 0;
            recAnimationSource.Y = 0;
            recAnimationSource.Width = texEnemy.Width / ANIM_PICS_HOR;
            recAnimationSource.Height = texEnemy.Height / ANIM_PICS_VER;

            //colArrayEnemy = TextureTo2DArray(texEnemy);
            //colArrayEnemy = TextureTo2DArray(texEnemy, ANIM_PICS_HOR, ANIM_PICS_VER, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive)
            {
                //spriteBatch.Draw(texEnemy, recEnemyPositionAndDimension, Color.White);
                //spriteBatch.Draw(texEnemy, recEnemyPositionAndDimension, recAnimationSource, Color.White);
                spriteBatch.Draw(texEnemy, new Vector2(recEnemyPositionAndDimension.X, recEnemyPositionAndDimension.Y), recAnimationSource, Color.White, 0f, new Vector2(0, 0), scaleEnemy, SpriteEffects.None, 0);
            }
        }

        public void Update()
        {
            if (isActive)
            {
                recEnemyPositionAndDimension.Y += 1;
                //recEnemyPositionAndDimension.X += 4;
                //recEnemyPositionAndDimension.X -= 8;

                //Wir zeigen das nächste Bild von unserem Player an
                recAnimationSource.X += (texEnemy.Width / ANIM_PICS_HOR);

                //Sobald das letzte Bild der Zeile 1 erreicht ist, springen wir auf die zweite Zeile. 
                if (recAnimationSource.X == texEnemy.Width && recAnimationSource.Y != texEnemy.Height / ANIM_PICS_VER)
                {
                    recAnimationSource.X = 0;
                    recAnimationSource.Y = texEnemy.Height / ANIM_PICS_VER;
                }
                //Haben wir das letzte Bild der Zeile 2 erreicht, springen wir auf die Anfangsposition der ersten Zeile zurück
                else if (recAnimationSource.X == texEnemy.Width && recAnimationSource.Y == texEnemy.Height / ANIM_PICS_VER)
                {
                    recAnimationSource.X = 0;
                    recAnimationSource.Y = 0;
                }

                if (recEnemyPositionAndDimension.Y > graphics.Viewport.Height)
                {
                    isActive = false;
                }
            }
        }

        public int getCountHorAnimPics()
        {
            return ANIM_PICS_HOR;
        }

        public int getCountVerAnimPics()
        {
            return ANIM_PICS_VER;
        }
    }
}
