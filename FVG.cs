#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it.
namespace NinjaTrader.NinjaScript.Indicators.SMC
{
    public class FvgCurrent : Indicator
    {
        private TimeSpan tsPlotStartTime;
        private TimeSpan tsPlotEndTime;

        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description                 = @"SMC : FVG with Time Filter";
                Name                        = "FVG"; 
                Calculate                   = Calculate.OnBarClose;
                IsOverlay                   = true;
                DisplayInDataBox            = true;
                DrawOnPricePanel            = true;
                DrawHorizontalGridLines     = true;
                DrawVerticalGridLines       = true;
                PaintPriceMarkers           = true;
                ScaleJustification          = ScaleJustification.Right;
                IsSuspendedWhileInactive    = true;
                BarsRequiredToPlot          = 3;

                // Default Colors for FVG
                BullishFvgColor             = Brushes.Teal;
                BearishFvgColor             = Brushes.Crimson;
                FvgOpacity                  = 30;

                // Default Time Filter settings
                EnableTimeFilter            = false;
                
                PlotStartTime               = new DateTime(1, 1, 1, 9, 30, 0);  // Default 09:30
                PlotEndTime                 = new DateTime(1, 1, 1, 11, 30, 0); // Default 16:00
            }
            else if (State == State.Configure)
            {
                
                tsPlotStartTime = PlotStartTime.TimeOfDay;
                tsPlotEndTime   = PlotEndTime.TimeOfDay;
            }
         
        }

        protected override void OnBarUpdate()
        {
            try
            {
                if (CurrentBar < BarsRequiredToPlot - 1)
                    return;

                // Time Filter Logic
                bool canPlotBasedOnTime = true;
                if (EnableTimeFilter)
                {
                    TimeSpan currentTimeOfDay = Time[0].TimeOfDay; 

                    if (tsPlotStartTime < tsPlotEndTime) 
                    {
                        canPlotBasedOnTime = (currentTimeOfDay >= tsPlotStartTime && currentTimeOfDay <= tsPlotEndTime);
                    }
                    else 
                    {
                        canPlotBasedOnTime = (currentTimeOfDay >= tsPlotStartTime || currentTimeOfDay <= tsPlotEndTime);
                    }
                }

                if (!canPlotBasedOnTime) 
                {
                    return; 
                }

                bool isBullFvg = Low[0] > High[2] && Close[1] > High[2];
                if (isBullFvg)
                {
                   
                    Draw.Rectangle(this, "bull-" + CurrentBar, false, 2, Low[0], 0, High[2], Brushes.Transparent, BullishFvgColor, FvgOpacity);
                }

              
                bool isBearFvg = High[0] < Low[2] && Close[1] < Low[2];
                if (isBearFvg)
                {
                    
                    Draw.Rectangle(this, "bear-" + CurrentBar, false, 2, Low[2], 0, High[0], Brushes.Transparent, BearishFvgColor, FvgOpacity);
                }
            }
            catch (Exception e)
            {
                Print("Time: " + Time[0] + " FVG Current Exception: " + e.Message);
                Print("Stack Trace: " + e.StackTrace);
            }
        }

        #region Properties

        // --- Visuals Group ---
        [NinjaScriptProperty]
        [XmlIgnore]
        [Display(Name="Bullish FVG Color", Description="Color for Bullish Fair Value Gaps.", Order=1, GroupName="Visuals")]
        public Brush BullishFvgColor { get; set; }

        [Browsable(false)] // Hide this from property grid, used for serialization
        public string BullishFvgColorSerialize
        {
            get { return Serialize.BrushToString(BullishFvgColor); }
            set { BullishFvgColor = Serialize.StringToBrush(value); }
        }

        [NinjaScriptProperty]
        [XmlIgnore]
        [Display(Name="Bearish FVG Color", Description="Color for Bearish Fair Value Gaps.", Order=2, GroupName="Visuals")]
        public Brush BearishFvgColor { get; set; }

        [Browsable(false)] // Hide this from property grid, used for serialization
        public string BearishFvgColorSerialize
        {
            get { return Serialize.BrushToString(BearishFvgColor); }
            set { BearishFvgColor = Serialize.StringToBrush(value); }
        }

        [NinjaScriptProperty]
        [Range(0, 100)]
        [Display(Name = "FVG Opacity", Description = "Opacity of the FVG rectangles (0-100). 0 is transparent, 100 is opaque.", Order = 3, GroupName = "Visuals")]
        public int FvgOpacity { get; set; }

        // --- Time Filter Group ---
        [NinjaScriptProperty]
        [Display(Name = "Enable Time Filter", Description = "Enable to plot FVGs only within the specified time window.", Order = 10, GroupName = "Time Filter")]
        public bool EnableTimeFilter { get; set; }

        [NinjaScriptProperty]
        [PropertyEditor("NinjaTrader.Gui.Tools.TimeEditorKey")]
        [Display(Name = "Plot Start Time", Description = "Start time for plotting FVGs (if time filter is enabled).", Order = 11, GroupName = "Time Filter")]
        public DateTime PlotStartTime { get; set; }

        [NinjaScriptProperty]
        [PropertyEditor("NinjaTrader.Gui.Tools.TimeEditorKey")]
        [Display(Name = "Plot End Time", Description = "End time for plotting FVGs (if time filter is enabled).", Order = 12, GroupName = "Time Filter")]
        public DateTime PlotEndTime { get; set; }

        #endregion
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private SMC.FvgCurrent[] cacheFvgCurrent;
		public SMC.FvgCurrent FvgCurrent(Brush bullishFvgColor, Brush bearishFvgColor, int fvgOpacity, bool enableTimeFilter, DateTime plotStartTime, DateTime plotEndTime)
		{
			return FvgCurrent(Input, bullishFvgColor, bearishFvgColor, fvgOpacity, enableTimeFilter, plotStartTime, plotEndTime);
		}

		public SMC.FvgCurrent FvgCurrent(ISeries<double> input, Brush bullishFvgColor, Brush bearishFvgColor, int fvgOpacity, bool enableTimeFilter, DateTime plotStartTime, DateTime plotEndTime)
		{
			if (cacheFvgCurrent != null)
				for (int idx = 0; idx < cacheFvgCurrent.Length; idx++)
					if (cacheFvgCurrent[idx] != null && cacheFvgCurrent[idx].BullishFvgColor == bullishFvgColor && cacheFvgCurrent[idx].BearishFvgColor == bearishFvgColor && cacheFvgCurrent[idx].FvgOpacity == fvgOpacity && cacheFvgCurrent[idx].EnableTimeFilter == enableTimeFilter && cacheFvgCurrent[idx].PlotStartTime == plotStartTime && cacheFvgCurrent[idx].PlotEndTime == plotEndTime && cacheFvgCurrent[idx].EqualsInput(input))
						return cacheFvgCurrent[idx];
			return CacheIndicator<SMC.FvgCurrent>(new SMC.FvgCurrent(){ BullishFvgColor = bullishFvgColor, BearishFvgColor = bearishFvgColor, FvgOpacity = fvgOpacity, EnableTimeFilter = enableTimeFilter, PlotStartTime = plotStartTime, PlotEndTime = plotEndTime }, input, ref cacheFvgCurrent);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.SMC.FvgCurrent FvgCurrent(Brush bullishFvgColor, Brush bearishFvgColor, int fvgOpacity, bool enableTimeFilter, DateTime plotStartTime, DateTime plotEndTime)
		{
			return indicator.FvgCurrent(Input, bullishFvgColor, bearishFvgColor, fvgOpacity, enableTimeFilter, plotStartTime, plotEndTime);
		}

		public Indicators.SMC.FvgCurrent FvgCurrent(ISeries<double> input , Brush bullishFvgColor, Brush bearishFvgColor, int fvgOpacity, bool enableTimeFilter, DateTime plotStartTime, DateTime plotEndTime)
		{
			return indicator.FvgCurrent(input, bullishFvgColor, bearishFvgColor, fvgOpacity, enableTimeFilter, plotStartTime, plotEndTime);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.SMC.FvgCurrent FvgCurrent(Brush bullishFvgColor, Brush bearishFvgColor, int fvgOpacity, bool enableTimeFilter, DateTime plotStartTime, DateTime plotEndTime)
		{
			return indicator.FvgCurrent(Input, bullishFvgColor, bearishFvgColor, fvgOpacity, enableTimeFilter, plotStartTime, plotEndTime);
		}

		public Indicators.SMC.FvgCurrent FvgCurrent(ISeries<double> input , Brush bullishFvgColor, Brush bearishFvgColor, int fvgOpacity, bool enableTimeFilter, DateTime plotStartTime, DateTime plotEndTime)
		{
			return indicator.FvgCurrent(input, bullishFvgColor, bearishFvgColor, fvgOpacity, enableTimeFilter, plotStartTime, plotEndTime);
		}
	}
}

#endregion
