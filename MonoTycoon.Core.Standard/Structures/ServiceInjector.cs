using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace MonoTycoon.Structures
{
	public static class ServiceInjector
	{
		/// <summary>
		/// Contains all the services. Necessary because there is no way to know the list of services any other way.
		/// </summary>
		private static readonly List<object> services = new List<object>();
		private static object Mutex = new object();

		public static Game Game { private get; set; }

		public static void AddComponent(IGameComponent gameComp)
		{
			AddComponent(gameComp, false);
		}

		public static void AddComponent(IGameComponent gameComp, bool addServices = false)
		{
			lock (Mutex)
			{
				if (!addServices) InjectServices(gameComp);
				Game.Components.Add(gameComp);
				if (addServices) AddService(gameComp);
#if DEBUG
				Console.WriteLine($"{gameComp.GetType().Name} loaded");
#endif
			}
		}

		public static void AddService(object serviceObj)
		{
			foreach (Type type in serviceObj.GetType().GetInterfaces())
			{
				Game.Services.AddService(type, serviceObj);
			}
			services.Add(serviceObj);
		}

		public static void InjectServices(object componentOrService)
		{
			Type type = componentOrService.GetType();
			do
			{
				foreach (PropertyInfo propInfo in type.GetSettableProperties())
				{
					InjectedServiceAttribute injectionAttr = propInfo.GetFirstAttribute<InjectedServiceAttribute>();
					Type propertyType = propInfo.PropertyType;
					object obj = ServiceInjector.Game == null ? null : Game.Services.GetService(propertyType);
					if (obj == null)
					{
						if (!injectionAttr.Optional)
							throw new Exception($"Missing service: {type} for property {propertyType.Name}.");
					}
					else
					{
						propInfo.GetSetMethod(true).Invoke(componentOrService, new[] { obj });
					}
				}
			}
			while ((type = type.BaseType) != typeof(object));
		}

		public static void InjectServices(Type staticType)
		{
			
		}

		public static Type[] GetTypesThatUseServiceInjection()
		{
			var types = new List<Type>();

			// Get the assemblies.
			var assembly = Assembly.GetCallingAssembly().DefinedTypes;

			return types.ToArray();
		}
	}
}
