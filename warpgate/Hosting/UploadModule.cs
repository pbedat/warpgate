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

				var absolutePath = Path.Combine(Environment.CurrentDirectory, relativePath);

				using(var file = File.OpenWrite(absolutePath))
					Request.Body.CopyTo(file);

				return absolutePath;
			};
		}
	}
}

