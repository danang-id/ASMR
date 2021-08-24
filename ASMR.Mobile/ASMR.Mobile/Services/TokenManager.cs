using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ASMR.Mobile.Services
{
    public static class TokenManager
    {
        public static Task<string> GetAsync(string name)
        {
            try
            {
                return SecureStorage.GetAsync(name);
            }
            catch (Exception)
            {
                return Task.FromResult(Preferences.Get(name, null));
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
                Preferences.Set(name, value);
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
                var hasToken = Preferences.ContainsKey(name);
                if (hasToken)
                {
                    Preferences.Remove(name);
                }

                return hasToken;
            }
        }
    }
}
