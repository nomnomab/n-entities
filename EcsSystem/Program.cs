using EcsSystem.Core;
using EcsSystem.Core.Components;

namespace EcsSystem {
	internal class Program {
		public static void Main(string[] _) {
			ValueWrapper[] wrappers = {
				new ValueWrapper(typeof(Health)),
				new ValueWrapper(typeof(Transform)),
			};
			
			wrappers[0].Add();
			wrappers[1].Add();
			
			RefArray[] refArrays = {
				wrappers[0].GetRefArray(),
				wrappers[1].GetRefArray()
			};
			
			var (health, transform) = ILHelpers.CreateGetRefTuple<Health, Transform>(refArrays, 0);
			health.Unwrap().value = 500;
		}
	}
}