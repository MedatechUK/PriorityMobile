using System;
using System.Collections.Generic;
using System.Text;
using GoogleChartSharp;

namespace Tests
{
    class BarChartTests
    {
        public static string horizontalStackedTest()
        {
            int[] data1 = new int[] { 10, 5, 20, 15 };
            int[] data2 = new int[] { 10, 10, 10, 10 };

            List<int[]> dataset = new List<int[]>();
            dataset.Add(data1);
            dataset.Add(data2);

            BarChart barChart = new BarChart(150, 150, BarChartOrientation.Horizontal, BarChartStyle.Stacked);
            barChart.SetTitle("Horizontal Stacked");
            barChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            barChart.AddAxis(new ChartAxis(ChartAxisType.Left));
            barChart.SetData(dataset);

            barChart.SetDatasetColors(new string[] { "FF0000", "00AA00" });
            
            return barChart.GetUrl();
        }

        public static string verticalStackedTest()
        {
            int[] data1 = new int[] { 10, 5, 20, 15 };
            int[] data2 = new int[] { 10, 10, 10, 10 };

            List<int[]> dataset = new List<int[]>();
            dataset.Add(data1);
            dataset.Add(data2);

            BarChart barChart = new BarChart(150, 150, BarChartOrientation.Vertical, BarChartStyle.Stacked);
            barChart.SetTitle("Vertical Grouped");
            barChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            barChart.AddAxis(new ChartAxis(ChartAxisType.Left));
            barChart.SetData(dataset);

            barChart.SetDatasetColors(new string[] { "FF0000", "00AA00" });

            return barChart.GetUrl();
        }

        public static string horizontalGroupedTest()
        {
            int[] data1 = new int[] { 10, 5, 20 };

            BarChart barChart = new BarChart(150, 150, BarChartOrientation.Horizontal, BarChartStyle.Grouped);
            barChart.SetTitle("Horizontal Grouped");
            barChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            barChart.AddAxis(new ChartAxis(ChartAxisType.Left));
            barChart.SetData(data1);

            barChart.SetDatasetColors(new string[] { "FF0000", "00AA00" });

            return barChart.GetUrl();
        }

        public static string verticalGroupedTest()
        {
            int[] data1 = new int[] { 10, 5, 20, 15 };

            BarChart barChart = new BarChart(150, 150, BarChartOrientation.Vertical, BarChartStyle.Grouped);
            barChart.SetTitle("Vertical Grouped");
            barChart.AddAxis(new ChartAxis(ChartAxisType.Bottom));
            barChart.AddAxis(new ChartAxis(ChartAxisType.Left));
            barChart.SetData(data1);

            barChart.SetDatasetColors(new string[] { "FF0000", "00AA00" });

            return barChart.GetUrl();
        }
    }
}
