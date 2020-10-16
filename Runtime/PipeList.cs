using System;

namespace GreatClock.Common.Collections {

	public sealed class PipeList<T> {

		private T[] mBuffer;
		private int mHead = 0;
		private int mSize = 0;

		public PipeList() {
			mBuffer = new T[4];
		}

		public PipeList(int capacity) {
			if (capacity <= 0) {
				throw new IndexOutOfRangeException("Capacity should be greater than 0 !");
			}
			mBuffer = new T[capacity];
		}

		public PipeList(System.Collections.Generic.List<T> list) {
			if (list == null || list.Count < 0) {
				throw new InvalidOperationException("List should not be null or empty !");
			}
			mBuffer = list.ToArray();
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

		public void Add(T item) {
			int capacity = mBuffer.Length;
			if (mSize >= capacity) {
				ExtendArray();
				capacity = mBuffer.Length;
			}
			int index = mHead + mSize;
			if (index >= capacity) {
				index -= capacity;
			}
			mBuffer[index] = item;
			mSize++;
		}

		public void Insert(T item) {
			int capacity = mBuffer.Length;
			if (mSize >= capacity) {
				ExtendArray();
				capacity = mBuffer.Length;
			}
			mHead--;
			if (mHead < 0) {
				mHead += capacity;
			}
			mBuffer[mHead] = item;
			mSize++;
		}

		public T Shift() {
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

		public T Remove() {
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

		public T[] ToArray() {
			T[] ret = new T[mSize];
			int capacity = mBuffer.Length;
			if (mHead + mSize <= capacity) {
				Array.Copy(mBuffer, mHead, ret, 0, mSize);
			} else {
				Array.Copy(mBuffer, mHead, ret, 0, capacity - mHead);
				Array.Copy(mBuffer, 0, ret, capacity - mHead, mHead + mSize - capacity);
			}
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
				if (index < 0 || index >= mSize) {
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
				if (index < 0 || index >= mSize) {
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

		private void ExtendArray() {
			int capacity = mBuffer.Length;
			T[] buffer = new T[capacity << 1];
			Array.Copy(mBuffer, mHead, buffer, 0, capacity - mHead);
			Array.Copy(mBuffer, 0, buffer, capacity - mHead, mHead);
			mBuffer = buffer;
			mHead = 0;
		}

	}

}