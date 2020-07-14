using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using EcsSystem.Core.Components;

namespace EcsSystem.Core {
	public static class ILHelpers {
		/* [ 2] */ public delegate (Ref<A>, Ref<B>) GetTuple<A, B>(RefArray[] objects, int index);
		/* [ 2] */ public delegate (Ref<A>, Ref<B>, Ref<C>) GetTuple<A, B, C>(RefArray[] objects, int index);
		/* [ 2] */ public delegate (Ref<A>, Ref<B>, Ref<C>, Ref<D>) GetTuple<A, B, C, D>(RefArray[] objects, int index);
		/* [ 2] */ public delegate (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>) GetTuple<A, B, C, D, E>(RefArray[] objects, int index);
		/* [ 2] */ public delegate (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>, Ref<F>) GetTuple<A, B, C, D, E, F>(RefArray[] objects, int index);

		public delegate IntPtr GetPinnedPtr(ref object arg);
		public delegate IntPtr GetPinnedPtrGeneric<T>(ref T arg);

		public static GetPinnedPtr GetPinnedPointer;
		//public static GetPinnedPtrGeneric<int> GetPinnedPointerGeneric;

		static ILHelpers() {
			CreateGetPinnedPtr();
			CreateGetPinnedPointerGeneric<int>();
		}

		public static IntPtr GetGenericPtr<T>(ref T obj) {
			return CreateGetPinnedPointerGeneric<T>()(ref obj);
		}

		public static (Ref<A>, Ref<B>) CreateGetRefTuple<A, B>(RefArray[] objects, int index) {
			Type[] types = {typeof(A), typeof(B)};
			var getTuple = (GetTuple<A, B>) _CreateGetRefTuple<ValueTuple<Ref<A>, Ref<B>>>(types, objects, index).CreateDelegate(typeof(GetTuple<A, B>));
			return getTuple(objects, index);
		}
		
		public static (Ref<A>, Ref<B>, Ref<C>) CreateGetRefTuple<A, B, C>(RefArray[] objects, int index) {
			Type[] types = {typeof(A), typeof(B), typeof(C)};
			var getTuple = (GetTuple<A, B, C>) _CreateGetRefTuple<ValueTuple<Ref<A>, Ref<B>, Ref<C>>>(types, objects, index).CreateDelegate(typeof(GetTuple<A, B, C>));
			return getTuple(objects, index);
		}
		
		public static (Ref<A>, Ref<B>, Ref<C>, Ref<D>) CreateGetRefTuple<A, B, C, D>(RefArray[] objects, int index) {
			Type[] types = {typeof(A), typeof(B), typeof(C), typeof(D)};
			var getTuple = (GetTuple<A, B, C, D>) _CreateGetRefTuple<ValueTuple<Ref<A>, Ref<B>, Ref<C>, Ref<D>>>(types, objects, index).CreateDelegate(typeof(GetTuple<A, B, C, D>));
			return getTuple(objects, index);
		}
		
		public static (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>) CreateGetRefTuple<A, B, C, D, E>(RefArray[] objects, int index) {
			Type[] types = {typeof(A), typeof(B), typeof(C), typeof(D), typeof(E)};
			var getTuple = (GetTuple<A, B, C, D, E>) _CreateGetRefTuple<ValueTuple<Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>>>(types, objects, index).CreateDelegate(typeof(GetTuple<A, B, C, D, E>));
			return getTuple(objects, index);
		}
		
		public static (Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>, Ref<F>) CreateGetRefTuple<A, B, C, D, E, F>(RefArray[] objects, int index) {
			Type[] types = {typeof(A), typeof(B), typeof(C), typeof(D), typeof(E), typeof(F)};
			var getTuple = (GetTuple<A, B, C, D, E, F>) _CreateGetRefTuple<ValueTuple<Ref<A>, Ref<B>, Ref<C>, Ref<D>, Ref<E>, Ref<F>>>(types, objects, index).CreateDelegate(typeof(GetTuple<A, B, C, D, E, F>));
			return getTuple(objects, index);
		}

		// private static void _Create<T /* The tuple arg types */>(RefArray[] objects, int index) {
		// 	var givenTupleInstance = Activator.CreateInstance<T>();
		//
		// 	// (Health, Transform)
		// 	Type[] givenTupleTypes = givenTupleInstance
		// 		.GetType()
		// 		.GetFields()
		// 		.Select(f => f.FieldType)
		// 		.ToArray();
		// 	
		// 	Type[] mappedRefTypes = new Type[givenTupleTypes.Length];
		// 	for (int i = 0; i < mappedRefTypes.Length; i++) {
		// 		mappedRefTypes[i] = typeof(Ref<>).MakeGenericType(givenTupleTypes[i]);
		// 	}
		// 	
		// 	
		//
		// 	// return (Ref<Health>, Ref<Transform>)
		// }
		
