using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MonoTycoon;
using MonoTycoon.Extensions;

namespace System.Reflection
{
	public static class ReflectionExtensions
	{
		public const BindingFlags PUBLIC_INSTANCE_MEMBERS = BindingFlags.Instance | BindingFlags.Public;

		private static readonly Type[] EmptyTypes;

		static readonly Dictionary<Tuple<Type, Type>, Attribute> typeAttributeCache;
		static readonly Dictionary<Tuple<PropertyInfo, Type>, Attribute> propertyAttributeCache;
		static readonly Dictionary<Tuple<FieldInfo, Type>, Attribute> fieldAttributeCache;
		static readonly Dictionary<Type, MemberInfo[]> propertyCache;
		static readonly Dictionary<Type, MemberInfo[]> serviceCache;
		static readonly Dictionary<MethodInfo, DynamicMethodDelegate> methodCache;
		static readonly Dictionary<Type, DynamicMethodDelegate> constructorCache;

		static ReflectionExtensions()
		{
			EmptyTypes = new Type[0];
			typeAttributeCache = new Dictionary<Tuple<Type, Type>, Attribute>();
			propertyAttributeCache = new Dictionary<Tuple<PropertyInfo, Type>, Attribute>();
			fieldAttributeCache = new Dictionary<Tuple<FieldInfo, Type>, Attribute>();
			propertyCache = new Dictionary<Type, MemberInfo[]>();
			serviceCache = new Dictionary<Type, MemberInfo[]>();
			methodCache = new Dictionary<MethodInfo, DynamicMethodDelegate>();
			constructorCache = new Dictionary<Type, DynamicMethodDelegate>();
		}

		public static void ClearMemberInfoCache(ClearFlag f)
		{
			if (f.HasFlag(ClearFlag.Properties)) propertyAttributeCache.Clear();
			if (f.HasFlag(ClearFlag.FieldAttributes)) fieldAttributeCache.Clear();
			if (f.HasFlag(ClearFlag.Constructors)) constructorCache.Clear();
			if (f.HasFlag(ClearFlag.Methods)) methodCache.Clear();
			if (f.HasFlag(ClearFlag.Properties)) propertyCache.Clear();
			if (f.HasFlag(ClearFlag.PropertyAttributes)) propertyAttributeCache.Clear();
			if (f.HasFlag(ClearFlag.Services)) serviceCache.Clear();
			if (f.HasFlag(ClearFlag.TypeAttributes)) typeAttributeCache.Clear();
		}

		[Flags]
		public enum ClearFlag : ushort
		{
			TypeAttributes = 1,
			PropertyAttributes = 2,
			FieldAttributes = 4,
			Properties = 8,
			Services = 16,
			Methods = 32,
			Constructors = 64,
		}

		public static void ClearMemberInfoCache()
		{
			propertyAttributeCache.Clear();
			fieldAttributeCache.Clear();
			constructorCache.Clear();
			methodCache.Clear();
		}

		public static T GetFirstAttribute<T>(this Type type) where T : Attribute, new()
		{
			Type typeFromHandle = typeof(T);
			Tuple<Type, Type> key = new Tuple<Type, Type>(type, typeFromHandle);
			Attribute value;
			lock (typeAttributeCache)
			{
				if (!typeAttributeCache.TryGetValue(key, out value))
				{
					typeAttributeCache.Add(key, value = (T)type.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault());
				}
			}
			return value as T;
		}

		public static T GetFirstAttribute<T>(this PropertyInfo propInfo)
				where T : Attribute, new()
		{
			Type typeFromHandle = typeof(T);
			Tuple<PropertyInfo, Type> key = new Tuple<PropertyInfo, Type>(propInfo, typeFromHandle);
			Attribute value;
			lock (propertyAttributeCache)
			{
				if (!propertyAttributeCache.TryGetValue(key, out value))
				{
					propertyAttributeCache.Add(key, value = (T)propInfo.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault());
				}
			}
			return value as T;
		}

