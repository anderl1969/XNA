using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Firebirds
{
    class Bullet
    {
        const int ANIM_PICS_HOR = 12;
        const int ANIM_PICS_VER = 1;

        public Texture2D texBullet;
        public Rectangle recBulletPositionAndDimension;
        public Rectangle recAnimationSource;
        public bool IsActive = true;

        float scaleBullet = 0.4f;

        public Bullet(int iPositionX, int iPositionY)
        {
            recBulletPositionAndDimension.X = iPositionX;
            recBulletPositionAndDimension.Y = iPositionY;
        }

        public void LoadContent(ContentManager Content, GraphicsDevice graphics)
        {
            texBullet = Content.Load<Texture2D>("Graphics\\bullet");
            recBulletPositionAndDimension.Width = texBullet.Width / ANIM_PICS_HOR;
            recBulletPositionAndDimension.Height = texBullet.Height / ANIM_PICS_VER;

            ///horizontale Position der Bullets justieren
            recBulletPositionAndDimension.X -= (int)(recBulletPositionAndDimension.Width / 2 * scaleBullet);

            recAnimationSource.X = 0;
            recAnimationSource.Y = 0;
            recAnimationSource.Width = texBullet.Width / ANIM_PICS_HOR;
            recAnimationSource.Height = texBullet.Height / ANIM_PICS_VER;
        }

        public void Update()
        {
            if (!IsActive)
                return;

            recBulletPositionAndDimension.Y -= 6;
            recAnimationSource.X += (texBullet.Width / ANIM_PICS_HOR);
            //if (recAnimationSource.X == texBullet.Width && recAnimationSource.Y != texBullet.Height / ANIM_PICS_VER)
            if (recAnimationSource.X == texBullet.Width)
            {
                recAnimationSource.X = 0;
                recAnimationSource.Y += texBullet.Height / ANIM_PICS_VER;
            }
            //Haben wir das letzte Bild der Zeile 2 erreicht, springen wir auf die Anfangsposition der ersten Zeile zurück
            //else if (recAnimationSource.X == texBullet.Width && recAnimationSource.Y == texBullet.Height / ANIM_PICS_VER)
            if (recAnimationSource.Y == texBullet.Height)
            {
                recAnimationSource.Y = 0;
            }

            //Sobald der Schuss auserhalb des Bildschirms ist wollen wir diesen löschen
            if (recBulletPositionAndDimension.Y + texBullet.Height < 0)
                IsActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                //spriteBatch.Draw(texBullet, recBulletPositionAndDimension, recAnimationSource, Color.White);
                spriteBatch.Draw(texBullet, new Vector2(recBulletPositionAndDimension.X, recBulletPositionAndDimension.Y), recAnimationSource, Color.White, 0f, new Vector2(0, 0), scaleBullet, SpriteEffects.None, 0);
            }
        }
    }
}
