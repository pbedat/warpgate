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

				var directory = Path.GetDirectoryName(absolutePath);

				if(!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				using(var file = File.OpenWrite(absolutePath))
					Request.Body.CopyTo(file);

				return absolutePath;
			};
		}
	}
}

