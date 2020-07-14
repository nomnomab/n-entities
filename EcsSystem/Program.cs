using System;
using EcsSystem.Core;
using EcsSystem.Core.Classes;
using EcsSystem.Core.Components;

namespace EcsSystem {
	internal class Program {
		public static void Main(string[] _) {
			Registry.RegisterAll();
			
			EcsTable ecsTable = new EcsTable();
			// ecsTable.CreateEntity<(Health, Transform)>();
			// ecsTable.CreateEntity<(Health, Transform)>();
			ecsTable.CreateEntity<(Health, Transform)>();
			ecsTable.CreateEntity<(Health, Transform, TestComp)>();

			// TypeQuery query = new TypeQuery()
			// 	.With<Health>();
			//
			// TypeQueryResults results = query.Execute(ecsTable);
			// var iter = results.Restrict<(Health, Transform)>();

			var iter = new TypeQuery()
				.With<Health>()
				.With<Transform>()
				.Execute(ecsTable)
				.GetIterator();
			
			while (iter.MoveNext()) {
				var (health, transform) = iter.Current<Health, Transform>();
				health.Unwrap().value = 500;
			}
			
			ecsTable.DebugClass<(Health, Transform)>();
			ecsTable.DebugClass<(Health, Transform, TestComp)>();
		}
	}
}