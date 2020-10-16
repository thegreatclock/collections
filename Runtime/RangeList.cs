using System;
using System.Collections.Generic;

namespace GreatClock.Common.Collections {

	public class RangeList<K, V> {

		public delegate int CompareDelegate(K k1, K k2);
		public CompareDelegate comparer;

		private Node[] mNodes;
		private int mCount;

		private Comparer<K> mComparer = Comparer<K>.Default;

		public RangeList() {
			mNodes = new Node[4];
		}

		public RangeList(int capacity) {
			mNodes = new Node[capacity];
		}

		public int Count { get { return mCount; } }

		public int Capacity { get { return mNodes.Length; } }

		public void Add(K key, V value) {
			int index = mCount;
			mCount++;
			if (mCount >= mNodes.Length) {
				Node[] nodes = new Node[mNodes.Length << 1];
				Array.Copy(mNodes, 0, nodes, 0, mNodes.Length);
				mNodes = nodes;
			}
			while (index > 0) {
				Node n = mNodes[index - 1];
				if (Compare(n.key, key) <= 0) { break; }
				mNodes[index] = n;
				index--;
			}
			Node node = new Node();
			node.key = key;
			node.value = value;
			mNodes[index] = node;
		}

		public void Clear() {
			Array.Clear(mNodes, 0, mCount);
			mCount = 0;
		}

		public int GetRange(K key) {
			int min = 0;
			int max = mCount;
			while (min < max) {
				int index = (min + max) >> 1;
				Node node = mNodes[index];
				int cmp = Compare(node.key, key);
				if (cmp == 0) {
					return index;
				}
				if (cmp < 0) {
					min = index + 1;
				} else {
					max = index;
				}
			}
			return min - 1;
		}

		public Node this[int index] {
			get {
				if (index < 0 || index >= mCount) {
					throw new IndexOutOfRangeException();
				}
				return mNodes[index];
			}
		}

		public struct Node {
			public K key;
			public V value;
		}

		private int Compare(K k1, K k2) {
			return comparer != null ? comparer(k1, k2) : mComparer.Compare(k1, k2);
		}

	}

}
