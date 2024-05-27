﻿using KrTrade.Nt.Core.Info;
using KrTrade.Nt.Core.Series;
using NinjaTrader.Data;
using System;

namespace KrTrade.Nt.Services.Series
{
    public abstract class BaseNinjascriptSeries : INinjascriptSeries // Series<T>, INinjascriptSeries
    {
        private bool _isConfigure = false;
        private bool _isDataLoaded = false;

        protected IBarsService Bars { get; private set; }

        public int Capacity { get => Info.Capacity; protected internal set { Info.Capacity = value; } }
        public int OldValuesCapacity { get => Info.Capacity; protected internal set { Info.Capacity = value; } }
        public string Name { get => string.IsNullOrEmpty(Info.Name) ? Key : Info.Name; internal set { Info.Name = value; } }
        public IBaseSeriesInfo Info { get; internal set; }
        public string Key => Info.Key;
        public SeriesType Type { get => Info.Type; protected set => Info.Type = value; }

        public bool Equals(IHasKey other) => other is IHasKey key && Key == key.Key;
        public bool Equals(ISeries other) => Equals(other as IHasKey);

        public override string ToString() => ToString(true, ": ", 0, null);
        public string ToString(int tabOrder) => ToString(
            includeOwner: true,
            separator: ": ",
            tabOrder: tabOrder,
            value: null);
        public string ToString(int tabOrder, int barsAgo) => ToString(
            includeOwner: true,
            separator: ": ",
            tabOrder: tabOrder,
            value: null);
        internal string ToString(int tabOrder, object value) => ToString(
            includeOwner: true,
            separator: ": ",
            tabOrder: tabOrder,
            value: value);
        internal string ToString(bool includeOwner, string separator, int tabOrder, object value)
        {
            string text = includeOwner ? Info.ToString(Bars.ToString()) : Info.ToString();
            string tab = string.Empty;

            if (tabOrder > 0)
                for (int i = 0; i < tabOrder; i++)
                    tab += "\t";
            text += tab;

            text += includeOwner ? Info.ToString(Bars.ToString()) : Info.ToString();

            if (value != null)
            {
                separator = string.IsNullOrEmpty(separator) ? ": " : separator;
                text += separator + value.ToString();
            }

            return text;
        }

        public void Log()
        {
            if (Bars.PrintService == null || !Bars.Options.IsLogEnable)
                return;
            Bars.PrintService.LogValue(ToString());
        }
        public void Log(int barsAgo)
        {
            if (Bars.PrintService == null || !Bars.Options.IsLogEnable)
                return;
            Bars.PrintService.LogValue(ToString());
        }

        //protected string ToLogString(string header, object value) => $"{header}: {value}";

        //public virtual string ToLogString() => ToLogString(Type.ToString(), 0, 0);
        //public virtual string ToLogString(int barsAgo) => ToLogString(Type.ToString(), barsAgo, 0);
        //public virtual string ToLogString(int barsAgo, int tabOrder) => ToLogString(Type.ToString(), barsAgo, tabOrder);
        //protected virtual string ToLogString(string header, int barsAgo) => ToLogString(header, barsAgo,0);
        //protected virtual string ToLogString(string header, int barsAgo, int tabOrder)
        //{
        //    string text = string.Empty;
        //    string tab = string.Empty;
        //    for (int i = 0; i < tabOrder; i++)
        //        tab += "\t";

        //    text += tab;

        //    if (barsAgo >= 0)
        //        text += $"{header}({Bars})[{barsAgo}]: {ToString()}";
        //    else
        //        text += $"{header}({Bars})[{barsAgo}]: 'barsAgo': {barsAgo} is out of range.";

        //    return text;

        //}

        /// <summary>
        /// Gets the difference between int.MaxValue and OldValuesCapacity.
        /// </summary>
        protected int MaxCapacity => int.MaxValue - OldValuesCapacity;
        /// <summary>
        /// Gets the maximum length of the series. Adds Capacity and OldValuesCapacity. 
        /// </summary>
        protected int MaxLength => Capacity + OldValuesCapacity;

        /// <summary>
        /// Create <see cref="BaseNinjascriptSeries"/> default instance with specified parameters.
        /// </summary>
        /// <param name="info">The series information necesary to construct it.</param>
        protected BaseNinjascriptSeries(IBarsService bars, BaseSeriesInfo info) //: base(info)
        {
            Bars = bars ?? throw new ArgumentNullException(nameof(bars));
            Info = info ?? throw new ArgumentNullException(nameof(info));
            
            Info.OldValuesCapacity = OldValuesCapacity < 1 ? Core.Series.Series.DEFAULT_OLD_VALUES_CAPACITY : OldValuesCapacity;
            Info.Capacity = Capacity <= 0 ? Core.Series.Series.DEFAULT_CAPACITY : Capacity > MaxCapacity ? MaxCapacity : Capacity;
        }

        public bool IsConfigure => _isConfigure;
        public bool IsDataLoaded => _isDataLoaded;

