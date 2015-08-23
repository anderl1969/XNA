using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Firebirds
{
    class CollidingObject
    {
        protected Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        public Color[,] TextureTo2DArray(Texture2D texture, int bilder_hor, int bilder_vert, int bildnummer)
        {

            //bildnummer    - Die Position des gesuchten Einzelbildes (beginnt mit 0)

            // breite   - Breite eines Einzelbildes
            int breite = texture.Width / bilder_hor;

            // höhe     - Höhe eines Einzelbildes
            int höhe = texture.Height / bilder_vert;

            //Color Array deiner MultiTextur
            Color[,] gesamt = this.TextureTo2DArray(texture);
            Color[,] einzelbild = new Color[breite, höhe];

            //Die Anzahl der Einzelbilder
            int bilderanzahl = bilder_hor * bilder_vert;

            int offset_vert = bildnummer / bilder_hor;
            int offset_hor = bildnummer % bilder_hor;


            for (int x = 0; x < breite; x++)
            {
                for (int y = 0; y < höhe; y++)
                {
                    einzelbild[x, y] = gesamt[x + (offset_hor * breite), y + (offset_vert * höhe)];
                }
            }
            return einzelbild;
        }
    }
}
