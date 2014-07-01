using System;
using System.Linq;
using System.Net;
using PCLStorage;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace Spectator.Core.Model.Web
{
	/// <summary>
	/// Хранение куков на диске
	/// </summary>
	public class PersistenCookieContainer
	{
		private const string CookieFileName = "cookies.v1.bin";

		public CookieContainer Cookies { get; private set; }

		private Dictionary<string, CookieCollection> cookieMap;

		private PersistenCookieContainer ()
		{
		}

		public static PersistenCookieContainer LoadFromFileOrCreateEmpty ()
		{
			var c = new CookieContainer ();
			var map = new Dictionary<string, CookieCollection> ();
			var dir = FileSystem.Current.LocalStorage;

			if (dir.CheckExistsAsync (CookieFileName).Result == ExistenceCheckResult.FileExists) {
				var t = dir.GetFileAsync (CookieFileName).Result.ReadAllTextAsync ().Result.Split ('\u0000');

				for (int i = 0; i < t.Length - 2; i += 3) {
					var u = t [i];
					var k = new Cookie (t [i + 1], t [i + 2]);
					c.Add (new Uri (u), k);
					if (!map.ContainsKey (u))
						map [u] = new CookieCollection ();
					map [u].Add (k);
				}
			}

			return new PersistenCookieContainer { Cookies = c, cookieMap = map };
		}

		public Task FlushToDiskAsync (string newUrl)
		{
			return Task.Run (() => {
				lock (this) {
					var nc = Cookies.GetCookies (new Uri (newUrl));
					if (nc == null || nc.Count < 1)
						return;
					if (cookieMap.ContainsKey (newUrl))
						return;
					cookieMap [newUrl] = nc;
			
					var text = new StringBuilder ();
					foreach (var u in cookieMap) {
						foreach (var c in u.Value.Cast<Cookie>()) {
							text.Append (u.Key).Append ('\u0000');
							text.Append (c.Name).Append ('\u0000');
							text.Append (c.Value).Append ('\u0000');
						}
					}
			
					var dir = FileSystem.Current.LocalStorage;
					dir.CreateFileAsync (CookieFileName, CreationCollisionOption.ReplaceExisting).Result
									.WriteAllTextAsync (text.ToString ()).Wait ();
				}
			});
		}
	}
}