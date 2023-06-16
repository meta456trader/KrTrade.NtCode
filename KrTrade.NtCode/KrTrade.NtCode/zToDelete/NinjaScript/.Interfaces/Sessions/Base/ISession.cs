﻿using KrTrade.NtCode.Events;

namespace KrTrade.NtCode
{

    /// <summary>
    /// Interface for any session script.
    /// </summary>
    public interface ISession : INinjascript
    {
        /// <summary>
        /// Event driven method which is called for every new session. 
        /// </summary>
        /// <param name="e"></param>
        void OnSessionChanged(SessionUpdateArgs e);

    }

}
