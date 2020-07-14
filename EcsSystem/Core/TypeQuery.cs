using System.Collections.Generic;

namespace EcsSystem.Core {
	public class TypeQuery {
		private readonly List<uint> _includedTypes;

		public TypeQuery() {
			_includedTypes = new List<uint>();
		}

		public TypeQuery With<T>() {
			_includedTypes.Add(Registry.GetComponent<T>().HashCode);
			return this;
		}

		public TypeQueryResults Execute(EcsTable table) {
			// get all types that contain the supplied components
			uint[] components = _includedTypes.ToArray();
			return new TypeQueryResults(table.GetSOAs(components), components);
		}
	}
}