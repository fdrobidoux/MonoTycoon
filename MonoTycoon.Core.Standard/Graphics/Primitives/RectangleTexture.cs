using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTycoon;

namespace MonoTycoon.Graphics.Primitives
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
            if (!Textures.TryGetValue(_color, out Texture2D texture))
            {
                texture = new Texture2D(Game.GraphicsDevice, 1, 1);
                texture.SetData(new[] { _color });
                Textures[_color] = texture;
            }
            return texture;
        }
    }
}
