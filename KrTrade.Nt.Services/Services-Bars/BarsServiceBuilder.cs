﻿using KrTrade.Nt.Core.Data;
using System;
using System.Collections.Generic;

namespace KrTrade.Nt.Services
{
    /// <summary>
    /// Build <see cref="BarsService"/> objects. 
    /// </summary>
    public class BarsServiceBuilder : IBarsServiceBuilder
    {

        private readonly List<Action<BarsServiceOptions>> _optionsDelegateActions = new List<Action<BarsServiceOptions>>();
        //private readonly Dictionary<BaseSeriesInfo, SeriesOptions> _seriesConfiguration = new Dictionary<BaseSeriesInfo,SeriesOptions>();
        private readonly Dictionary<string,BaseSeriesInfo> _seriesConfiguration = new Dictionary<string,BaseSeriesInfo>();
        //private readonly List<BaseSeriesInfo> _seriesConfiguration = new List<BaseSeriesInfo>();

        public IBarsServiceBuilder ConfigureOptions(Action<BarsServiceOptions> configureBarsServiceOptions)
        {
            _optionsDelegateActions.Add(configureBarsServiceOptions ?? throw new ArgumentNullException(nameof(configureBarsServiceOptions)));
            return this;
        }

        public IBarsServiceBuilder AddSeries<TInfo>(Action<TInfo> configureSeries)
            where TInfo : BaseSeriesInfo, new()
        {
            if (configureSeries == null)
                throw new ArgumentNullException(nameof(configureSeries));

            TInfo seriesInfo = new TInfo();
            configureSeries(seriesInfo);

            if (!_seriesConfiguration.ContainsKey(seriesInfo.GetKey()))
                _seriesConfiguration.Add(seriesInfo.GetKey(), seriesInfo);

            return this;
        }
        public IBarsServiceBuilder AddSeries(Action<SeriesInfo> configureSeries)
        {
            SeriesInfo seriesInfo = new SeriesInfo();
            SeriesOptions seriesOptions = new SeriesOptions();
            configureSeries(seriesInfo);

            if (!_seriesConfiguration.ContainsKey(seriesInfo.GetKey()))
                _seriesConfiguration.Add(seriesInfo.GetKey(), seriesInfo);

            return this;
        }
        //public IBarsServiceBuilder AddSeries<TInfo,TOptions>(Action<TInfo, TOptions> configureSeries)
        //    where TInfo : BaseSeriesInfo, new()
        //    where TOptions : SeriesOptions, new()
        //{
        //    if (configureSeries == null)
        //        throw new ArgumentNullException(nameof(configureSeries));

        //    TInfo seriesInfo = new TInfo();
        //    TOptions seriesOptions = new TOptions();
        //    configureSeries(seriesInfo, seriesOptions);

        //    if (!_seriesConfiguration.ContainsKey(seriesInfo))
        //        _seriesConfiguration.Add(seriesInfo, seriesOptions);

        //    return this;
        //}
        //public IBarsServiceBuilder AddSeries<TInfo>(Action<TInfo, SeriesOptions> configureSeries)
        //    where TInfo : BaseSeriesInfo, new()
        //{
        //    TInfo seriesInfo = new TInfo();
        //    SeriesOptions seriesOptions = new SeriesOptions();
        //    configureSeries(seriesInfo, seriesOptions);

        //    if (!_seriesConfiguration.ContainsKey(seriesInfo))
        //        _seriesConfiguration.Add(seriesInfo, seriesOptions);

        //    return this;
        //}
        //public IBarsServiceBuilder AddSeries(Action<SeriesInfo,SeriesOptions> configureSeries)
        //{
        //    SeriesInfo seriesInfo = new SeriesInfo();
        //    SeriesOptions seriesOptions = new SeriesOptions();
        //    configureSeries(seriesInfo, seriesOptions);

        //    if (!_seriesConfiguration.ContainsKey(seriesInfo))
        //        _seriesConfiguration.Add(seriesInfo, seriesOptions);

        //    return this;
        //}

        public IBarsService Build(IBarsManager barsManager, DataSeriesInfo dataSeriesInfo)
        {

            string logText = string.Empty;

            // Create service options
            BarsServiceOptions options = new BarsServiceOptions();

            foreach (var action in _optionsDelegateActions)
                action(options);

            // Create the service with specified info
            IBarsService barsService = new BarsService(barsManager, dataSeriesInfo, options);

            // Log trace
            if (barsService != null)
                logText = $"{barsService.Name} has been created succesfully.";
            else
                logText = "BarsService has NOT been created. The value is NULL.";
            barsManager.PrintService.LogTrace(logText);

            // Add SERIES
            foreach (var series in _seriesConfiguration)
                barsService.AddSeries(series.Value);

            return barsService;
        }

        //public IDataSeriesBuilder AddIndicators(Action<IIndicatorsBuilder> configureIndicatorsDelegate)
        //{
        //    throw new NotImplementedException();
        //}

        //public IDataSeriesBuilder AddFilters()
        //{
        //    throw new NotImplementedException();
        //}

        //public IDataSeriesBuilder UseStats()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
