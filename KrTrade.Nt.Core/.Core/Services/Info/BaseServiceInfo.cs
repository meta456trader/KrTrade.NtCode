﻿using KrTrade.Nt.Core.Data;
using System;

namespace KrTrade.Nt.Core
{
    public class ServiceInfo : Info, IServiceInfo
    {
        new public ServiceType Type { get => base.Type.ToServiceType(); set => base.Type = value.ToElementType(); }

        protected ServiceInfo() : this(ServiceType.UNKNOWN) { }
        protected ServiceInfo(ServiceType type) : base(type.ToElementType()) {  }

    }

    public class BaseServiceInfo<T> : ServiceInfo, IBaseServiceInfo<T>
    where T : Enum
    {
        private T _type;
        new public T Type
        {
            get => _type;
            set
            {
                base.Type = value.ToElementType().ToServiceType();
                _type = value;
            }
        }
    }
}
