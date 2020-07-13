using System;
using System.Collections.Generic;
using System.Linq;

namespace EcsSystem.Core {
	public class ContainerIterator {
		// [0]Health[]
		// [1]Transform[]
		private RefArray[] _classArrays;
		private int _index;
		
		public ContainerIterator(RefArray[] classClassArrays) {
			_classArrays = classClassArrays;
		}

		public bool MoveNext() {
			_index++;

			if (_index < _classArrays.Length) {
				return true;
			}
			
			_index--;
			return false;
		}

		/// <summary>
		/// Get a tuple for the current index across each array
		/// </summary>
		public ref T Current<T>() where T: unmanaged {
			//ILHelpers.CreateGetRefTuple<T>(ref _classArrays, _index);
			throw null;
			// var instance = Activator.CreateInstance<T>();
			//
			// Type[] types = instance
			// 	.GetType()
			// 	.GetFields()
			// 	.Select(f => typeof(Ref<>).MakeGenericType(f.FieldType))
			// 	.ToArray();
			//
			// dynamic[] values = new dynamic[_classArrays.Length];
			//
			// ILHelpers.CreateGetRefTuple<T>();
			//
			// typeof(Tuple).GetMethods()
			// 	.FirstOrDefault(method => method.Name.Equals("Create") && method.GetParameters().Length == types.Length)
			// 	?.MakeGenericMethod(types)
			// 	?.Invoke(null, values);
		}
	}
}