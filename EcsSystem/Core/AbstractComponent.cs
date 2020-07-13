using System;
using System.Text;
using xxHashSharp;

namespace EcsSystem.Core {
	public class AbstractComponent {
		public readonly Type ComponentType;
		public readonly uint HashCode;
		
		private readonly AbstractValue[] _values;

		public AbstractComponent(Type componentType) {
			ComponentType = componentType;
			HashCode = xxHashBranch.DetermineHashCode(ComponentType);
		}
	}
}