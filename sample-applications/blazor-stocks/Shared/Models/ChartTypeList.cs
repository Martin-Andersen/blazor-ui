using System.Collections.Generic;
using Telerik.Blazor;

namespace BlazorFinancePortfolio.Models
{
    public class ChartTypeList
    {
        public ChartSeriesType Value { get; set; }
        public string Text => Value.ToString();

        public static List<ChartTypeList> GetAvailableSeriesTypes()
        {
            return new List<ChartTypeList>
                {
                    new() { Value = ChartSeriesType.Candlestick },
                    new() { Value = ChartSeriesType.Area },
                    new() { Value = ChartSeriesType.Line }
                };
        }
    }
}
