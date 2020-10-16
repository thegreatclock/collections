using System;

namespace GreatClock.Common.Collections {

	public sealed class LinkListQueue<T> {

		private object mEnqueueLock = new object();
		private object mDequeueLock = new object();

		private Node mHead;
		private Node mTail;

		private long mEnqueueCount = 0L;
		private long mDequeueCount = 0L;

		private Node mCachedHead;
		private Node mCachedTail;

		public LinkListQueue() {
			Init(4);
		}

		public LinkListQueue(int capacity) {
			Init(capacity);
		}

		public void Enqueue(T item) {
			lock (mEnqueueLock) {
				Node node = mCachedHead;
				if (node == mCachedTail) {
					node = new Node();
				} else {
					mCachedHead = node.next;
				}
				node.next = null;
				mTail.data = item;
				mTail.next = node;
				mEnqueueCount++;
				mTail = node;
			}
		}

		public T Dequeue() {
			lock (mDequeueLock) {
				Node head = mHead;
				if (head == mTail) {
					throw new InvalidOperationException("Empty Queue");
				}
				mDequeueCount++;
				T item = head.data;
				mHead = head.next;
				head.data = default(T);
				head.next = null;
				mCachedTail.next = head;
				mCachedTail = head;
				return item;
			}
		}

		public void Clear() {
			while (mHead != mTail) {
				Node head = mHead;
				mHead = head.next;
				head.data = default(T);
				head.next = null;
				mCachedTail.next = head;
				mCachedTail = head;
			}
			mEnqueueCount = 0L;
			mDequeueCount = 0L;
		}

		public int Count { get { return (int)(mEnqueueCount - mDequeueCount); } }

		private void Init(int capacity) {
			mHead = new Node();
			mTail = mHead;
			mCachedHead = new Node();
			mCachedTail = mCachedHead;
			for (int i = 0; i < capacity; i++) {
				Node node = new Node();
				mCachedTail.next = node;
				mCachedTail = node;
			}
		}

		private class Node {
			public Node next;
			public T data;
		}

	}

}