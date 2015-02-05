using cint = Akira.CatalogData<int>;
namespace Akira
{
	public class Environment
	{
		public class OSVersoin
		{
			public static Akira.PlatformID Platform;
			public OSVersoin(){
				Platform=Akira.PlatformID.Android;
			}
		}
		/// <summary>
		/// Get variable of the specified key and value.
		/// this is the wrapper of any of Get Data of Windows Registry , Mac OS X plist , Debian dconf ,GNU gconf , Android SQLite or each Environment Variables.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void Variable(string key,ref string value)
		{

		}
		public void Variable(string key,ref ulong value)
		{
		}
		public void Variable(string key,ref bool value)
		{
		}
		public void Variable(string key,ref System.IO.FileInfo value)
		{

		}
	}
}