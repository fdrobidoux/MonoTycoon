using System;

namespace Microsoft.Xna.Framework
{
    public static class DoubleExtensions
    {
        public static float ToFloat(this double theDouble) => (float) theDouble;
    }
}