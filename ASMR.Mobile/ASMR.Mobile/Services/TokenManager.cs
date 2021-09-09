using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ASMR.Mobile.Services
{
    public static class TokenManager
    {
        private static string _sharedName = "ASMR.Mobile";
        
        public static Task<string> GetAsync(string name)
        {
            try
            {
                return SecureStorage.GetAsync(name);
            }
            catch (Exception)
            {
                return Task.FromResult(Preferences.Get(name, null, _sharedName));
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
                Preferences.Set(name, value, _sharedName);
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
                var hasToken = Preferences.ContainsKey(name, _sharedName);
                if (hasToken)
                {
                    Preferences.Remove(name, _sharedName);
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
                Preferences.Clear(_sharedName);
            }
        }
    }
}
