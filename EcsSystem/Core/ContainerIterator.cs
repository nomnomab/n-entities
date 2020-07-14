using System;
using System.Collections.Generic;
using System.Linq;
using EcsSystem.Core.Components;

namespace EcsSystem.Core {
	public class ContainerIterator {
		// [0]Health[]
		// [1]Transform[]
		public int ClassArrayLength => _classArrays.Length;
		public int ComponentsLength => _classArrays[_classIndex].Length;
		public int ElementLength => _classArrays[_classIndex][_index].Length;
		public int Index => _index;
		
		private readonly RefArray[][] _classArrays;
		private int _index;
		private int _classIndex;
		
		public ContainerIterator(RefArray[][] classClassArrays) {
			_index = -1;
			_classArrays = classClassArrays;
			
			for (var i = 0; i < _classArrays.Length; i++) {
				if (_classArrays[i] == null) {
					throw new Exception($"An array[{i}] supplied is null");
				}

				if (_classArrays[i].Length == 0) {
					throw new Exception($"An array[{i}] supplied has a length of 0");
				}
			}
		}

		public bool MoveNext() {
			_index++;
			
			if (_index >= ElementLength) {
				_classIndex++;
				_index = 0;

				if (_classIndex >= ClassArrayLength) {
					return false;
				}
				
				RefArray[] arrays = _classArrays[_classIndex];
				if (arrays.Length == 0) {
					return MoveNext();
				}
				
				for (int i = 0; i < arrays.Length; i++) {
					if (arrays[i].Length == 0) {
						return false;
					}
				}
				
				return _classIndex < ClassArrayLength;
			}

			return true;
		}
		
		public (Ref<A>, Ref<B>) Current<A, B>() {
			Console.WriteLine($"ClassArrayLength::{ClassArrayLength} | ComponentsLength::{ComponentsLength} | ElementLength::{ElementLength} | Index::{Index} | ClassIndex::{_classIndex}");
			RefArray[] arrays = _classArrays[_classIndex];
			return ILHelpers.CreateGetRefTuple<A, B>(arrays, Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>) Current<A, B, C>() {
			return ILHelpers.CreateGetRefTuple<A, B, C>(_classArrays[_classIndex], Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>, Ref<D>) Current<A, B, C, D>() {
			return ILHelpers.CreateGetRefTuple<A, B, C, D>(_classArrays[_classIndex], Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>) Current<A, B, C, D, E>() {
			return ILHelpers.CreateGetRefTuple<A, B, C, D, E>(_classArrays[_classIndex], Index);
		}
		
		public (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>, Ref<F>) Current<A, B, C, D, E, F>() {
			return ILHelpers.CreateGetRefTuple<A, B, C, D, E, F>(_classArrays[_classIndex], Index);
		}
	}
}