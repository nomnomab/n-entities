using System;
using System.Collections.Generic;
using System.Linq;
using EcsSystem.Core.Components;

namespace EcsSystem.Core {
	public class ContainerIterator {
		// [0]Health[]
		// [1]Transform[]
		public int Length => _classArrays[0].Length;
		public int Index => _index - 1;
		
		private readonly RefArray[] _classArrays;
		private int _index;
		
		public ContainerIterator(RefArray[] classClassArrays) {
			_classArrays = classClassArrays;

			for (var i = 0; i < _classArrays.Length; i++) {
				if (_classArrays[i].Length == 0) {
					throw new Exception("An array supplied has a length of 0");
				}
			}
		}

		public bool MoveNext() {
			if (_index + 1 > Length) {
				return false;
			}
			
			_index++;
			return true;
		}
		
		public (Ref<A>, Ref<B>) Current<A, B>() {
			return ILHelpers.CreateGetRefTuple<A, B>(_classArrays, Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>) Current<A, B, C>() {
			return ILHelpers.CreateGetRefTuple<A, B, C>(_classArrays, Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>, Ref<D>) Current<A, B, C, D>() {
			return ILHelpers.CreateGetRefTuple<A, B, C, D>(_classArrays, Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>) Current<A, B, C, D, E>() {
			return ILHelpers.CreateGetRefTuple<A, B, C, D, E>(_classArrays, Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>, Ref<F>) Current<A, B, C, D, E, F>() {
			return ILHelpers.CreateGetRefTuple<A, B, C, D, E, F>(_classArrays, Index);
		}
	}
}