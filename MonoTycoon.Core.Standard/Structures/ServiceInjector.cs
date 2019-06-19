using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTycoon.Structures
{
	public static class ServiceInjector
	{
		private static List<object> services = new List<object>();
		
		public static Game Game { private get; set; }

		public static void AddComponent(IGameComponent component)
		{
			Game.Components.Add(component);
		}

		public static void AddService(object serviceObj)
		{
			if (serviceObj is IGameComponent gameComp)
				Game.Components.Add(gameComp);
		}

	}
}
