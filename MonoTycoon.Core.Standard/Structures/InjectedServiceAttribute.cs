using System;
using System.Collections.Generic;
using System.Text;

namespace MonoTycoon.Structures
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	sealed class InjectedServiceAttribute : Attribute
	{
		// See the attribute guidelines at 
		//  http://go.microsoft.com/fwlink/?LinkId=85236
		readonly string positionalString;

		static InjectedServiceAttribute()
		{

		}

		/// <summary>Defines an injected service as a parameter.</summary>
		public InjectedServiceAttribute()
		{
			
		}
	}
}
