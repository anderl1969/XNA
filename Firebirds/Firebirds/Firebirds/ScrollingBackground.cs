using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Firebirds
{
    class ScrollingBackground
    {
        GraphicsDevice graphics;
        Texture2D texBackground;
        Rectangle recBackgroundPositionAndDimension;
        Rectangle recBackgroundPositionAndDimension1;

        public void LoadContent(ContentManager content, GraphicsDevice graphics)
        {
            this.graphics = graphics;
            texBackground = content.Load<Texture2D>("Graphics\\ground");
            recBackgroundPositionAndDimension.X = 0;
            recBackgroundPositionAndDimension.Y = 0;
            recBackgroundPositionAndDimension.Width = graphics.Viewport.Width;
            recBackgroundPositionAndDimension.Height = graphics.Viewport.Height;
            recBackgroundPositionAndDimension1.X = 0;
            recBackgroundPositionAndDimension1.Y = -graphics.Viewport.Height;
            recBackgroundPositionAndDimension1.Width = graphics.Viewport.Width;
            recBackgroundPositionAndDimension1.Height = graphics.Viewport.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texBackground, recBackgroundPositionAndDimension, Color.White);
            spriteBatch.Draw(texBackground, recBackgroundPositionAndDimension1, Color.White);
        }

        public void Update()
        {
            recBackgroundPositionAndDimension.Y += 2;
            recBackgroundPositionAndDimension1.Y += 2;
            if (recBackgroundPositionAndDimension.Y >= graphics.Viewport.Height)
            {
                recBackgroundPositionAndDimension.Y = -graphics.Viewport.Height;
            }
            if (recBackgroundPositionAndDimension1.Y >= graphics.Viewport.Height)
            {
                recBackgroundPositionAndDimension1.Y = -graphics.Viewport.Height;
            }

        }
    }
}
