﻿using NinjaTrader.NinjaScript;

namespace KrTrade.Nt.Services
{
    /// <summary>
    /// Implemets methods to plot in the ninjatrader charts.
    /// </summary>
    public class PlotService : NinjascriptService
    {
        public PlotService(NinjaScriptBase ninjascript) : base(ninjascript)
        {
        }

        public PlotService(NinjaScriptBase ninjascript, IPrintService printService) : base(ninjascript, printService)
        {
        }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        public override string Name => nameof(PlotService);

        internal override void Configure(out bool isConfigured)
        {
            throw new System.NotImplementedException();
        }

        internal override void DataLoaded(out bool isDataLoaded)
        {
            throw new System.NotImplementedException();
        }
    }
}
