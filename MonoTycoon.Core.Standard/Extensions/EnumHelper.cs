using System;
using System.Collections.Generic;

namespace MonoTycoon.Extensions
{
	public static class EnumHelper
	{
		private static IDictionary<Type, Enum> _allFlagsCache;

		static EnumHelper()
		{
			_allFlagsCache = new Dictionary<Type, Enum>();
		}

		public static T AllFlags<T>(this T @this) where T : Enum
		{
			return AllFlags<T>();
		}

		public static T AllFlags<T>() where T : Enum
		{
			Type ofT = typeof(T);
			Type underT = ofT.GetEnumUnderlyingType();

			if (!_allFlagsCache.TryGetValue(ofT, out Enum value))
			{
				Array values = Enum.GetValues(ofT);

				if (values is int[] intVals)
					value = (T)Enum.ToObject(ofT, SumBox(intVals, (x, y) => x + y));
				else if (values is uint[] uintVals)
					value = (T)Enum.ToObject(ofT, SumBox(uintVals, (x, y) => x + y));
				else if (values is short[] int16Vals)
					value = (T)Enum.ToObject(ofT, SumBox(int16Vals, (x, y) => x += y));
				else if (values is ushort[] uint16Vals)
					value = (T)Enum.ToObject(ofT, SumBox(uint16Vals, (x, y) => x += y));
				else if (values is long[] int64Vals)
					value = (T)Enum.ToObject(ofT, SumBox(int64Vals, (x, y) => x + y));
				else if (values is ulong[] uint64Vals)
					value = (T)Enum.ToObject(ofT, SumBox(uint64Vals, (x, y) => x + y));
				else if (values is byte[] byteVals)
					value = (T)Enum.ToObject(ofT, SumBox(byteVals, (x, y) => x += y));
				else if (values is sbyte[] sbyteVals)
					value = (T)Enum.ToObject(ofT, SumBox(sbyteVals, (x, y) => x += y));
				else
					throw new ArgumentOutOfRangeException($"Couldn't find a value to assign for all the flags in {typeof(T).FullName}");
				
				_allFlagsCache.Add(ofT, value);
			}

			return (T)value;
		}

		private static bool Box<T>(object value, Func<T, bool> op)
			=> op((T)value);
		private static bool Box<T>(object value, object flags, Func<T, T, bool> op)
			=> op((T)value, (T)flags);

		private static T SumBox<T>(T[] array, Func<T, T, T> fn)
		{
			T result = default;
			foreach (T num in array)
				result = fn(num, result);
			return result;
		}

		/// <summary>
		/// TODO: Clear method.
		/// </summary>
		public static T Clear<T>(this Enum flags, Enum toClear) where T : struct
		{
			throw new NotImplementedException();
		}

		private static void _validateIsEnum(Type type)
		{
			if (!type.IsEnum)
				throw new ArgumentException("The type parameter T must be an enum type.");
		}
	}
}
