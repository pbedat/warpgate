using System;
using Nancy;
using Ninject;

namespace warpgate
{
	public class RegistrarModule: NancyModule
	{
		public RegistrarModule (IKernel kernel)
		{
			Post ["/warpgates"] = _ => {
				var uid = kernel.Get<IRelayServer>().Register();

				return uid;
			};
		}
	}
}

