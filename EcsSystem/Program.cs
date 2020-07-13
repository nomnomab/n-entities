using System;
using EcsSystem.Core;
using EcsSystem.Core.Components;

namespace EcsSystem {
	internal class Program {
		public static void Main(string[] _) {
			Registry.RegisterAll();
			
			EcsTable ecsTable = new EcsTable();
			ecsTable.CreateEntity<(Health, Transform)>();
			
			TypeQuery query = new TypeQuery()
				.With<Health>();

			TypeQueryResults results = query.Execute(ecsTable);
			var iter = results.Restrict<(Health, Transform)>();
			
			while (iter.MoveNext()) {
				var (health, transform) = iter.Current<Health, Transform>();
				health.Unwrap().value = 500;
			}
			
			ecsTable.DebugClass<(Health, Transform)>();

			// ValueWrapper[] wrappers = {
			// 	new ValueWrapper(typeof(Health)),
			// 	new ValueWrapper(typeof(Transform))
			// };
			//
			// wrappers[0].Add();
			// wrappers[1].Add();
			//
			// RefArray[] refArrays = {
			// 	wrappers[0].GetRefArray(),
			// 	wrappers[1].GetRefArray()
			// };
			//
			// var (health, transform) = ILHelpers.CreateGetRefTuple<Health, Transform>(refArrays, 0);
			// health.Unwrap().value = 500;
		}
	}
}