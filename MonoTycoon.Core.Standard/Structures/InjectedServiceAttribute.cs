using System;
using System.Collections.Generic;
using System.Text;
using XNA = Microsoft.Xna.Framework;

namespace MonoTycoon.Structures
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class InjectedServiceAttribute : Attribute
	{
		public bool Optional {get; set; }
	}
}
