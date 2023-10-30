﻿using System;

namespace KrTrade.Nt.Core.Bars
{
    public class Bar
    {
        public DateTime Time { get; set; }
        public double Open { get;set; }
        public double High { get;set; }
        public double Low { get;set; }
        public double Close { get;set; }
        public long Volume { get;set; }
        
    }
}
