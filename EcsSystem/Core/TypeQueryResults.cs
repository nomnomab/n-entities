using System;
using System.Collections.Generic;
using EcsSystem.Core.Components;

namespace EcsSystem.Core {
	public class TypeQueryResults {
		private readonly TypeQueryResultValue[] _SOAs;
		private readonly uint[] _components;
		
		public TypeQueryResults(TypeQueryResultValue[] SOAs, uint[] components) {
			_SOAs = SOAs;
			_components = components;
		}

		public ContainerIterator Restrict<T>() {
			uint id = Registry.DirectClassSearch<T>().HashCode;
			List<RefArray> classArrays = new List<RefArray>();

			for (var i = 0; i < _SOAs.Length; i++) {
				// check component hashcode against restriction
				if (_SOAs[i].HashCode == id) {
					classArrays.AddRange(_SOAs[i].ComponentArrays);
					break;
				}
			}

			return new ContainerIterator(new []{ classArrays.ToArray() });
		}

		public ContainerIterator GetIterator() {
			AbstractClass[] classes = Registry.LazyClassesSearch(_components);
			List<RefArray[]> arrays = new List<RefArray[]>();

			for (int i = 0; i < classes.Length; i++) {
				AbstractClass c = classes[i];
				bool done = false;
				
				for (int j = 0; j < _SOAs.Length; j++) {
					TypeQueryResultValue result = _SOAs[j];
					for (int k = 0; k < _components.Length; k++) {
						if (_SOAs[i].HashCode == c.HashCode) {
							RefArray[] classArgs = _SOAs[i].ComponentArrays;

							if (classArgs.Length == 0) {
								continue;
							}
							
							arrays.Add(classArgs);
							done = true;
							Console.WriteLine("Added: " + c.ClassType.FullName);
							break;
						}
					}

					if (done) {
						break;
					}
				}
			}
			
			return new ContainerIterator(arrays.ToArray());
		}
	}
}