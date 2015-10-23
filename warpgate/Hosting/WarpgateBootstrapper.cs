using System;
using Ninject;

namespace warpgate
{
	public class WarpgateBootstrapper: Nancy.DefaultNancyBootstrapper
	{
		protected override void ApplicationStartup (Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
		{
			var kernel = new StandardKernel ();

			container.Register<IKernel> (kernel);
		}
	}
}

