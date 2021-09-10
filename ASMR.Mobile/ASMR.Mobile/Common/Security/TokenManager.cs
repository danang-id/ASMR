using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ASMR.Mobile.Common.Security
{
    public static class TokenManager
    {
        private const string SharedName = "ASMR.Mobile";

        public static Task<string> GetAsync(string name)
        {
            try
            {
                return SecureStorage.GetAsync(name);
            }
            catch (Exception)
            {
                return Task.FromResult(Preferences.Get(name, null, SharedName));
            }
        }

        public static Task SetAsync(string name, string value)
        {
            try
            {
                return SecureStorage.SetAsync(name, value);
            }
            catch (Exception)
            {
                Preferences.Set(name, value, SharedName);
                return Task.CompletedTask;
            }
        }

        public static bool Remove(string name)
        {
            try
            {
                return SecureStorage.Remove(name);
            }
            catch (Exception)
            {
                var hasToken = Preferences.ContainsKey(name, SharedName);
                if (hasToken)
                {
                    Preferences.Remove(name, SharedName);
                }

                return hasToken;
            }
        }

        public static void RemoveAll()
        {
            try
            {
                SecureStorage.RemoveAll();
            }
            catch (Exception)
            {
                Preferences.Clear(SharedName);
            }
        }
    }
}
