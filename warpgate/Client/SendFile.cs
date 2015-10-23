using System;
using System.Net.Http;
using System.IO;
using NServiceKit.Text;

namespace warpgate
{
	public class SendFile
	{
		public void Process(FileTransmission transmission)
		{
			var uri = new Uri ("{0}/warpgate/io/{1}".Fmt(transmission.BaseUrl, Path.GetFileName(transmission.Path)));

			using (var file = File.OpenRead (transmission.Path)) {

				var content = new StreamContent (file);

				using (var client = new HttpClient ()) {
					var upload = client.PutAsync (uri, content);

					upload.Wait ();
				}
			}
		}
	}
}

