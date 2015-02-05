using System;

class example<T,I>
{
	public T value;
	public class buffer:I
	{
		public string filetype;
		buffer()
		{
			filetype=new string("");
		}
	}
	public example<T,I>.buffer image;
	example()
	{
		value=new T();
		image=new example<T,I>.buffer();
	}
}

public class main
{
	static int Main(string[] args)
	{
		example<int,System.IO.FileStream> ex=new example<int,object>();
		ex.image.filetype = "img/png";
		System.IO.FileStream fs = new System.IO.FileStream ("example.png", System.IO.FileMode.Open);
		ex.image = fs.Name.Contains (".png") ? fs : null;
		try{
			System.IO.StreamReader sr=new System.IO.StreamReader(ex.image);
		}
		catch(ArgumentNullException ae){
			Console.Write (ae.Message+"\nFile:\""+ex.image.Name+"\" Not Found.\n");
		}
		return 0;
	}
}
