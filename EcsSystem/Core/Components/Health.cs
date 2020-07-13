namespace EcsSystem.Core.Components {
	public struct Health {
		public float value;

		public override string ToString() {
			return $"Health->(value: {value})";
		}
	}
}