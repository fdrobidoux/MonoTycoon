using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoTycoon.Core.Screens.Components
{
	public class FilteredComponentList<T> : GameComponentCollection where T : GameComponent
	{
		public FilteredComponentList()
		{
		}
	}
}
