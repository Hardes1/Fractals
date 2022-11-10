using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable
namespace FractalPeer.Fractals.Base
{
    /// <summary>
    /// Класс, содержащий все константы приложения.
    /// </summary>
    internal static class Constants
    {
        public const int CantorWidth = 10;
        public const int MainFontSize = 18;
        public const int SecondFontSize = 16;
        public const int SliderAngleMinimum = 0;
        public const int SliderAngleMaximum = 90;
        public const int SliderAngleDefaultValue = 45;
        public const int SliderAngleDefaultFrequency = 5;
        public const double SliderCoefficientMinimum = 0.1;
        public const double SliderCoefficientMaximum = 0.9;
        public const double SliderCoefficientDefaultValue = 0.5;
        public const double SliderCoefficientDefaultFrequency = 0.1;
        public const int SliderDistanceMinimum = 10;
        public const int SliderDistanceMaximum = 100;
        public const int SliderDistanceDefaultValue = 30;
        public const int SliderDistanceDefaultFrequency = 10;
        public const int LineThickness = 2;
        public const int FatLineThickness = 5;
        public const int MaxTreeFractal = 12;
        public const int ValueTreeFractal = 5;
        public const int MaxKochFractal = 6;
        public const int ValueKochFractal = 4;
        public const int MaxCarpetFractal = 5;
        public const int ValueCarpetFractal = 3;
        public const int MaxTriangleFractal = 7;
        public const int ValueTriangleFractal = 5;
        public const int MaxSetFractal = 9;
        public const int ValueSetFractal = 4;
        public const string Coefficient = "Коэффициент cжатия";
        public const string Distance = "Расстояние между уровнями";
        public const string AngleLeft = "Угол левого наклона";
        public const string AngleRight = "Угол правого наклона";
        public const string ParametresHeader = "Дополнительные параметры";
    }
}