namespace EcsSystem.Core.Components {
	public class TestComp {
		public float value;

		public override string ToString() {
			return $"TestComp->(value: {value})";
		}
	}
}