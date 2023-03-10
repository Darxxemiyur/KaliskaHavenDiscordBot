using DisCatSharp;
using DisCatSharp.EventArgs;

using System.Reflection;

namespace KaliskaHaven.DiscordClient
{
    /// <summary>
    /// Basic JANVIS interface for its modules
    /// </summary>
    public interface IKaliskaBot
    {
        Task<DisCatSharp.DiscordClient> GetClient();

        Task<DCEventRouter<DisCatSharp.DiscordClient, TEvent>> GetEventRouter<TEvent>() where TEvent : DiscordEventArgs;

        Task<DCEventRouter<DisCatSharp.DiscordClient, TEvent>> GetEventRouter<TEvent>(Func<EventInfo, bool> selector) where TEvent : DiscordEventArgs;


        /// <summary>
        /// Tools for interactivity.
        /// </summary>
        /// <returns></returns>
        Task GetInteractivity();

        /// <summary>
        /// Unfiltered events.
        /// </summary>
        /// <returns></returns>
        Task GetHandling();

        Task<IMyServices> GetServices();
    }
}