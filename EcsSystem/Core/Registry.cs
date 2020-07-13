using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EcsSystem.Core {
	public static class Registry {
		public static int Length => Classes.Count();
		
		private const string ClassNamespace = "Core.Classes";
		private const string ComponentsNamespace = "Core.Components";
		
		private static readonly Dictionary<Type, AbstractClass> Classes;
		private static readonly Dictionary<Type, AbstractComponent> Components;

		private static readonly Dictionary<uint, Type> HashSets;
		
		static Registry() {
			Classes = new Dictionary<Type, AbstractClass>();
			Components = new Dictionary<Type, AbstractComponent>();
			HashSets = new Dictionary<uint, Type>();
		}

		public static void RegisterAll() {
			Assembly asm = Assembly.GetExecutingAssembly();

			IEnumerable<Type> classTypes = asm.GetTypes().Where(type => type.Namespace.EndsWith(ClassNamespace));
			IEnumerable<Type> componentTypes = asm.GetTypes().Where(type => type.Namespace.EndsWith(ComponentsNamespace));

			using (var enumerator = componentTypes.GetEnumerator()) {
				while (enumerator.MoveNext()) {
					var value = new AbstractComponent(enumerator.Current);
					Components.Add(enumerator.Current, value);
					HashSets.Add(value.HashCode, enumerator.Current);
					Console.WriteLine($"Registry::Components\t::Add({value.ComponentType.Name}:{value.HashCode})");
				}
			}

			using (var enumerator = classTypes.GetEnumerator()) {
				while (enumerator.MoveNext()) {
					var value = new AbstractClass(enumerator.Current);
					Classes.Add(enumerator.Current, value);
					HashSets.Add(value.HashCode, enumerator.Current);
					Console.WriteLine($"Registry::Classes\t::Add({value.ClassType.Name}({string.Join(", ", value.Components)}):{value.HashCode})");
				}
			}
		}

		/// <summary>
		/// Finds an archetype that contains specific components
		/// </summary>
		public static AbstractClass DirectClassSearch(uint[] components) {
			foreach (var pair in Classes) {
				int count = 0;
				for (int i = 0; i < pair.Value.Components.Length; i++) {
					for (int j = 0; j < components.Length; j++) {
						if (pair.Value.Components[i] == components[j]) {
							count++;
						}
					}
				}

				if (count == pair.Value.Components.Length) {
					return pair.Value;
				}
			}

			return null;
		}

		private static uint[] ExtractComponents<T>() {
			var instance = Activator.CreateInstance<T>();

			uint[] ids;
			if (instance is ITuple _) {
				Type[] types = instance
					.GetType()
					.GetFields()
					.Select(f => f.FieldType)
					.ToArray();
				
				ids = new uint[types.Length];
				for (int i = 0; i < ids.Length; i++) {
					ids[i] = Components[types[i]].HashCode;
				}
			} else {
				ids = new[] {Components[typeof(T)].HashCode};
			}

			return ids;
		}

		public static AbstractClass DirectClassSearch<T>() {
			return DirectClassSearch(ExtractComponents<T>());
		}

		public static AbstractClass[] LazyClassesSearch(uint[] components) {
			List<AbstractClass> results = new List<AbstractClass>();
			foreach (var pair in Classes) {
				int count = 0;
				for (int i = 0; i < pair.Value.Components.Length; i++) {
					for (int j = 0; j < components.Length; j++) {
						if (pair.Value.Components[i] == components[j]) {
							count++;
						}
					}
				}

				if (count > 0) {
					results.Add(pair.Value);
				}
			}

			return results.ToArray();
		}
		
		public static AbstractClass[] LazyClassesSearch<T>() {
			return LazyClassesSearch(ExtractComponents<T>());
		}

		public static AbstractClass GetClass(Type type) {
			return Classes[type];
		}
		public static AbstractClass GetClass<T>() {
			return GetClass(typeof(T));
		}

		public static AbstractClass GetClass(uint hashCode) {
			return Classes[HashSets[hashCode]];
		}
		
		public static AbstractComponent GetComponent(Type type) {
			return Components[type];
		}
		
		public static AbstractComponent GetComponent<T>() {
			return GetComponent(typeof(T));
		}
		
		public static AbstractComponent GetComponent(uint hashCode) {
			return Components[HashSets[hashCode]];
		}
		
		/// <summary>
		/// Gets all abstract immutable classes
		/// </summary>
		public static AbstractClass[] GetAllClasses() {
			return Classes.Values.ToArray();
		}
	}
}