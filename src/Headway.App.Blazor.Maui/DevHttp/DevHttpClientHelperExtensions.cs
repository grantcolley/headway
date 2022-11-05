using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Security;

namespace Headway.App.Blazor.Maui.DevHttp
{
    public static class DevHttpClientHelperExtensions
    {
        /// <summary>
        /// Adds the <see cref="IHttpClientFactory"/> and related services to the <see cref="IServiceCollection"/> and configures
        /// a named <see cref="HttpClient"/> to use localhost or 10.0.2.2 and bypass certificate checking on Android.
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="sslPort">Development server port</param>
        /// <returns>The IServiceCollection</returns>
        /// <remarks>
        /// <para>
        /// https://github.com/dotnet/maui/discussions/8131
        /// </para>
        /// <para>
        /// https://gist.github.com/Eilon/49e3c5216abfa3eba81e453d45cba2d4
        /// by https://gist.github.com/Eilon
        /// </para>
        /// <para>
        /// https://gist.github.com/EdCharbeneau/ed3d44d8298319c201f276de7a0580f1
        /// by https://gist.github.com/EdCharbeneau
        /// </para>
        /// </remarks>
        public static IServiceCollection AddDevHttpClient(this IServiceCollection services, string name, int sslPort)
        {
            var devServerRootUrl = new UriBuilder("https", DevServerName, sslPort).Uri.ToString();

#if WINDOWS
            services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new UriBuilder("https", DevServerName, sslPort).Uri;
            }); 
            
            return services;
#endif

#if ANDROID
            services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new UriBuilder("https", DevServerName, sslPort).Uri;
            })
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    var handler = new CustomAndroidMessageHandler();
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        if (cert != null && cert.Issuer.Equals("CN=localhost"))
                            return true;
                        return errors == SslPolicyErrors.None;
                    };
                    return handler;
                });

            return services;

#else
        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif
        }

        public static string DevServerName =>
#if WINDOWS
        "localhost";
#elif ANDROID
        "10.0.2.2";
#else
        throw new PlatformNotSupportedException("Only Windows and Android currently supported.");
#endif

#if ANDROID
        internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
        {
            protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
                => new CustomHostnameVerifier();

            private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
            {
                public bool Verify(string hostname, Javax.Net.Ssl.ISSLSession session)
                {
                    return
                        Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
                        || hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
                }
            }
        }
#endif
    }
}
