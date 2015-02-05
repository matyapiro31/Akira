using cstring = Akira.CatalogData<string>;
namespace Akira
{
	public class String
	{
		public string BaseString;
		public char Quote;
		public bool UseQuote;
		private char[] Endl;
		public Akira.String[] Col {
			set {//TODO

			}
			get {//TODO
				System.IO.StringReader sr = new System.IO.StringReader ((string)this);
				string line;
				if (!UseQuote) {
					System.Collections.Generic.List<Akira.String> ret;
					while ((line=sr.ReadLine())!=null) {

					}

				} else {
				}
				return null;
			}
		}
		public Akira.String[] Row {
			set {//TODO
				int i = 0;
				foreach (Akira.String item in value) {
					if (item != null && item.BaseString.Length >0) {
						string itembase = item.BaseString;
						if (item.UseQuote && this.UseQuote) {
							//Replace item's quote to this one's.

						}
						if (item.Endl != this.Endl) {
							//Replace item's end of line to this one's.
						}
					}
				}
			}
			get {//TODO
				if (!UseQuote) {
					return this.Split (Endl);
				} else {
					return null;
				}
			}
		}
		public String ()
		{
			UseQuote = false;
			Quote = '\"';
			System.Collections.Generic.Dictionary<Akira.Platforms,string> data = new System.Collections.Generic.Dictionary<Platforms, string>();
			data.Add (Platforms.Win32NT, "\n\r");
			data.Add (Platforms.Unix, "\n");
			data.Add (Platforms.Default, "\n");
			data.Add (Platforms.iOS, "\r");
			data.Add (Platforms.MacOSX, "\r");
			cstring cs = new cstring (data);
			Endl = ((string)cs).ToCharArray ();
		}
		public unsafe Akira.String[] Split(char[] word)
		{
			string[] s = this.BaseString.Split (word);
			Akira.String[] ret = new Akira.String[s.Length];
			for (int i=0; i<ret.Length; i++) {
				ret[i].BaseString = s [i];
				ret[i].Endl = this.Endl;
				ret[i].Quote = this.Quote;
				ret[i].UseQuote = this.UseQuote;
			}
			return ret;
		}
		/// <summary>
		/// Replace the specified oldchar to  newchar.
		/// </summary>
		/// <param name="str">String.</param> 
		/// <param name="oldchar">Old char to replace.</param>
		/// <param name="newchar">New char to replace.</param>
		/// <param name="location">Location that specify where the char to replace.
		/// if you specify minus number  -1...-string.Length is same as from string end to string start.</param>
		public	unsafe System.String Replace(string str,char oldchar,char newchar,int[] location)
		{
			char[] rawstr = str.ToCharArray ();
			fixed(char* prstr = &rawstr[0]) {
				for (int i=0;i<location.Length;i++) {
				int l = location [i];
					if (-str.Length > l || str.Length <= l) {
						throw new System.ArgumentOutOfRangeException (); 
					} else {
						int n = l < 0 ? l + str.Length : l;
						if (n == 0) {
							*(prstr + n) = *(prstr + n) == oldchar ? newchar : *(prstr + n);
						} else {
							*(prstr + n) = *(prstr + n) == oldchar && *(prstr + n - 1) != '\'' ? newchar : *(prstr + n);
						}
					}
				}
			}
			return new string (rawstr);
		}
		public static explicit operator System.String (Akira.String s)
		{
			return s.BaseString;
		}
		public static explicit operator Akira.String (System.String s)
		{
			Akira.String ret = new Akira.String ();
			ret.BaseString = s;
			return ret;
		}
	}
}
/*
public static string readcsv(string line,int number)
{
	string[] ret = new string[line.Split (Endl).Length];

	do {
		if (line.StartsWith (Quote)) {
			ret.Add(line.Split (Quote) [1]);
			line = line.Substring(line.Split (Quote) [1].Length+1);
			if (line.Split (Endl).Length < 2) {
				break;
			} else {
				line = line.Substring(line.Split (Endl) [0].Length+1);
			}
		} else {
			ret [i] = line.Split (Endl) [0];
			if (line.Split (Endl).Length > 1) {
				line = line.Substring(line.Split (Endl) [0].Length+1);
			} else {
				break;
			}
		}
	} while(true);

	return ret[number];
}*/