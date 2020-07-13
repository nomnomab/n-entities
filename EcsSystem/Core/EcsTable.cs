using System;
using System.Collections.Generic;

namespace EcsSystem.Core {
	public class EcsTable {
		// one per class [Player, etc]
		private readonly Dictionary<uint, ValueWrapper[]> _containers;
		
		public EcsTable() {
			_containers = new Dictionary<uint, ValueWrapper[]>();

			var classes = Registry.GetAllClasses();
			for (int i = 0; i < classes.Length; i++) {
				AbstractClass abstractClass = classes[i];
				ValueWrapper[] wrappers = new ValueWrapper[abstractClass.Components.Length];
				
				for (int j = 0; j < wrappers.Length; j++) {
					wrappers[j] = new ValueWrapper(Registry.GetComponent(abstractClass.Components[j]).ComponentType);
				}
				
				_containers.Add(abstractClass.HashCode, wrappers);
			}
		}

		/// <summary>
		/// Adds a new instance of a class and pushes it onto the end of
		/// the stack
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public void CreateEntity<T>() {
			AbstractClass abstractClass = Registry.DirectClassSearch<T>();
			var wrappers = _containers[abstractClass.HashCode];
			
			for (int i = 0; i < wrappers.Length; i++) {
				wrappers[i].Add();
			}
		}

		/// <summary>
		/// Getsn't
		/// </summary>
		public TypeQueryResultValue[] GetSOAs(uint[] components) {
			AbstractClass[] results = Registry.LazyClassesSearch(components);
			TypeQueryResultValue[] values = new TypeQueryResultValue[results.Length];
			
			for (int i = 0; i < values.Length; i++) {
				var value = _containers[results[i].HashCode];
				// maps to a single class
				RefArray[] refArrays = new RefArray[value.Length];
				
				for (var j = 0; j < refArrays.Length; j++) {
					// for each component
					refArrays[j] = value[j].GetRefArray();
				}
				
				values[i] = new TypeQueryResultValue(results[i], refArrays);
				Console.WriteLine($"EcsTable::GetSOAs\t::[{string.Join(", ", components)}]->{results[i].ClassType.Name}");
			}

			return values;
		}
		
		public void DebugClass<T>() {
			AbstractClass abstractClass = Registry.DirectClassSearch<T>();
			var wrappers = _containers[abstractClass.HashCode];
			
			for (int i = 0; i < wrappers.Length; i++) {
				Console.WriteLine(wrappers[i]);
			}
		}
	}
}