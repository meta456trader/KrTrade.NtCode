﻿using NinjaTrader.NinjaScript;
using System;
using System.Diagnostics;
using System.Text;

namespace KrTrade.Nt.Core
{

    public abstract class BaseElement<TType> : BaseInfoScript<TType, IInfo<TType>>, IElement<TType, IInfo<TType>>
        where TType : Enum
    {
        private readonly IPrintService _printService;
        //private readonly IInfo<TType> _info;
        private bool _isConfigure = false;
        private bool _isDataLoaded = false;

        public IPrintService PrintService => _printService;
        public bool IsConfigure => _isConfigure;
        public bool IsDataLoaded => _isDataLoaded;

        //private TType _type;
        //public override TType Type
        //{
        //    get => _type;
        //    protected set
        //    {
        //        base.Type = value.ToElementType();
        //        _type = value;
        //    }
        //}

        //public override TType Type { get => Info.Type; protected set => Info.Type = value; }
        public override string Name => string.IsNullOrEmpty(Info.Name) ? Key : Info.Name;
        public string Key => Info.Key;

        public void Configure()
        {

            if (IsOutOfConfigurationStates())
                LoggingHelpers.ThrowIsNotConfigureException(Name);

            if (_isConfigure && _isDataLoaded)
                return;

            if (Ninjascript.State == State.Configure && !_isConfigure)
                Configure(out _isConfigure);

            else if (Ninjascript.State == State.DataLoaded && !_isConfigure)
                Configure(out _isConfigure);

            //else if (Ninjascript.State == State.DataLoaded && _isConfigure)
            //    DataLoaded(out _isDataLoaded);

            LogConfigurationState();
        }
        public void DataLoaded()
        {

            // TODO: Delete this line
            Debugger.Break();

            if (Ninjascript.State != State.DataLoaded)
                LoggingHelpers.ThrowIsNotConfigureException(Name);

            if (_isConfigure && _isDataLoaded)
                return;

            if (Ninjascript.State == State.DataLoaded && !_isConfigure)
                Configure(out _isConfigure);

            if (Ninjascript.State == State.DataLoaded && _isConfigure)
                DataLoaded(out _isDataLoaded);

            LogConfigurationState();
        }
        public virtual void Terminated() { }

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

        public override bool Equals(object obj) => obj is IElement<TType> other && this == other;
        public bool Equals(IElement<TType> other) => other != null && this == other;
        public override int GetHashCode() => Info.Key.GetHashCode();

        public static bool operator ==(BaseElement<TType> element1, IElement<TType> element2) =>
            (element1 is null && element2 is null) ||
            (!(element1 is null) && !(element2 is null) && element1.Key == element2.Key);
        public static bool operator !=(BaseElement<TType> element1, IElement<TType> element2) => !(element1 == element2);

        public static bool operator ==(IElement<TType> element1, BaseElement<TType> element2) =>
            (element1 is null && element2 is null) ||
            (!(element1 is null) && !(element2 is null) && element1.Key == element2.Key);
        public static bool operator !=(IElement<TType> element1, BaseElement<TType> element2) => !(element1 == element2);

        public static bool operator ==(BaseElement<TType> element1, BaseElement<TType> element2) =>
            (element1 is null && element2 is null) ||
            (!(element1 is null) && !(element2 is null) && element1.Key == element2.Key);
        public static bool operator !=(BaseElement<TType> element1, BaseElement<TType> element2) => !(element1 == element2);

        protected BaseElement(NinjaScriptBase ninjascript, IPrintService printService, IInfo<TType> info) : base(ninjascript, info)
        {
            _printService = printService;
        }

        public void Log(Logging.LogLevel level, string state)
        {
            if (_printService == null || !_printService.IsEnable)
                return;
            switch (level)
            {
                case Logging.LogLevel.Trace:
                    _printService?.LogTrace(GetLogString(state));
                    break;
                case Logging.LogLevel.Debug:
                    _printService?.LogDebug(GetLogString(state));
                    break;
                case Logging.LogLevel.Information:
                    _printService?.LogInformation(GetLogString(state));
                    break;
                case Logging.LogLevel.Warning:
                    _printService?.LogWarning(GetLogString(state));
                    break;
                case Logging.LogLevel.Error:
                    _printService?.LogError(GetLogString(state));
                    break;
                default:
                    break;
            }
        }
        public virtual void LogConfigurationState()
        {
            if (_printService != null && _printService.IsEnable)
                return;

            if (IsDataLoaded && Ninjascript.State == State.DataLoaded)
                Log(Logging.LogLevel.Information, "has been loaded successfully.");
            else if (IsConfigure && Ninjascript.State == State.Configure)
                Log(Logging.LogLevel.Information, "has been configured successfully.");
            else if (!(IsConfigure && IsDataLoaded) && Ninjascript.State == State.DataLoaded)
                Log(Logging.LogLevel.Error, "could not be configured. The service will not work.");
            else
                Log(Logging.LogLevel.Error, "could not be configured. You are configuring the service out of configure or data loaded states.");
        }

        // REPAIR
        #region String methods

        public string ToString(int tabOrder, string title, string subtitle, string description, int index, bool isDescriptionBracketsVisible, bool isIndexVisible, string separator, string state)
        {
            // {tab}{header}({subheader})[{description}][{index}]{separator}{state}

            // Text to returns.
            //string text = string.Empty;
            StringBuilder sb = new StringBuilder();

            // TAB
            string tab = string.Empty;
            if (tabOrder > 0)
                for (int i = 0; i < tabOrder; i++)
                    tab += "\t";
            //text += tab;
            sb.Append(tab);

            // HEADER
            if (!string.IsNullOrEmpty(title))
                //text += header;
                sb.Append(title);

            // SUBHEADER
            if (!string.IsNullOrEmpty(subtitle))
            {
                if (string.IsNullOrEmpty(title))
                    //text += subheader;
                    sb.Append(subtitle);
                else
                    //text += "(" + subheader + ")";
                    sb.AppendFormat("({0})", subtitle);
            }

            // DESCRIPTION
            if (!string.IsNullOrEmpty(description))
            {
                if (!isDescriptionBracketsVisible)
                    //text += description;
                    sb.Append(description);
                else
                    //text += "(" + description + ")";
                    sb.AppendFormat("[{0}]", description);
            }

            // INDEX
            if (isIndexVisible)
                sb.AppendFormat("[{0}]", index);

            //SEPARATOR
            if (!string.IsNullOrEmpty(state) && string.IsNullOrWhiteSpace(state))
            {
                if (!string.IsNullOrEmpty(state))
                {
                    if (separator.EndsWith(" "))
                        sb.AppendFormat("{0}", separator);
                    else
                        sb.AppendFormat("{0} ", separator);
                }
                else
                    sb.Append(": ");
            }

            // STATE
            if (!string.IsNullOrEmpty(state))
                sb.Append(state);

            return sb.ToString();
        }
        public string ToString(string state) => ToString(
            tabOrder: 0,
            title: GetHeaderString(),
            subtitle: GetParentString(),
            description: GetDescriptionString(),
            index: 0,
            isDescriptionBracketsVisible: true,
            isIndexVisible: false,
            separator: ": ",
            state: state);
        public string ToString(int tabOrder, string state, int index = 0, string separator = ": ", bool isTitleVisible = true, bool isSubtitleVisible = false, bool isDescriptionVisible = true, bool isDescriptionBracketsVisible = true, bool isIndexVisible = false) => ToString(
            tabOrder: tabOrder,
            title: isTitleVisible ? GetHeaderString() : null,
            subtitle: isSubtitleVisible ? GetParentString() : null,
            description: isDescriptionVisible ? GetDescriptionString() : null,
            index: index,
            isDescriptionBracketsVisible: isDescriptionBracketsVisible,
            isIndexVisible: isIndexVisible,
            separator: separator,
            state: state);

        protected string GetLabelString(bool isLabelVisible, bool isHeaderVisible, bool isParentVisible, bool isDescriptionVisible, bool isDescriptionBracketsVisible, bool isIndexVisible, int index = 0)
        {
            if (!isLabelVisible)
                return string.Empty;

            if (!isHeaderVisible && !isParentVisible && !isDescriptionVisible && !isIndexVisible)
                return string.Empty;

            string header = GetHeaderString();
            string parent = GetParentString();
            string description = GetDescriptionString();

            // Text to returns.
            StringBuilder sb = new StringBuilder();

            // HEADER
            if (!string.IsNullOrEmpty(header))
                sb.Append(header);

            // SUBHEADER
            if (!string.IsNullOrEmpty(parent))
            {
                if (string.IsNullOrEmpty(header))
                    sb.Append(parent);
                else
                    sb.AppendFormat("({0})", parent);
            }

            // DESCRIPTION
            if (!string.IsNullOrEmpty(description))
            {
                if (!isDescriptionBracketsVisible)
                    sb.Append(description);
                else
                    sb.AppendFormat("({0})", description);
            }

            // INDEX
            if (isIndexVisible)
                sb.AppendFormat("[{0}]", index);

            return sb.ToString();

        }
        protected abstract string GetLogString(string state);
        protected abstract string GetHeaderString();
        protected abstract string GetParentString();
        protected abstract string GetDescriptionString();

        public string ToLogString(int tabOrder, string label, string state, string separator = ": ")
        {
            // Text to returns.
            StringBuilder sb = new StringBuilder();

            // TAB
            string tab = string.Empty;
            if (tabOrder > 0)
                for (int i = 0; i < tabOrder; i++)
                    tab += "\t";
            sb.Append(tab);

            // LABEL
            if (!string.IsNullOrEmpty(label))
                sb.Append(label);

            //SEPARATOR AND STATE
            if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(state) && !string.IsNullOrWhiteSpace(state))
            {
                if (!string.IsNullOrEmpty(state))
                {
                    if (separator.EndsWith(" "))
                        sb.AppendFormat("{0}", separator);
                    else
                        sb.AppendFormat("{0} ", separator);

                    sb.Append(state);
                }
            }

            sb.Append(state);

            return sb.ToString();

        }
        public string ToLogString(int tabOrder, string state, int index = 0, string separator = ": ", bool isLabelVisible = true, bool isTitleVisible = true, bool isDescriptionVisible = true, bool isDescriptionBracketsVisible = true, bool isIndexVisible = false)
            => ToLogString(
                tabOrder,
                ToLabelString(isLabelVisible, isTitleVisible, isDescriptionVisible, isDescriptionBracketsVisible, isIndexVisible, index),
                state,
                separator);
        protected string ToString(ElementType elementType) => elementType.ToString();
        protected string ToLabelString(bool isLabelVisible, bool isTitleVisible, bool isDesriptionVisible, bool isDescriptionBracketsVisible, bool isIndexVisible, int index = 0)
        {
            if (!isLabelVisible)
                return string.Empty;

            if (!isTitleVisible && !isDesriptionVisible && !isIndexVisible)
                return string.Empty;

            string title = Type.ToString(); // ToString(Type);
            string description = ToString();

            // Text to returns.
            StringBuilder sb = new StringBuilder();

            // HEADER
            if (!string.IsNullOrEmpty(title))
                sb.Append(title);

            // SUBHEADER
            if (!string.IsNullOrEmpty(description))
            {
                if (string.IsNullOrEmpty(title) || !isDescriptionBracketsVisible)
                    sb.Append(description);
                else
                    sb.AppendFormat("({0})", description);
            }

            // INDEX
            if (isIndexVisible)
                sb.AppendFormat("[{0}]", index);

            return sb.ToString();
        }

        #endregion


    }

    public abstract class BaseElement<TType,TInfo> : BaseElement<TType>, IElement<TType,TInfo>
        where TInfo : IInfo<TType>
        where TType : Enum
    {
        protected BaseElement(NinjaScriptBase ninjascript, IPrintService printService, TInfo info) : base(ninjascript, printService, info)
        {
        }

        new public TInfo Info => (TInfo)base.Info; 
    }

    public abstract class BaseElement<TType,TInfo, TOptions> : BaseElement<TType>, IElement<TType, TInfo, TOptions>
        where TInfo : IInfo<TType>
        where TOptions : IOptions
        where TType : Enum
    {
        private readonly TOptions _options;

        new public TInfo Info => (TInfo)base.Info;
        public TOptions Options => _options;

        protected BaseElement(NinjaScriptBase ninjascript, IPrintService printService, TInfo info, TOptions options) : base(ninjascript, printService, info)
        {
            if (options == null)
                throw new ArgumentNullException($"The {nameof(options)} argument cannot be null.");
            _options = options;
        }
    }

}
