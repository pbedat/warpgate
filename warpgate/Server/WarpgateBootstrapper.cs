using System;
using Ninject;
using Nancy;

namespace warpgate
{
	public class WarpgateBootstrapper: Nancy.DefaultNancyBootstrapper
	{
		IKernel kernel;

		public WarpgateBootstrapper (IKernel kernel)
		{
			this.kernel = kernel;
			
		}

		protected override void ApplicationStartup (Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
		{
			StaticConfiguration.DisableErrorTraces = false;
			container.Register<IKernel> (kernel);
		}
	}
}

