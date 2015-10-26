using System;
using WebSocketSharp;
using NServiceKit.Text;
using System.IO;

namespace warpgate
{
	public class RelayListener: IDisposable
	{
		WebSocket socket;

		string state = "IDLE";
		string path = "";
		Stream stream = null;

		public RelayListener (string uid)
		{
			socket = new WebSocket ("ws://localhost:8081/{0}".Fmt (uid));

			socket.OnMessage += OnMessage;
		}

		private void OnMessage(object sender, MessageEventArgs e)
		{
			if(e.Data == "TRANSMISSION")
			{
				state = "TRANSMISSION";
			}
			else if(e.Data == "EOF")
			{
				state = "IDLE";
				stream.Flush();
				stream.Close();
				stream = null;
			}
			else if(state == "TRANSMISSION")
			{
				path = e.Data;
				Console.WriteLine (path);
				var relativePath = path;

				var absolutePath = Path.Combine(Environment.CurrentDirectory, relativePath);

				var directory = Path.GetDirectoryName(absolutePath);

				if(!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				stream = File.OpenWrite(absolutePath);
				state = "PATH";
			}
			else if(state == "PATH")
			{
				state = "BINARY";
			}
			else if(state == "BINARY")
			{
				stream.Write(e.RawData,0, e.RawData.Length);
			}

		}

		public void Listen()
		{
			socket.Connect ();
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			socket.Close ();
		}

		#endregion
	}
}

