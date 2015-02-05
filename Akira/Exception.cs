using System.Collections.Generic;
using cstring = Akira.CatalogData<string>;
namespace Akira
{
	/// <summary>
	/// Exception.This support multi-platforming.
	/// </summary>
	public class PlatformException:System.Exception
	{
		private cstring MessageData;
		public new cstring Message
		{
			get{
				return this.MessageData;
			}
		}
		public PlatformException ()
		{
			this.MessageData = new cstring (PlatformInfo.CurrentPlatform, "", base.Message);
		}
		public PlatformException(cstring Message)
		{
			this.MessageData = Message;
		}
	}
}

