using System;
using System.Runtime.CompilerServices;

namespace EcsSystem.Core {
	public class RefArray {
		public int Length { get; private set; }
		
		private readonly Wrapper<object> _wrapper;
		private readonly IntPtr _byteOffset;

		public RefArray(object[] array) {
			Length = array.Length;
			_wrapper = Unsafe.As<Wrapper<object>>(array);
			_byteOffset = SpanHelpers.PerTypeValues<object>.ArrayAdjustment;
		}

		public ref object Unwrap(int index) {
			return ref Unsafe.Add(ref Unsafe.AddByteOffset(ref _wrapper.Value, _byteOffset), index);
		}

		public ref T SplitUnmanaged<T>(int index) where T: unmanaged {
			ref T t = ref Unsafe.Unbox<T>(Unsafe.Add(ref Unsafe.AddByteOffset(ref _wrapper.Value, _byteOffset), index));
			return ref t;
		}

		// doesn't fucking WORK; make IL-side
		public bool SplitFilterUnmanaged<T>(int index, ref T output, Func<T, bool> func) where T: unmanaged {
			output = ref Unsafe.Unbox<T>(Unsafe.Add(ref Unsafe.AddByteOffset(ref _wrapper.Value, _byteOffset), index));
			bool result = func(output);
			return result;
		}
		
		public Ref<object> Split(int index) {
			return new Ref<object>(ref Unsafe.Add(ref Unsafe.AddByteOffset(ref _wrapper.Value, _byteOffset), index));
		}
	}
}