using System;
using System.Collections.Generic;

namespace Akira
{
	/// <summary>
	/// Platform ID.This is the replacement of System.PlatformID.
	/// The difference is it includes some mobile platform or not.
	/// </summary>
	public enum PlatformID
	{
		Win32S,Win32Windows,Win32NT,WinCE,Unix,Xbox,MacOSX,Android,iOS,WindowsPhone,UbuntuTouch
	};
	/// <summary>
	/// This class defines current platform and provide info.This class is static and it's not able to inherited.
	/// </summary>
	public static class PlatformInfo
	{
		public static PlatformID CurrentPlatformID;
		public static Platforms CurrentPlatform {
			get{
				return ToCode (CurrentPlatformID);
			}
		}
		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Akira.PlatformInfo"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Akira.PlatformInfo"/>.</returns>
		public static new string ToString ()
		{
			System.Collections.Generic.Dictionary<PlatformID,string> dic = new Dictionary<PlatformID, string> ();
			dic.Add (PlatformID.Android, "Android");
			dic.Add (PlatformID.iOS, "iOS");
			dic.Add (PlatformID.MacOSX, "Mac OS X");
			dic.Add (PlatformID.UbuntuTouch, "Ubuntu Touch");
			dic.Add (PlatformID.Unix, "Unix");
			dic.Add (PlatformID.Win32NT, "Win32 NT");
			dic.Add (PlatformID.Win32S, "Win32 S");
			dic.Add (PlatformID.Win32Windows, "Win32 Windows");
			dic.Add (PlatformID.WinCE, "Win32 CE");
			dic.Add (PlatformID.WindowsPhone, "Windows Phone");
			return string.Format (dic [CurrentPlatformID]);
		}
		/// <summary>
		/// Return the Platform code from the PlatformID.
		/// </summary>
		/// <returns>The code.</returns>
		/// <param name="p">P.</param>
		public static Platforms ToCode(PlatformID p)
		{
			int i = 1 << (int)CurrentPlatformID;
			Akira.Platforms ret = (Akira.Platforms)i;
			return ret;
		}
	}
}

