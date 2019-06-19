using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoTycoon.Physics
{
	public delegate Vector2 Offsetify(Vector2 cOffset, Vector2 clocation);

	public class Transform2
	{
		private List<Tuple<object, Offsetify>> offsetCallbacks;

		public static Transform2 Zero = new Transform2(Vector2.Zero, Size2.Zero);

		public Vector2 Location { get; set; }
		public Vector2 Offset { get; set; } = Vector2.Zero;
		public Rotation2 Rotation { get; set; }
		public float Scale { get; set; } = 1f;
		public Size2 Size { get; set; }
		public int ZIndex { get; set; } = 0;

		#region "Constructors"

		public Transform2(Rectangle rectangle)
			: this(new Vector2(rectangle.Location.X, rectangle.Location.Y), new Size2(rectangle.Size.X, rectangle.Size.Y)) { }

		public Transform2(float scale)
			: this(Vector2.Zero, Rotation2.Default, Size2.Zero, scale) { }

		public Transform2(Rotation2 rotation)
			: this(Vector2.Zero, rotation, Size2.Zero, 1) { }

		public Transform2(Vector2 location)
			: this(location, Rotation2.Default, Size2.Zero, 1) { }

		public Transform2(Vector2 location, float scale)
			: this(location, Rotation2.Default, Size2.Zero, scale) { }

		public Transform2(Size2 size)
			: this(Vector2.Zero, Rotation2.Default, size, 1) { }

		public Transform2(Vector2 location, Rotation2 rotation, float scale)
			: this(location, rotation, Size2.Zero, scale) { }

		public Transform2(Vector2 location, Size2 size)
			: this(location, Rotation2.Default, size, 1) { }

		public Transform2(Vector2 location, Size2 size, float scale)
			: this(location, Rotation2.Default, size, scale, 0) { }

		public Transform2(Vector2 location, Rotation2 rotation, Size2 size, float scale)
			: this(location, rotation, size, scale, 0) { }

		public Transform2(Vector2 location, Rotation2 rotation, Size2 size, float scale, int zIndex)
		{
			Location = location;
			Rotation = rotation;
			Size = size;
			Scale = scale;
			ZIndex = zIndex;
			offsetCallbacks = new List<Tuple<object, Offsetify>>();
		}

		#endregion

		#region "Offset Callbacks"

		public void AddOffsetifier(object owner, Offsetify offsetifier)
		{
			offsetCallbacks.Add(new Tuple<object, Offsetify>(owner, offsetifier));
		}

		public void RemoveOffsetifiersBy(object owner)
		{
			offsetCallbacks.RemoveAll(kpv => kpv.Item1.Equals(owner));
		}

		public void ClearOffsetifiers()
		{
			offsetCallbacks.Clear();
		}

		public Vector2 accumulateLocationOffsets()
		{
			Vector2 offset = Location;
			foreach (Offsetify offsetifier in offsetCallbacks.Select(c => c.Item2).ToList<Offsetify>())
			{
				offset += offsetifier(offset, Location);
			}
			return offset;
		}

		#endregion

		public bool Intersects(Point point)
			=> ToRectangle().Contains(point);

		public bool Intersects(Transform2 other)
			=> ToRectangle().Intersects(other.ToRectangle());

		public Transform2 WithSize(Size2 size)
			=> new Transform2(Location, Rotation, size, Scale, ZIndex);

		public Rectangle ToRectangle()
			=> new Rectangle(Location.ToPoint(), (Size * Scale).ToPoint());

		public void DeconstructScaledF(out Vector2 locationF, out Vector2 sizeF)
		{
			locationF = Location;
			sizeF = Size.ToVector2() * Scale;
		}

		public Transform2 WithPadding(int x, int y)
			=> WithPadding(new Size2(x, y));

		public Transform2 WithPadding(Size2 amt)
			=> new Transform2(Location + amt.ToVector2(), Rotation, Size - (amt * 2), Scale, ZIndex);

		public Transform2 WithPadding(Vector2 amt)
			=> new Transform2(Location + amt, Rotation, Size - (amt * 2f).ToSize2(), Scale, ZIndex);

		public override string ToString()
		{
			return $"{Location} {Size} {Rotation} {Scale.ToString("F4")} {ZIndex}";
		}

		#region "Operator methods"

		public static Transform2 operator +(Transform2 t1, Transform2 t2)
		{
			return new Transform2(t1.Location + t2.Location, t1.Rotation + t2.Rotation, t1.Size + t2.Size, t1.Scale * t2.Scale, t1.ZIndex);
		}

		public static Transform2 operator +(Transform2 t1, Vector2 by)
		{
			return new Transform2(t1.Location + by, t1.Rotation, t1.Size, t1.Scale, t1.ZIndex);
		}

		public static Transform2 operator +(Transform2 t1, float scale)
		{
			return new Transform2(t1.Location, t1.Rotation, t1.Size, t1.Scale * scale);
		}

		public static Transform2 operator -(Transform2 t1, Vector2 by)
			=> new Transform2(t1.Location - by, t1.Rotation, t1.Size, t1.Scale, t1.ZIndex);

		public static Transform2 operator -(Vector2 by, Transform2 t1)
			=> new Transform2(by - t1.Location, t1.Rotation, t1.Size, t1.Scale, t1.ZIndex);

		#endregion

		public Transform2 ToScale(float scale)
		{
			return this + new Transform2(Vector2.Zero, Rotation2.Default, Size2.Zero, Scale / scale, ZIndex);
		}

		public Vector2 Center() => (Size.ToVector2() / 2f) + Location;
	}
}
