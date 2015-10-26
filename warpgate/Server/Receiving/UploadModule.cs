using System;
using Ninject;
using Nancy;
using System.IO;

namespace warpgate
{
	public class UploadModule: NancyModule
	{
		public UploadModule (IKernel kernel)
		{
			Put ["warpgate/io/{path*}"] = _ => {


				var relativePath = (string)_.path;

				kernel.Get<IRelayServer>().Relay(relativePath, Request.Body);

				return "OK";
			};

			Put ["{uid}/warpgate/io/{path*}"] = _ => {


				var relativePath = (string)_.path;
				var uid = (string)_.uid;

				kernel.Get<IRelayServer>().Relay(uid, relativePath, Request.Body);

				return "OK";
			};
		}
	}
}

