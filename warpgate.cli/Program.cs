using System;
using System.Linq;
using Nancy.Hosting.Self;
using NServiceKit.Text;
using Ninject;
using System.Net.Http;

namespace warpgate.cli
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			args = args.Any () 
				? args
				: new []{ "host" };

			switch (args [0]) {
			case "host":
				Host (args.Skip (1).ToArray ());
				return;
			case "cp":
				Copy (args.Skip (1).ToArray ());
				return;
			case "link":
				Link (args.Skip (1).ToArray ());
				return;
			case "relay":
				Relay (args.Skip (1).ToArray ());
				return;
			}
		}

		static void Host (string[] args)
		{
			var portArgs = args.SkipWhile (arg => arg != "--port" || arg != "-p");

			var ports = portArgs.Any ()
				? portArgs.Skip (1)
				: portArgs;

			var port = int.Parse(ports.FirstOrDefault () ?? "8080");

			var kernel = new StandardKernel ();

			using (var host = new NancyHost (new Uri ("http://localhost:{0}".Fmt (port)), new WarpgateBootstrapper (kernel))) 
			{
				Console.WriteLine ("launching warpgate on http://localhost:{0}", port);
				host.Start ();

				using(var relayServer = new RelayServer ())
				{
					var uid = relayServer.Register ();
					relayServer.Start ();
					kernel.Bind<IRelayServer> ().ToConstant (relayServer);

					using (var listener = new Listener (uid)) 
					{
						listener.Listen ();


						Console.WriteLine ("press ENTER to quit");
						Console.ReadLine ();
					}
				}

			}

		}

		static void Copy (string[] args)
		{
			var host = args [0];
			var path = args [1];

			var transmission = new FileTransmission{ BaseUrl = host, Path = path };

			new SendFile ().Process (transmission).ToArray().PrintDump();
		}

		static void Link (string[] args)
		{
			var relay = args [0];

			using (var httpClient = new HttpClient ()) {
			
				var registerPost = httpClient.PostAsync (relay + "/warpgates", new StringContent(string.Empty));
				registerPost.Wait ();

				var read = registerPost.Result.Content.ReadAsStringAsync ();
				read.Wait ();
				var uid = read.Result;

				using (var listener = new Listener (uid)) {
					listener.Listen ();

					Console.WriteLine ("listening to relay {0}", relay);

					Console.WriteLine ("press ENTER to quit");
					Console.ReadLine ();
				}
			}
		}

		static void Relay (string[] args)
		{
			var portArgs = args.SkipWhile (arg => arg != "--port" || arg != "-p");

			var ports = portArgs.Any ()
				? portArgs.Skip (1)
				: portArgs;

			var kernel = new StandardKernel ();

			var port = int.Parse(ports.FirstOrDefault () ?? "8080");

			using (var host = new NancyHost (new Uri ("http://localhost:{0}".Fmt (port)), new WarpgateBootstrapper (kernel))) 
			{
				Console.WriteLine ("launching warpgate on http://localhost:{0}", port);
				host.Start ();

				using(var relayServer = new RelayServer ())
				{
					Console.WriteLine ("launching relay");
					relayServer.Start ();
					kernel.Bind<IRelayServer> ().ToConstant (relayServer);

					Console.WriteLine ("press ENTER to quit");
					Console.ReadLine ();
				}

			}

		}
	}
}
