using System.Collections.Generic;
using EcsSystem.Core.Components;

namespace EcsSystem.Core {
	public class TypeQueryResults {
		private readonly TypeQueryResultValue[] _SOAs;
		
		public TypeQueryResults(TypeQueryResultValue[] SOAs) {
			_SOAs = SOAs;
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

			return new ContainerIterator(classArrays.ToArray());
		}
	}
}