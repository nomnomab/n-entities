using Unity.Mathematics;

namespace EcsSystem.Core.Components {
	public struct Transform {
		public float3 position;
		public float3 scale;
		public quaternion rotation;

		public override string ToString() {
			return $"Transform->(position: ({position.x}, {position.y}, {position.z}), rotation: ({rotation.value.x}, {rotation.value.y}, {rotation.value.z}), scale: ({scale.x}, {scale.y}, {scale.z}))";
		}
	}
}