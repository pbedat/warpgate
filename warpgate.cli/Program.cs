using Fclp;
using System;
using System.Linq;
using Nancy.Hosting.Self;
using NServiceKit.Text;
using Ninject;
using System.Net.Http;
using System.Collections.Generic;

namespace warpgate.cli
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var commands = new Dictionary<string, Action<string[]>> {
				{ "receive", Receive },
				{ "cp", Copy },
				{ "link", Link },
				{ "relay", Relay }
			};

			if (!args.Any () || !commands.Keys.Contains(args.First())) {
				Console.WriteLine ("available commands: ");
				commands.Keys.ToList ().ForEach (Console.WriteLine);
				return;
			}

			commands [args.First ()](args.Skip(1).ToArray());
		}

		static void Receive (string[] args)
		{
			var settings = ParseHostSettings (args);

			if (settings == null)
				return;

			var api = new WarpgateApi ();

			Console.WriteLine ("launching warpgate on http://localhost:{0}", settings.Port);

			api.Receive (settings);

			Console.WriteLine ("press ENTER to quit");
			Console.ReadLine ();
		}

		static void Copy (string[] args)
		{
			var host = args [0];
			var path = args [1];

			var transmission = new FileTransmission{ BaseUrl = host, Path = path };

			new WarpgateApi ().Send (transmission).ToArray ().PrintDump ();
		}

		static void Link (string[] args)
		{
			var relay = args [0];

			var uid = new WarpgateApi ().Link (relay);

			Console.WriteLine ("listening to relay {0}", relay);
			Console.WriteLine ("Use the following UID to send files over here:");
			Console.WriteLine (uid);

			Console.WriteLine ("press ENTER to quit");
			Console.ReadLine ();
		}

		static void Relay (string[] args)
		{
			var settings = ParseHostSettings (args);

			if (settings == null)
				return;

			var api = new WarpgateApi ();

			Console.WriteLine ("launching warpgate relay on http://localhost:{0}", settings.Port);

			api.Relay (settings);

			Console.WriteLine ("press ENTER to quit");
			Console.ReadLine ();
		}

		static HostSettings ParseHostSettings(string[] args)
		{
			var parser = new FluentCommandLineParser<HostSettings> ();

			parser.Setup (hs => hs.Port)
				.As ('p', "port")
				.SetDefault (8080);

			var result = parser.Parse (args);

			if (result.HasErrors) {
				Console.WriteLine (result.ErrorText);
				return null;
			}

			return parser.Object;
		}
	}
}
