using PCLStorage;
using Spectator.Core.Model.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectator.Core.Model.Database
{
    public class PreferenceCookieStorage : Account.Account.IStorage, IAuthProvider
    {
        const string SettingsFile = "cookies.dat";

        public async Task<IDictionary<string, string>> Load()
        {
            if ((await GetStorage().CheckExistsAsync(SettingsFile)) == ExistenceCheckResult.FileExists)
            {
                var file = await GetStorage().GetFileAsync(SettingsFile);
                var text = await file.ReadAllTextAsync();
                return text.Split(';').Select(s => s.Split('=')).ToDictionary(s => s[0], s => s[1]);
            }
            return new Dictionary<string, string>();
        }

        public async void ReplaceAll(IEnumerable<AccountCookie> cookies)
        {
            var text = new StringBuilder();
            foreach (var s in cookies)
            {
                if (text.Length > 0) text.Append(";");
                text.Append(s.Name).Append("=").Append(s.Value);
            }
            var file = await GetStorage().CreateFileAsync(SettingsFile, CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(text.ToString());
        }

        private static IFolder GetStorage()
        {
            return FileSystem.Current.LocalStorage;
        }
    }
}