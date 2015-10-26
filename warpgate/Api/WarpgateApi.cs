using System;
using System.Linq;
using System.Collections.Generic;
using Ninject;
using Nancy.Hosting.Self;
using NServiceKit.Text;
using System.Net.Http;

namespace warpgate
{
	public class WarpgateApi
	{
		private IKernel kernel = new StandardKernel();

		public void Receive(HostSettings settings)
		{
			var port = settings.Port;

			Relay (settings);
			Link ("http://localhost:{0}".Fmt (settings.Port));
		}

		public void Relay(HostSettings settings)
		{
			var port = settings.Port;

			var host = new NancyHost (new Uri ("http://localhost:{0}".Fmt (port)), new WarpgateBootstrapper (kernel));
			host.Start ();

			var relayServer = new RelayServer ();
			relayServer.Start ();
			kernel.Bind<IRelayServer> ().ToConstant (relayServer);
		}

		public string Link(string relayUri)
		{
			var httpClient = new HttpClient ();
			var registerPost = httpClient.PostAsync (relayUri + "/warpgates", new StringContent(string.Empty));
			registerPost.Wait ();

			var read = registerPost.Result.Content.ReadAsStringAsync ();
			read.Wait ();
			var uid = read.Result;

			var listener = new RelayListener (uid);
			listener.Listen ();

			return uid;
		}

		public IEnumerable<string> Send(FileTransmission transmission)
		{
			return kernel.Get<SendFile>().Process (transmission).ToArray();
		}
	}
}

