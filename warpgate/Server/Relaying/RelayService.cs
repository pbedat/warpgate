using System;
using WebSocketSharp.Server;
using System.IO;

namespace warpgate
{
	public class RelayService: WebSocketBehavior, IRelay
	{
		

		#region IRelay implementation
		public void Send (string path, Stream stream)
		{

			base.Send ("TRANSMISSION");
			base.Send (path);
			base.Send (stream.Length.ToString());

			const int BUFFER_SIZE = 4096;

			for (var i = 0; i < stream.Length; i += BUFFER_SIZE) {
				var buffer = new byte[Math.Min (stream.Length - i, BUFFER_SIZE)];
				stream.Read (buffer, 0, buffer.Length);
				Send (buffer);
			}

			base.Send ("EOF");
		}
		#endregion
		
	}

	interface IRelay
	{
		void Send (string path, Stream stream);
	}
}

