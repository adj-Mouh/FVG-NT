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

namespace NinjaTrader.NinjaScript.Indicators.RajIndicators
{
    public class FvgLtf : Indicator
    {
        
        protected override void OnStateChange()
        {
            if (State == State.SetDefaults)
            {
                Description = @"MTF FVG";
                Name = "MTF FVG";
                Calculate = Calculate.OnBarClose;
                IsOverlay = true;
                DisplayInDataBox = true;
                DrawOnPricePanel = true;
                DrawHorizontalGridLines = true;
                DrawVerticalGridLines = true;
                PaintPriceMarkers = true;
                ScaleJustification = ScaleJustification.Right;
                //Disable this property if your indicator requires custom values that cumulate with each new market data event.
                //See Help Guide for additional information.
                IsSuspendedWhileInactive = true;
                BarsRequiredToPlot = 10;

                TF_Type = BarsPeriodType.Minute;
                TF_Value = 15 ;
               

            }
            else if (State == State.Configure)
            {
                AddDataSeries(TF_Type, TF_Value);
            }
        }

        protected override void OnBarUpdate()
        {
            try
            {
                if (CurrentBars[0] < BarsRequiredToPlot)
                    return;

                if (BarsInProgress != 1)
                    return;
				
                bool isBullFvg = Lows[1][0] > Highs[1][2] && Closes[1][1] > Highs[1][2];
                if (isBullFvg)
                {
                    Draw.Rectangle(this, "bull-" + CurrentBars[0], true, 2, Lows[1][0], -20, Highs[1][2], Brushes.Transparent, Brushes.Teal, 50);
                }

                bool isBearFvg = Highs[1][0] < Lows[1][2] && Closes[1][1] < Lows[1][2];
                if (isBearFvg)
                {
                    Draw.Rectangle(this, "bear-" + CurrentBars[0], true, 2, Lows[1][2], -20, Highs[1][0], Brushes.Transparent, Brushes.Crimson, 50);
                }
            }
            catch (Exception e)
            {
                Print("Time: " + Time[0]);
                Print("Exception caught: " + e.Message);
                Print("Stack Trace: " + e.StackTrace);
            }
        }

        public class Fvg
        {
            public int left;
            public double top;
            public int right;
            public double bottom;
            
            public List<Labels> labels;
            public int x_val;

            public Fvg(int _left)
            {
                left = _left;
            }

            public Fvg(int _left, double _top, int _right, double _bottom)
            {
                left = _left;
                top = _top;
                right = _right;
                bottom = _bottom;
            }
        }

        public struct Labels
        {
            public int x;
            public double y;
            public int direction;

            public Labels(int _x = 0, double _y = 0.0, int _direction = 0)
            {
                x = _x;
                y = _y;
                direction = _direction;
            }
        }

        #region Properties
        [NinjaScriptProperty]
        [Display(Name = "TF_Type", Order = 1, GroupName = "Parameters")]
        [PropertyEditor("NinjaTrader.Gui.Tools.BarsPeriodTypeEditor")]
        public BarsPeriodType TF_Type { get; set; }

        [NinjaScriptProperty]
        [Range(1, int.MaxValue)]
        [Display(Name = "TF_Value", Order = 2, GroupName = "Parameters")]
        public int TF_Value { get; set; }
		#endregion
      
    }
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private RajIndicators.FvgLtf[] cacheFvgLtf;
		public RajIndicators.FvgLtf FvgLtf(BarsPeriodType tF_Type, int tF_Value)
		{
			return FvgLtf(Input, tF_Type, tF_Value);
		}

		public RajIndicators.FvgLtf FvgLtf(ISeries<double> input, BarsPeriodType tF_Type, int tF_Value)
		{
			if (cacheFvgLtf != null)
				for (int idx = 0; idx < cacheFvgLtf.Length; idx++)
					if (cacheFvgLtf[idx] != null && cacheFvgLtf[idx].TF_Type == tF_Type && cacheFvgLtf[idx].TF_Value == tF_Value && cacheFvgLtf[idx].EqualsInput(input))
						return cacheFvgLtf[idx];
			return CacheIndicator<RajIndicators.FvgLtf>(new RajIndicators.FvgLtf(){ TF_Type = tF_Type, TF_Value = tF_Value }, input, ref cacheFvgLtf);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.RajIndicators.FvgLtf FvgLtf(BarsPeriodType tF_Type, int tF_Value)
		{
			return indicator.FvgLtf(Input, tF_Type, tF_Value);
		}

		public Indicators.RajIndicators.FvgLtf FvgLtf(ISeries<double> input , BarsPeriodType tF_Type, int tF_Value)
		{
			return indicator.FvgLtf(input, tF_Type, tF_Value);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.RajIndicators.FvgLtf FvgLtf(BarsPeriodType tF_Type, int tF_Value)
		{
			return indicator.FvgLtf(Input, tF_Type, tF_Value);
		}

		public Indicators.RajIndicators.FvgLtf FvgLtf(ISeries<double> input , BarsPeriodType tF_Type, int tF_Value)
		{
			return indicator.FvgLtf(input, tF_Type, tF_Value);
		}
	}
}

#endregion
