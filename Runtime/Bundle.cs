using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GreatClock.Common.Collections {

	public sealed class Bundle {

		private Dictionary<string, object> mDict = new Dictionary<string, object>();

		private StringBuilder mStringBuilder = new StringBuilder();

		public Bundle() { }

		public Bundle(Bundle bundle) {
			if (bundle != null) {
				foreach (KeyValuePair<string, object> kv in bundle.mDict) {
					mDict.Add(kv.Key, kv.Value);
				}
			}
		}

		public Bundle Set<T>(string key, T value) {
			if (key == null) {
				throw new System.ArgumentNullException("key");
			}
			mDict.Remove(key);
			mDict.Add(key, value);
			return this;
		}

		public T Get<T>(string key) {
			if (key == null) {
				throw new System.ArgumentNullException("key");
			}
			object value;
			return mDict.TryGetValue(key, out value) && value is T ? (T)value : default(T);
		}

		public bool Contains(string key) {
			if (key == null) {
				throw new System.ArgumentNullException("key");
			}
			return mDict.ContainsKey(key);
		}

		public bool Contains<T>(string key) {
			if (key == null) {
				throw new System.ArgumentNullException("key");
			}
			object value;
			return mDict.TryGetValue(key, out value) && value is T;
		}

		public bool Remove(string key) {
			if (key == null) {
				throw new System.ArgumentNullException("key");
			}
			return mDict.Remove(key);
		}

		public override string ToString() {
			mStringBuilder.Length = 0;
			mStringBuilder.Append("[Bundle]{");
			bool isEmpty = true;
			foreach (KeyValuePair<string, object> kv in mDict) {
				mStringBuilder.Append(kv.Key);
				mStringBuilder.Append(':');
				object v = kv.Value;
				if ((v is IEnumerable) && !(v is string)) {
					mStringBuilder.Append('[');
					IEnumerator itor = (v as IEnumerable).GetEnumerator();
					bool flag = true;
					while (itor.MoveNext()) {
						string format = itor.Current is string ? "\"{0}\"," : "{0},";
						mStringBuilder.AppendFormat(format, itor.Current);
						flag = false;
					}
					if (!flag) {
						mStringBuilder.Remove(mStringBuilder.Length - 1, 1);
					}
					mStringBuilder.Append("],");
				} else {
					mStringBuilder.AppendFormat(v is string ? "\"{0}\"," : "{0},", v);
				}
				isEmpty = false;
			}
			if (!isEmpty) {
				mStringBuilder.Remove(mStringBuilder.Length - 1, 1);
			}
			mStringBuilder.Append('}');
			string ret = mStringBuilder.ToString();
			mStringBuilder.Remove(0, mStringBuilder.Length);
			return ret;
		}

	}

}
