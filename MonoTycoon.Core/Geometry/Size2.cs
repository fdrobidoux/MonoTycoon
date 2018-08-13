using System;
using System.Runtime.Serialization;

namespace MonoTycoon.Core.Geometry
{
    public partial struct Size2
    {
        [DataMember]
        public int Width;
        
        [DataMember]
        public int Height;

        public Size2(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}