		public static T GetFirstAttribute<T>(this FieldInfo fieldInfo)
				where T : Attribute, new()
		{
			var typeFromHandle = typeof(T);
			var key = new Tuple<FieldInfo, Type>(fieldInfo, typeFromHandle);
			Attribute value;

			lock (fieldAttributeCache)
			{
				if (!fieldAttributeCache.TryGetValue(key, out value))
				{
					fieldAttributeCache.Add(key, value = (T)fieldInfo.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault());
				}
			}
			return value as T;
		}

		public static T GetFirstAttribute<T>(this MemberInfo memberInfo) where T : Attribute, new()
		{
			if (memberInfo is PropertyInfo propertyInfo)
			{
				return GetFirstAttribute<T>(propertyInfo);
			}
			return GetFirstAttribute<T>(memberInfo as FieldInfo);
		}

		public static MemberInfo[] GetSerializableMembers(this Type type)
		{
			lock (propertyCache)
			{
				if (propertyCache.TryGetValue(type, out MemberInfo[] value))
				{
					return value;
				}
				value = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(delegate (PropertyInfo p)
				{
					if (p.GetGetMethod() != null && p.GetSetMethod() != null)
					{
						return p.GetGetMethod().GetParameters().Length == 0;
					}
					return false;
				}).Cast<MemberInfo>()
					.Union(type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Cast<MemberInfo>())
					.ToArray();
				propertyCache.Add(type, value);
				return value;
			}
		}

		public static MemberInfo[] GetSettableProperties(this Type type)
		{
			lock (serviceCache)
			{
				if (serviceCache.TryGetValue(type, out MemberInfo[] value))
				{
					return value;
				}
				value = (from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
						 where p.GetSetMethod(nonPublic: true) != null
						 select p).ToArray();
				serviceCache.Add(type, value);
				return value;
			}
		}

		public static Type GetMemberType(this MemberInfo member)
		{
			if (member is PropertyInfo)
			{
				return (member as PropertyInfo).PropertyType;
			}
			if (member is FieldInfo)
			{
				return (member as FieldInfo).FieldType;
			}
			throw new NotImplementedException();
		}

		public static bool IsGenericSet(this Type type)
		{
			return type.GetInterfaces().Any(delegate (Type i)
			{
				if (i.IsGenericType)
				{
					return i.GetGenericTypeDefinition() == typeof(ISet<>);
				}
				return false;
			});
		}

		public static bool IsGenericList(this Type type)
		{
			return type.GetInterfaces().Any(delegate (Type i)
			{
				if (i.IsGenericType)
				{
					return i.GetGenericTypeDefinition() == typeof(IList<>);
				}
				return false;
			});
		}

		public static bool IsGenericCollection(this Type type)
		{
			return type.GetInterfaces().Any(delegate (Type i)
			{
				if (i.IsGenericType)
				{
					return i.GetGenericTypeDefinition() == typeof(ICollection<>);
				}
				return false;
			});
		}

		public static bool IsGenericDictionary(this Type type)
		{
			return type.GetInterfaces().Any(
				delegate (Type i)
				{
					if (i.IsGenericType)
					{
						return i.GetGenericTypeDefinition() == typeof(IDictionary<,>);
					}
					return false;
				});
		}

		public static bool IsNullable(this Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(Nullable<>);
			}
			return false;
		}

