using System;
using System.Net.Http;
using System.IO;
using NServiceKit.Text;
using System.Collections.Generic;

namespace warpgate
{
	public class SendFile
	{
		public IEnumerable<string> Process(FileTransmission transmission)
		{
			var attr = File.GetAttributes(transmission.Path);

			return attr.HasFlag (FileAttributes.Directory)
				? SendDirectory (transmission)
				: new []{Send (transmission)};
		}

		private IEnumerable<string> SendDirectory(FileTransmission transmission)
		{
			foreach (var path in Directory.GetFiles(transmission.Path, "*.*", SearchOption.AllDirectories)) 
			{
				var attr = File.GetAttributes (path);

				if(!attr.HasFlag(FileAttributes.Directory))
					yield return Send (new FileTransmission{ BaseUrl = transmission.BaseUrl, Path = path }, transmission.Path);
			}
		}

		private string Send(FileTransmission transmission, string basePath = null)
		{
			var path = basePath == null
				? Path.GetFileName(transmission.Path)
				: transmission.Path.Replace(basePath, string.Empty);

			var uri = new Uri ("{0}/warpgate/io/{1}".Fmt(transmission.BaseUrl, path));

			using (var file = File.OpenRead (transmission.Path)) {

				var content = new StreamContent (file);

				using (var client = new HttpClient ()) {
					var upload = client.PutAsync (uri, content);

					upload.Wait ();

					var readContent = upload.Result.Content.ReadAsStringAsync ();

					readContent.Wait ();

					return readContent.Result;
				}
			}
		}
	}
}

