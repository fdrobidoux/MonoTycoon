using System;
using Microsoft.Xna.Framework;

namespace MonoTycoon.Core.Geometry
{
    /// <summary>
    /// Represents a transformation.
    /// </summary>
    public partial class Transform2
    {
        public Rotation2 Rotation { get; set; }
        public Size2 Resize { get; set; }
        public Point Offset { get; set; }
    }
}