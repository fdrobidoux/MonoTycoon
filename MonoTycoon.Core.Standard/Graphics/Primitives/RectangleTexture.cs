using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon.Core.Common;

namespace MonoTycoon.Core.Graphics.Primitives
{
    public class RectangleTexture : GameHelper
    {
        private static readonly Dictionary<Color, Texture2D> Textures = new Dictionary<Color, Texture2D>();

        private readonly Color _color;

        public RectangleTexture(Color color)
        {
            _color = color;
        }

        public Texture2D Create()
        {
            if (!Textures.ContainsKey(_color))
            {
                var data = new[] { _color };
                var texture = new Texture2D(Game.GraphicsDevice, 1, 1);
                texture.SetData(data);
                Textures[_color] = texture;
            }

            return Textures[_color];
        }
    }
}