namespace Akira
{
	/// <summary>
	/// Platforms data which can combine multiple number of platforms.
	/// </summary>
	public enum Platforms
	{
		Default=0,Win32S=1,Win32Windows=2,Win32NT=4,WinCE=8,
		Unix=16,Xbox=32,MacOSX=64,Android=128,iOS=256,WindowsPhone=512,UbuntuTouch=1024
	}
	/// <summary>
	/// Catalog data for cross platforming.
	/// This class can operate with T type,like
	/// <code>CatalogData&lt;int&gt;.platform = Akira.Platforms.Default;</code>
	/// <code>CatalogData&lt;int&gt; ca = new CatalogData\lt;int&gt;(Unix|MacOSX,0,1,2);</code>
	/// <code>int a = ca;</code>
	/// <code>a += ca;</code>
	/// However, this copies all the data in cb to ca.
	/// <code>CatalogData&lt;int&gt; ca,cb;</code>
	/// <code>ca = cb;</code>
	/// And this code is an error.
	/// <code>ca = 1;</code>
	/// So, if you want to change only the value of current platform, you should write like
	/// <code>ca.CurrentValue = cb + 1;</code>
	/// </summary>
	public class CatalogData<T>
	{
		private Dictionary<Platforms,T> obj;
		/// <summary>
		/// The platform.
		/// the CatalogData varies by this variable;
		/// the value is 0 if it is not inilized,so when you use this in library,
		/// You must not inilize this and you must inilize it when you use it
		/// in the platforms you want.
		/// </summary>
		public Akira.Platforms platform;
		/// <summary>
		/// Gets or sets the current value.
		/// </summary>
		/// <value>The current value of CatalogData&lt;T&gt;.</value>
		public T CurrentValue {
			get {
				if (obj.ContainsKey (platform)) {
					return obj [platform];
				} else {
					return obj [Platforms.Default];
				}
			}
			set {
				if (obj.Remove (platform)) {
					obj.Add (platform, value);
				} else {
					obj.Remove (Platforms.Default);
					obj.Add (Platforms.Default, value);
				}
			}
		}
		#region
		/// <summary>
		/// Initializes a new instance of the <see cref="Akira.CatalogData`1"/> class.
		/// </summary>
		/// <param name="platforms">All platforms to set in this class.</param>
		/// <param name="default_obj">Default object if not any correct platforms found.</param>
		/// <param name="objs">Objects for each platforms.</param>
		public CatalogData (Platforms platforms,T default_obj,params T[] objs)
		{
			obj = new Dictionary<Platforms, T> ();
			obj.Add (Platforms.Default, default_obj);
			int i,j;
			j = 0;
			Platforms p = (Platforms)1;
			for (i=0; i<10; i++) {
				if (((int)platforms & (int)p << i)!=0) {
					obj.Add ((Platforms)((int)p << i), objs [j]);
					j++;
				}
			}
			platform = PlatformInfo.CurrentPlatform;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Akira.CatalogData`1"/> class.
		/// </summary>
		/// <param name="data">The data set of platform and value.</param>
		public CatalogData (Dictionary<Akira.Platforms,T> data)
		{
			obj = new Dictionary<Platforms, T> ();
			foreach (KeyValuePair<Platforms, T> kvp in data) {
				obj.Add (kvp.Key, kvp.Value);
			}
			platform = PlatformInfo.CurrentPlatform;
		}
		private CatalogData (T type_obj)
		{
			obj = new Dictionary<Platforms,T> ();
			obj.Add (this.platform, type_obj);
			platform = PlatformInfo.CurrentPlatform;
		}
		#endregion
		/// <summary>
		/// Clone this instance.
		/// </summary>
		/// <returns>
		/// <c><see cref="Akira.CatalogData`1"/></c></returns>
		public virtual CatalogData<T> Clone ()
		{
			Platforms p = this.platform;
			Dictionary<Platforms,T> dic = new Dictionary<Platforms, T> ();
			this.platform = Platforms.Default;
			T rtd = this;
			dic.Add (Platforms.Default, rtd);
			int i;
			int d = 1;
			for (i=0; i<10; i++) {
				this.platform = (Platforms)(d << i);
				T rt = this;
				if (!(rt as object).Equals(rtd as object)) {
					dic.Add ((Platforms)(d << i), rt);
				}
			}
			CatalogData<T> rct = new CatalogData<T> (dic);
			this.platform = p;
			return rct;
		}
		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Akira.CatalogData`1"/>.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Akira.CatalogData`1"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Akira.CatalogData`1"/>; otherwise, <c>false</c>.</returns>
		/// However,if the argument is same as Akira.Catalog,
		/// <returns><c>true</c> if the Current Value equals.
		/// otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals (object obj)
		{
			T tobj;
			tobj = this.CurrentValue;
			if (obj is CatalogData<T>) {
				return tobj.Equals ((obj as CatalogData<T>).CurrentValue);
			} else {
				return tobj.Equals (obj);
			}
		}
		/// <summary>
		/// Serves as a hash function for a <see cref="Akira.CatalogData`1"/> object.
		/// It is for the current value.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			return this.CurrentValue.GetHashCode ();
		}
		public override string ToString ()
		{
			return string.Format ("[CatalogData: CurrentValue={0}]", CurrentValue);
		}
		public static implicit operator T(CatalogData<T> ct)
		{
			if (ct.obj.ContainsKey (ct.platform)) {
				return ct.obj [ct.platform];
			} else {
				return ct.obj [Platforms.Default];
			}
		}
		public static CatalogData<T> operator +(CatalogData<T> ct,dynamic t)
		{
			Platforms p = ct.platform;
			Dictionary<Platforms,T> dic = new Dictionary<Platforms, T> ();
			ct.platform = Platforms.Default;
			T rtd = ct;
			dic.Add (Platforms.Default, rtd);
			int i;
			int d = 1;
			for (i=0; i<10; i++) {
				ct.platform = (Platforms)(d << i);
				T rt = ct;
				if (!(rt as object).Equals(rtd as object)) {
					dic.Add ((Platforms)(d << i), rt);
				}
			}
			if (ct.obj.ContainsKey (p)) {
				dic.Remove (p);
				dic.Add (p, ct.obj [p] + t);
			} else {
				dic.Remove (Platforms.Default);
				dic.Add (Platforms.Default, ct.obj [Platforms.Default] + t);
			}
			CatalogData<T> rct = new CatalogData<T> (dic);
			ct.platform = p;
			return rct;
		}
		public static CatalogData<T> operator +(dynamic t,CatalogData<T> ct)
		{
			Platforms p = ct.platform;
			Dictionary<Platforms,T> dic = new Dictionary<Platforms, T> ();
			ct.platform = Platforms.Default;
			T rtd = ct;
			dic.Add (Platforms.Default, rtd);
			int i;
			int d = 1;
			for (i=0; i<10; i++) {
				ct.platform = (Platforms)(d << i);
				T rt = ct;
				if (!(rt as object).Equals(rtd as object)) {
					dic.Add ((Platforms)(d << i), rt);
				}
			}
			if (ct.obj.ContainsKey (p)) {
				dic.Remove (p);
				dic.Add (p, t + ct.obj [p]);
			} else {
				dic.Remove (Platforms.Default);
				dic.Add (Platforms.Default, t + ct.obj [Platforms.Default]);
			}
			CatalogData<T> rct = new CatalogData<T> (dic);
			ct.platform = p;
			return rct;
		}
		public static CatalogData<T> operator -(CatalogData<T> ct,dynamic t)
		{
			Platforms p = ct.platform;
			Dictionary<Platforms,T> dic = new Dictionary<Platforms, T> ();
			ct.platform = Platforms.Default;
			T rtd = ct;
			dic.Add (Platforms.Default, rtd);
			int i;
			int d = 1;
			for (i=0; i<10; i++) {
				ct.platform = (Platforms)(d << i);
				T rt = ct;
				if (!(rt as object).Equals(rtd as object)) {
					dic.Add ((Platforms)(d << i), rt);
				}
			}
			if (ct.obj.ContainsKey (p)) {
				dic.Remove (p);
				dic.Add (p, ct.obj [p] - t);
			} else {
				dic.Remove (Platforms.Default);
				dic.Add (Platforms.Default, ct.obj [Platforms.Default] - t);
			}
			CatalogData<T> rct = new CatalogData<T> (dic);
			ct.platform = p;
			return rct;
		}
		public static CatalogData<T> operator -(dynamic t,CatalogData<T> ct)
		{
			Platforms p = ct.platform;
			Dictionary<Platforms,T> dic = new Dictionary<Platforms, T> ();
			ct.platform = Platforms.Default;
			T rtd = ct;
			dic.Add (Platforms.Default, rtd);
			int i;
			int d = 1;
			for (i=0; i<10; i++) {
				ct.platform = (Platforms)(d << i);
				T rt = ct;
				if (!(rt as object).Equals(rtd as object)) {
					dic.Add ((Platforms)(d << i), rt);
				}
			}
			if (ct.obj.ContainsKey (p)) {
				dic.Remove (p);
				dic.Add (p, t - ct.obj [p]);
			} else {
				dic.Remove (Platforms.Default);
				dic.Add (Platforms.Default, t - ct.obj [Platforms.Default]);
			}
			CatalogData<T> rct = new CatalogData<T> (dic);
			ct.platform = p;
			return rct;
		}
	}
}