﻿namespace KrTrade.Nt.Core.Bars
{
    /// <summary>
    /// Represents the bars log levels.
    /// </summary>
    public enum BarsLogLevel
    {
        /// <summary>
        /// Represents all ticks of the bars.
        /// </summary>
        Tick,

        /// <summary>
        /// Represents the ticks when the price changed.
        /// </summary>
        PriceChanged,

        /// <summary>
        /// Represents the ticks when the last bar closed.
        /// </summary>
        BarClosed,

        /// <summary>
        /// Does not represent any level.
        /// </summary>
        None
    }
}
