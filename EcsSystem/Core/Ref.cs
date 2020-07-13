using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EcsSystem.Core {
	public class Ref<T> {
		private IntPtr _ptr;

		public unsafe Ref(void* pointer) {
			_ptr = new IntPtr(pointer);
		}

		public Ref(ref object raw) {
			_ptr = ILHelpers.GetPinnedPointer(ref raw);
		}
		
		public Ref(ref T raw) {
			_ptr = ILHelpers.GetGenericPtr(ref raw);
		}

		public ref T Unwrap() {
			unsafe {
				return ref Unsafe.AsRef<T>(_ptr.ToPointer());
			}
		}
	}
}