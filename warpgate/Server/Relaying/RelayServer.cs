using System;
using WebSocketSharp.Server;
using System.Collections.Generic;
using System.IO;

namespace warpgate
{
	public class RelayServer: IRelayServer, IDisposable
	{
		WebSocketServer socketServer;
		Dictionary<string, IRelay> relays = new Dictionary<string, IRelay>();

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
				relays.Add(uid, relay);
				return relay;
			});

			return uid;
		}

		public void Relay(string path, Stream stream)
		{
			foreach (var relay in relays.Values)
				relay.Send (path, stream);
		}

		public void Relay(string uid, string path, Stream stream)
		{
			var relay = relays [uid];

			if(relay != null)
				relay.Send (path, stream);
			else {
				Console.WriteLine ("relay {0} is not registered", uid);
			}
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
		void Relay(string uid, string path, Stream body);
	}
}

