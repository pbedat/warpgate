using System;
using WebSocketSharp.Server;
using System.Collections.Generic;
using System.IO;

namespace warpgate
{
	public class RelayServer: IRelayServer, IDisposable
	{
		WebSocketServer socketServer;
		List<IRelay> relays = new List<IRelay>();

		public RelayServer ()
		{
			socketServer = new WebSocketServer ("ws://localhost:8081");
		}

		public void Start()
		{
			socketServer.Start ();
		}

		public string Register()
		{
			var uid = Guid.NewGuid ().ToString ();
			socketServer.AddWebSocketService<RelayService> ("/" + uid, () => {
				var relay = new RelayService();
				relays.Add(relay);
				return relay;
			});

			return uid;
		}

		public void Relay(string path, Stream stream)
		{
			foreach (var relay in relays)
				relay.Send (path, stream);
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			socketServer.Stop ();
		}

		#endregion
	}

	public interface IRelayServer {
		string Register ();
		void Relay(string path, Stream body);
	}
}

