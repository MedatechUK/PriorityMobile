using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GoogleChartSharp;
using System.Diagnostics;

namespace Tests
{
    class LineChartTests
    {
        public static string singleDatasetPerLine()
        {
            int[] line1 = new int[] { 5, 10, 50, 34, 10, 25 };
            int[] line2 = new int[] { 15, 20, 60, 44, 20, 35 };

            List<int[]> dataset = new List<int[]>();
            dataset.Add(line1);
            dataset.Add(line2);

            LineChart lineChart = new LineChart(250, 150);
            lineChart.SetTitle("Single Dataset Per Line", "0000FF", 14);
            lineChart.SetData(dataset);
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Left));

            return lineChart.GetUrl();
        }

        public static string multiDatasetPerLine()
        {
            int[] line1x = new int[] { 0, 15, 30, 45, 60 };
            int[] line1y = new int[] { 10, 50, 15, 60, 12};
            int[] line2x = new int[] { 0, 15, 30, 45, 60 };
            int[] line2y = new int[] { 45, 12, 60, 34, 60 };

            List<int[]> dataset = new List<int[]>();
            dataset.Add(line1x);
            dataset.Add(line1y);
            dataset.Add(line2x);
            dataset.Add(line2y);

            LineChart lineChart = new LineChart(250, 150, LineChartType.MultiDataSet);
            lineChart.SetTitle("Multi Dataset Per Line", "0000FF", 14);
            lineChart.SetData(dataset);
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Left));

            return lineChart.GetUrl();
        }

        public static string lineColorAndLegendTest()
        {
            int[] line1 = new int[] { 5, 10, 50, 34, 10, 25 };
            int[] line2 = new int[] { 15, 20, 60, 44, 20, 35 };

            List<int[]> dataset = new List<int[]>();
            dataset.Add(line1);
            dataset.Add(line2);

            LineChart lineChart = new LineChart(250, 150);
            lineChart.SetTitle("Line Color And Legend Test", "0000FF", 14);
            lineChart.SetData(dataset);
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            lineChart.AddAxis(new ChartAxis(ChartAxisType.Left));

            lineChart.SetDatasetColors(new string[] { "FF0000", "00FF00" });
            lineChart.SetLegend(new string[] { "line1", "line2" });

            return lineChart.GetUrl();
        }
    }
}