		public static DynamicMethodDelegate CreateDelegate(MethodBase method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			int num = parameters.Length;
			Type[] parameterTypes = new Type[2]
			{
				typeof(object),
				typeof(object[])
			};
			DynamicMethod dynamicMethod = new DynamicMethod("", typeof(object), parameterTypes, typeof(ReflectionExtensions).Module, skipVisibility: true);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			Label label = iLGenerator.DefineLabel();
			iLGenerator.Emit(OpCodes.Ldarg_1);
			iLGenerator.Emit(OpCodes.Ldlen);
			iLGenerator.Emit(OpCodes.Ldc_I4, num);
			iLGenerator.Emit(OpCodes.Beq, label);
			iLGenerator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(Type.EmptyTypes));
			iLGenerator.Emit(OpCodes.Throw);
			iLGenerator.MarkLabel(label);
			if (!method.IsStatic && !method.IsConstructor)
			{
				iLGenerator.Emit(OpCodes.Ldarg_0);
				if (method.DeclaringType.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Unbox, method.DeclaringType);
				}
			}
			for (int i = 0; i < num; i++)
			{
				iLGenerator.Emit(OpCodes.Ldarg_1);
				iLGenerator.Emit(OpCodes.Ldc_I4, i);
				iLGenerator.Emit(OpCodes.Ldelem_Ref);
				Type parameterType = parameters[i].ParameterType;
				if (parameterType.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Unbox_Any, parameterType);
				}
			}
			if (method.IsConstructor)
			{
				iLGenerator.Emit(OpCodes.Newobj, method as ConstructorInfo);
			}
			else if (method.IsFinal || !method.IsVirtual)
			{
				iLGenerator.Emit(OpCodes.Call, method as MethodInfo);
			}
			else
			{
				iLGenerator.Emit(OpCodes.Callvirt, method as MethodInfo);
			}
			Type type = method.IsConstructor ? method.DeclaringType : (method as MethodInfo).ReturnType;
			if (type != typeof(void))
			{
				if (type.IsValueType)
				{
					iLGenerator.Emit(OpCodes.Box, type);
				}
			}
			else
			{
				iLGenerator.Emit(OpCodes.Ldnull);
			}
			iLGenerator.Emit(OpCodes.Ret);
			return (DynamicMethodDelegate)dynamicMethod.CreateDelegate(typeof(DynamicMethodDelegate));
		}

		public static object Instantiate(Type type)
		{
			if (type.IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			if (type.IsArray)
			{
				return Array.CreateInstance(type.GetElementType(), 0);
			}
			DynamicMethodDelegate value;
			lock (constructorCache)
			{
				if (!constructorCache.TryGetValue(type, out value))
				{
					value = CreateDelegate(type.GetConstructor(EmptyTypes));
					constructorCache.Add(type, value);
				}
			}
			return value(null);
		}

		public static DynamicMethodDelegate GetDelegate(MethodInfo info)
		{
			lock (methodCache)
			{
				if (methodCache.TryGetValue(info, out DynamicMethodDelegate value))
				{
					return value;
				}
				value = CreateDelegate(info);
				methodCache.Add(info, value);
				return value;
			}
		}

		public static object InvokeMethod(MethodInfo info, object targetInstance, params object[] arguments)
		{
			return GetDelegate(info)(targetInstance, arguments);
		}

		public static object GetValue(PropertyInfo member, object instance)
		{
			return InvokeMethod(member.GetGetMethod(nonPublic: true), instance);
		}

		public static object GetValue(MemberInfo member, object instance)
		{
			if (member is PropertyInfo)
			{
				return GetValue(member as PropertyInfo, instance);
			}
			if (member is FieldInfo)
			{
				return (member as FieldInfo).GetValue(instance);
			}
			throw new NotImplementedException();
		}

		public static void SetValue(PropertyInfo member, object instance, object value)
		{
			InvokeMethod(member.GetSetMethod(nonPublic: true), instance, value);
		}

		public static void SetValue(MemberInfo member, object instance, object value)
		{
			if (member is PropertyInfo)
			{
				SetValue(member as PropertyInfo, instance, value);
			}
			else
			{
				if (!(member is FieldInfo fieldInfo))
					throw new NotImplementedException();

				fieldInfo.SetValue(instance, value);
			}
		}

		public static string GetShortAssemblyQualifiedName<T>()
						  => GetShortAssemblyQualifiedName(typeof(T));

		public static string GetShortAssemblyQualifiedName(Type type)
						  => GetShortAssemblyQualifiedName(type.AssemblyQualifiedName);

		public static string GetShortAssemblyQualifiedName(string assemblyQName)
		{
			while (assemblyQName.Contains(", Version"))
			{
				int num = assemblyQName.IndexOf(", Version");
				int num2 = assemblyQName.IndexOf("],", num);

				if (num2 == -1)
					num2 = assemblyQName.Length;

				if (assemblyQName[num2 - 1] == ']')
					num2--;

				assemblyQName = assemblyQName.Substring(0, num) + assemblyQName.Substring(num2);
			}
			return assemblyQName;
		}
	}
}
