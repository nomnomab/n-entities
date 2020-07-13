﻿using System;

namespace EcsSystem.Core {
	/// <summary>
	/// A structure of array for a class (Player, etc)
	/// </summary>
	public class ValueWrapper {
		public readonly Type ValueType;
		
		// Health[], Transform[], etc
		private object[] _values;

		public ValueWrapper(Type type) {
			ValueType = type;
			_values = new object[0];
		}

		public RefArray GetRefArray() {
			return new RefArray(_values);
		}

		public void Add() {
			var instance = Activator.CreateInstance(ValueType);
			Array.Resize(ref _values, _values.Length + 1);
			_values[_values.Length - 1] = instance;
			
			Console.WriteLine($"ValueWrapper<{ValueType.Name}>\t::Add({_values[_values.Length - 1]})");
		}

		public override string ToString() {
			return $"ValueWrapper<{ValueType.Name}>\t::[{_values.Length}] -> {string.Join(", ", _values)}";
		}
		
#if DEBUG
		public T GetValue<T>(int index) {
			return (T)_values[index];
		}
#endif
	}
}