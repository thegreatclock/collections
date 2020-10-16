using System;

namespace GreatClock.Common.Collections {

	public sealed class CircleList<T> {

		private T[] mBuffer;
		private int mHead = 0;
		private int mSize = 0;

		public CircleList(int capacity) {
			if (capacity <= 0) {
				throw new IndexOutOfRangeException("Capacity should be greater than 0 !");
			}
			mBuffer = new T[capacity];
		}

		public int Capacity {
			get {
				return mBuffer.Length;
			}
		}

		public int Count {
			get {
				return mSize;
			}
		}

		public T Add(T item) {
			int capacity = mBuffer.Length;
			if (mSize == capacity) {
				T ret = mBuffer[mHead];
				mBuffer[mHead] = item;
				mHead++;
				if (mHead >= capacity) {
					mHead = 0;
				}
				return ret;
			}
			int index = mHead + mSize;
			if (index >= capacity) {
				index -= capacity;
			}
			mBuffer[index] = item;
			mSize++;
			return default(T);
		}

		public T RemoveFirst() {
			if (mSize <= 0) {
				throw new InvalidOperationException("Empty list !");
			}
			T ret = mBuffer[mHead];
			mBuffer[mHead] = default(T);
			mHead++;
			if (mHead >= mBuffer.Length) {
				mHead = 0;
			}
			mSize--;
			return ret;
		}

		public T RemoveLast() {
			if (mSize <= 0) {
				throw new InvalidOperationException("Empty list !");
			}
			int capacity = mBuffer.Length;
			int index = mHead + mSize - 1;
			if (index >= capacity) {
				index -= capacity;
			}
			T ret = mBuffer[index];
			mBuffer[index] = default(T);
			mSize--;
			return ret;
		}

		public void Clear() {
			int capacity = mBuffer.Length;
			if (mSize == capacity) {
				Array.Clear(mBuffer, 0, capacity);
			} else if (mHead + mSize <= capacity) {
				Array.Clear(mBuffer, mHead, mSize);
			} else {
				Array.Clear(mBuffer, mHead, capacity - mHead);
				Array.Clear(mBuffer, capacity - mHead, mHead + mSize - capacity);
			}
			mHead = 0;
			mSize = 0;
		}

		public T this[int index] {
			get {
				if (index >= mSize) {
					throw new IndexOutOfRangeException();
				}
				int capacity = mBuffer.Length;
				index += mHead;
				if (index >= capacity) {
					index -= capacity;
				}
				return mBuffer[index];
			}
			set {
				if (index >= mSize) {
					throw new IndexOutOfRangeException();
				}
				int capacity = mBuffer.Length;
				index += mHead;
				if (index >= capacity) {
					index -= capacity;
				}
				mBuffer[index] = value;
			}
		}

	}

}