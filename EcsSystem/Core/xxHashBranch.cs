using System;
using System.Text;
using xxHashSharp;

namespace EcsSystem.Core {
	public static class xxHashBranch {
		public static uint DetermineHashCode(Type type) {
			byte[] bytes = Encoding.UTF8.GetBytes(type.FullName);
			xxHash hash = new xxHash();
			hash.Init();
			hash.Update(bytes, bytes.Length);
			return hash.Digest();
		}
	}
}