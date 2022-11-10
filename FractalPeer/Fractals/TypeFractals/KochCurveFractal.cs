using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using FractalPeer.Fractals.Base;

#pragma warning disable
namespace FractalPeer.Fractals.TypeFractals
{
    /// <summary>
    /// Реализует фрактал "Кривая Коха" и паттерн Singleton.
    /// </summary>
    internal class KochCurveFractal : Fractal
    {
        /// <summary>
        /// Одиночный экземпляр класса.
        /// </summary>
        private static readonly KochCurveFractal s_instance = new KochCurveFractal();

        /// <summary>
        /// Приватный конструктор класса.
        /// </summary>
        private KochCurveFractal()
        {
        }

        /// <summary>
        /// Для реализации паттерна Singleton.
        /// </summary>
        /// <returns>Возвращает единственный экземпляр данного класса.</returns>
        public static KochCurveFractal GetInstance()
        {
            return s_instance;
        }

        /// <summary>
        /// Меняет панель дополнительных параметров.
        /// </summary>
        protected override void ChangeAdditionalParametres()
        {
            while (s_stackPanel.Children.Count > 0)
                s_stackPanel.Children.RemoveAt(s_stackPanel.Children.Count - 1);
        }

        /// <summary>
        /// Происходит перед тем, как начать рисовать фрактал.
        /// </summary>
        public override void PrepareDraw()
        {
            s_canvas.Children.Clear();
            SetColors();
            if (s_stackPanel.Children.Count > 0)
                ChangeAdditionalParametres();
            double height = s_canvas.ActualHeight;
            double width = s_canvas.ActualWidth;
            List<List<(Point, Point)>> list = new List<List<(Point, Point)>>();
            for (int i = 0; i <= (int) s_sliderRecursionDepth.Value; i++)
                list.Add(new List<(Point, Point)>());
            Point start = new Point(width / 6, 2 * height / 3);
            Point end = new Point(5 * width / 6, 2 * height / 3);
            DrawLine(start, end, s_brushes[0]);
            list[0].Add((start, end));
            Draw(list);
        }

        /// <summary>
        /// Рекурсивный метод рисования фракталов.
        /// </summary>
        /// <param name="list">лист отрезков для рисования на текущей итерации.</param>
        /// <param name="currentIteration">Номер текущей итерации.</param>
        private void Draw(List<List<(Point, Point)>> list, int currentIteration = 1)
        {
            if (currentIteration > (int) s_sliderRecursionDepth.Value)
                return;
            foreach ((Point, Point) current in list[currentIteration - 1])
            {
                double newLength = getDistance(current.Item1, current.Item2) / 3;
                Point newStart = new Point(current.Item1.X + (current.Item2.X - current.Item1.X) / 3,
                    current.Item1.Y + (current.Item2.Y - current.Item1.Y) / 3);
                Point newEnd = new Point(current.Item1.X + 2 * (current.Item2.X - current.Item1.X) / 3,
                    current.Item1.Y + 2 * (current.Item2.Y - current.Item1.Y) / 3);
                DrawLine(newStart, newEnd, Brushes.White, Constants.FatLineThickness);
                double angle = -Math.Atan2(current.Item2.Y - current.Item1.Y, current.Item2.X - current.Item1.X);
                angle += Math.PI / 3;
                Point newMiddle = new Point(newStart.X + newLength * Math.Cos(angle),
                    newStart.Y - newLength * Math.Sin(angle));
                DrawLine(newStart, newMiddle, s_brushes[currentIteration - 1]);
                DrawLine(newMiddle, newEnd, s_brushes[currentIteration - 1]);
                list[currentIteration].Add((newStart, newMiddle));
                list[currentIteration].Add((newMiddle, newEnd));
                list[currentIteration].Add((current.Item1, newStart));
                list[currentIteration].Add((newEnd, current.Item2));
            }

            Draw(list, currentIteration + 1);
        }

        private double getDistance(Point a, Point b) =>
            Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
    }
}