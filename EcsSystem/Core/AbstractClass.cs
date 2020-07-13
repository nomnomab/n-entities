using System;
using System.Reflection;

namespace EcsSystem.Core {
	public class AbstractClass {
		public readonly Type ClassType;
		public readonly uint HashCode;
		public readonly uint[] Components;

		public AbstractClass(Type classType) {
			ClassType = classType;

			FieldInfo[] fields = ClassType.GetFields();
			Components = new uint[fields.Length];
			for (int i = 0; i < fields.Length; i++) {
				FieldInfo fieldInfo = fields[i];
				Components[i] = Registry.GetComponent(fieldInfo.FieldType).HashCode;
			}

			HashCode = xxHashBranch.DetermineHashCode(ClassType);
		}
	}
}