        public void Configure()
        {
            if (IsOutOfConfigurationStates())
                LoggingHelpers.ThrowIsNotConfigureException(Name);
            if (_isConfigure && _isDataLoaded)
                return;
            if (Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.Configure && !_isConfigure)
                Configure(out _isConfigure);

            else if (Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.DataLoaded && !_isConfigure)
            {
                Configure(out _isConfigure);
                DataLoaded(out _isDataLoaded);
            }
            else if (Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.DataLoaded && _isConfigure)
                DataLoaded(out _isDataLoaded);

        }
        public void DataLoaded()
        {
            if (Bars.Ninjascript.State != NinjaTrader.NinjaScript.State.DataLoaded)
                LoggingHelpers.ThrowIsNotConfigureException(Name);

            if (_isConfigure && _isDataLoaded)
                return;

            if (Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.DataLoaded && !_isConfigure)
                Configure(out _isConfigure);

            if (Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.DataLoaded && _isConfigure)
                DataLoaded(out _isDataLoaded);

        }
        public virtual void Terminated() => Dispose();

        public abstract void Add();
        public abstract void Update();
        public abstract void Dispose();
        public abstract void RemoveLastElement();
        public abstract void Reset();

        public virtual void BarUpdate() 
        {
            if (Bars.LastBarIsRemoved)
                RemoveLastElement();
            else if (Bars.IsClosed)
                Add();
            else if (Bars.IsPriceChanged || Bars.IsTick)
                Update();
        }
        public virtual void BarUpdate(IBarsService updatedBarsService) { }
        public virtual void MarketData(MarketDataEventArgs args) { }
        public virtual void MarketData(IBarsService updatedBarsService) { }
        public virtual void MarketDepth(MarketDepthEventArgs args) { }
        public virtual void MarketDepth(IBarsService updatedBarsService) { }

        /// <summary>
        /// Method to configure the service. This method should be used by 
        /// services that can be configured without first passing any filters.
        /// </summary>
        /// <param name="isConfigured">True, if the service has been configure, otherwise false.</param>
        internal abstract void Configure(out bool isConfigured);

        /// <summary>
        /// Method to configure the service when NinjaScript data is loaded. This method should be used by 
        /// services that can be configured without first passing any filters.
        /// </summary>
        /// <param name="isDataLoaded">True, if the service has been configure, otherwise false.</param>
        internal abstract void DataLoaded(out bool isDataLoaded);

        #region Helper methods

        /// <summary>
        /// Indicates whether NinjaScript is in any of the configuration states.
        /// The configuaration states are 'Configure' and 'DataLoaded'.
        /// </summary>
        /// <returns></returns>
        protected bool IsInConfigurationStates()
        {
            if (Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.Configure || Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.DataLoaded)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript is in running states.
        /// The running states are 'Historical' and 'Realtime'.
        /// </summary>
        /// <returns>True, when the NijaScript State is 'Historical' or 'Realtime'.</returns>
        protected bool IsInRunningStates()
        {
            if (Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.Historical || Bars.Ninjascript.State == NinjaTrader.NinjaScript.State.Realtime)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript is out of the configuration states.
        /// The configuaration states are 'Configure' and 'DataLoaded'.
        /// </summary>
        /// <returns>True, when the NijaScript State is NOT 'Configure' and 'DataLoaded'.</returns>
        protected bool IsOutOfConfigurationStates()
        {
            if (Bars.Ninjascript.State != NinjaTrader.NinjaScript.State.Configure && Bars.Ninjascript.State != NinjaTrader.NinjaScript.State.DataLoaded)
                return true;

            return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript is out of the configure state.
        /// The configure state is 'Configure'.
        /// </summary>
        /// <returns>True, when the NijaScript State is NOT 'Configure'.</returns>
        protected bool IsOutOfConfigureState()
        {
            if (Bars.Ninjascript.State != NinjaTrader.NinjaScript.State.Configure)
                return true;

            return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript is out of the data loaded state.
        /// The data loaded state is 'DataLoaded'.
        /// </summary>
        /// <returns>True, when the NijaScript State is NOT 'DataLoaded'.</returns>
        protected bool IsOutOfDataLoadedState()
        {
            if (Bars.Ninjascript.State != NinjaTrader.NinjaScript.State.DataLoaded)
                return true;

            return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript is out of the running states.
        /// The running states are 'Historical' and 'Realtime'.
        /// </summary>
        /// <returns>True, when the NijaScript State is NOT 'Historical' and 'Realtime'.</returns>
        protected bool IsOutOfRunningStates()
        {
            if (Bars.Ninjascript.State != NinjaTrader.NinjaScript.State.Historical && Bars.Ninjascript.State != NinjaTrader.NinjaScript.State.Realtime)
                return true;

            return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript indexes are available.
        /// The indexs are 'BarsInProgress' and 'CurrentBar'.
        /// </summary>
        /// <returns>True, when the NijaScript indexes are greater than -1.</returns>
        protected bool IsNinjaScriptIndexesAvailable()
        {
            if (IsNotAvilableBarsInProgressIdx() && IsNotAvailableFirstBarIdx())
                return true;

            return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript data series is available to be updated.
        /// The index is 'CurrentBar'.
        /// </summary>
        /// <returns>True, when the NijaScript index are greater than -1.</returns>
        protected bool IsNotAvailableFirstBarIdx()
        {
            if (Bars.Ninjascript.CurrentBars[Bars.Ninjascript.BarsInProgress] < 0)
                return true;

            return false;
        }
        /// <summary>
        /// Indicates whether NinjaScript multi data series is available to be updated.
        /// The index is 'BarsInProgress'.
        /// </summary>
        /// <returns>True, when the NijaScript index are greater than -1.</returns>
        protected bool IsNotAvilableBarsInProgressIdx()
        {
            if (Bars.Ninjascript.BarsInProgress < 0)
                return true;

            return false;
        }

        #endregion
    }
}
