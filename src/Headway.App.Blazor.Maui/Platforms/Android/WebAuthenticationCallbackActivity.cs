using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Headway.App.Blazor.Maui.Platforms.Android
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
    [IntentFilter(new[] { "android.intent.action.VIEW" },
                  Categories = new[] {
                      "android.intent.category.DEFAULT",
                      "android.intent.category.BROWSABLE"
                  },
                  DataScheme = CALLBACK_SCHEME)]
    public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
        const string CALLBACK_SCHEME = "myapp";
    }
}
