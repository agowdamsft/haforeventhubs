using System;
using System.Collections.Generic;
using System.Text;

namespace EventhubConsumer
{
    internal sealed class GeoDrWatch
    {
        private GeoDrWatch() { }

        private static GeoDrWatch instance;
        internal static GeoDrWatch Instance
        {
            get
            {
                // not thread safe
                return instance ?? (instance = new GeoDrWatch());
            }
        }

        internal event EventHandler OnFailover;

        internal void InitiateFailover(object sender)
        {
            OnFailover?.Invoke(sender, EventArgs.Empty);
        }

        internal static bool IsGeoDRException(Exception ex)
        {
            var unauthorized = ex as UnauthorizedAccessException;
            return (unauthorized != null && unauthorized.HResult == -2147024891);
            // message contains GeoDRFailOver
        }
    }
}
