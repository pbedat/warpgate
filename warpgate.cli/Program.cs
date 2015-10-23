using System;
using System.Linq;
using Nancy.Hosting.Self;
using NServiceKit.Text;

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
				Host (args.Skip (1).ToArray());
				return;
			case "cp":
				Copy (args.Skip (1).ToArray());
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

			using (var host = new NancyHost (new Uri ("http://localhost:{0}".Fmt (port)), new WarpgateBootstrapper ())) 
			{
				Console.WriteLine ("launching warpgate on http://localhost:{0}", port);
				host.Start ();
				Console.WriteLine ("press ENTER to quit");
				Console.ReadLine ();
			}

		}

		static void Copy (string[] args)
		{
			var host = args [0];
			var path = args [1];

			var transmission = new FileTransmission{ BaseUrl = host, Path = path };

			new SendFile ().Process (transmission).ToArray().PrintDump();
		}
	}
}