		private static DynamicMethod _CreateGetRefTuple<T>(
			Type[] types /* Type args */, 
			RefArray[] arrays /* Array from each Class instance */, 
			int index /* Index in each array */) {
			// [0] A& a,
			// [1] B& b,
			// [2] A* rA,
			// [3] A& pinned V_3,
			// [4] B* rB,
			// [5] B& pinned V_5,
			// [6] (Ref<A>, Ref<B>) output
			
			Type[] tupleRefArgs = new Type[types.Length];
			Type[] tuplePointerArgs = new Type[types.Length];
			Type[] returnArgs = new Type[types.Length];
			for (int i = 0; i < tupleRefArgs.Length; i++) {
				tupleRefArgs[i] = types[i].MakeByRefType();
				tuplePointerArgs[i] = types[i].MakePointerType();
				returnArgs[i] = typeof(Ref<>).MakeGenericType(types[i]);
			}
			
			var dyn = new DynamicMethod("CreateRefTuple", 
				typeof(T), // return type
				new[] { typeof(RefArray[]), typeof(int) }, // args
				typeof(ILHelpers).Module);
			
			var il = dyn.GetILGenerator();
			
			// ref locals
			for (int i = 0; i < tupleRefArgs.Length; i++) {
				il.DeclareLocal(tupleRefArgs[i]); // don't pin; will do manually
			}
			
			// stored & pinned locals
			for (int i = 0; i < tupleRefArgs.Length; i++) {
				il.DeclareLocal(tuplePointerArgs[i]); // Type*
				il.DeclareLocal(tupleRefArgs[i], true); // Type& pinned
			}
			
			il.DeclareLocal(typeof(T));

			// get unmanaged split value from array -> ref T
			for (int i = 0; i < tupleRefArgs.Length; i++) {
				il.Emit(OpCodes.Ldarg, 0); // RefArray[]
				il.Emit(OpCodes.Ldc_I4, i); // RefArray[i]
				il.Emit(OpCodes.Ldelem_Ref); // ref
				il.Emit(OpCodes.Ldarg, 1); // index
				il.Emit(OpCodes.Callvirt, typeof(RefArray).GetMethod("SplitUnmanaged").MakeGenericMethod(types[i]));
				il.Emit(OpCodes.Stloc, i); // set Type&[i]
			}
			
			// pin references
			for (int i = 0; i < tupleRefArgs.Length; i++) {
				int pinnedIndex = 2 + 2 * i;
				il.Emit(OpCodes.Ldloc, i); // load Type&[i]
				il.Emit(OpCodes.Stloc, pinnedIndex + 1); // set Type& pinned[pinnedIndex + 1]
				il.Emit(OpCodes.Ldloc, pinnedIndex + 1); // load Type& pinned[pinnedIndex + 1]
				il.Emit(OpCodes.Conv_U); // convert to a pointer
				il.Emit(OpCodes.Stloc, pinnedIndex); // set Type*[pinnedIndex]
			}
			
			// newObject each Type*
			for (int i = 0; i < tupleRefArgs.Length; i++) {
				int pinnedIndex = 2 + 2 * i;
				il.Emit(OpCodes.Ldloc, pinnedIndex); // load Type*[pinnedIndex]
				il.Emit(OpCodes.Newobj, typeof(Ref<>).MakeGenericType(types[i]).GetConstructors()[0]); // create Ref<T>
			}
			
			// create return object
			il.Emit(OpCodes.Newobj, typeof(T).GetConstructors()[0]); // create AbstractTuple<Ref<T>, etc>
			il.Emit(OpCodes.Stloc, types.Length * 3); // set AbstractTuple<Ref<T>, etc>
			il.Emit(OpCodes.Ldloc, types.Length * 3); // load AbstractTuple<Ref<T>, etc>
			il.Emit(OpCodes.Ret); // return AbstractTuple<Ref<T>, etc>

			return dyn;
		}
		
		private static void CreateGetPinnedPtr() {
			var dyn = new DynamicMethod("GetPinnedPtr", typeof(IntPtr), new[] {typeof(object).MakeByRefType()}, typeof(ILHelpers).Module);
			var il = dyn.GetILGenerator();
			il.DeclareLocal(typeof(object), true);

			il.Emit(OpCodes.Ldarg, 0);
			il.Emit(OpCodes.Stloc, 0);
			il.Emit(OpCodes.Ldloc, 0);
			il.Emit(OpCodes.Conv_U);
			//il.Emit(OpCodes.Call, typeof(Action<IntPtr>).GetMethod("Invoke"));
			il.Emit(OpCodes.Ret);

			GetPinnedPointer = (GetPinnedPtr) dyn.CreateDelegate(typeof(GetPinnedPtr));
		}
		
		private static GetPinnedPtrGeneric<T> CreateGetPinnedPointerGeneric<T>() {
			var dyn = new DynamicMethod("GetPinnedPtr", typeof(IntPtr), new[] {typeof(T).MakeByRefType()}, typeof(ILHelpers).Module);
			var il = dyn.GetILGenerator();
			il.DeclareLocal(typeof(T), true);

			il.Emit(OpCodes.Ldarg, 0);
			il.Emit(OpCodes.Stloc, 0);
			il.Emit(OpCodes.Ldloc, 0);
			il.Emit(OpCodes.Conv_U);
			//il.Emit(OpCodes.Call, typeof(Action<IntPtr>).GetMethod("Invoke"));
			il.Emit(OpCodes.Ret);

			return (GetPinnedPtrGeneric<T>) dyn.CreateDelegate(typeof(GetPinnedPtrGeneric<T>));
		}

		private static void CreateConvertRefObjectToRefGeneric<T>(Ref<object> reference) {
			var dyn = new DynamicMethod("ConvertRef", typeof(Ref<T>), new[] {typeof(Ref<object>)}, typeof(ILHelpers).Module);
			var il = dyn.GetILGenerator();
		}
	}
}