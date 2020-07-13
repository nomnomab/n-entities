namespace EcsSystem.Core {
	/// <summary>
	/// Maps to a single class
	/// </summary>
	public class TypeQueryResultValue {
		public readonly uint HashCode;
		public readonly RefArray[] ComponentArrays;
		
		public TypeQueryResultValue(AbstractClass abstractClass, RefArray[] arrays) {
			HashCode = abstractClass.HashCode;
			ComponentArrays = arrays;
		}
	}
}