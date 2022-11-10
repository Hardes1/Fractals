using System;
using System.Windows;
using FractalPeer.Fractals.Base;

#pragma warning disable
namespace FractalPeer.Fractals.TypeFractals
{
    /// <summary>
    /// Реализует фрактал "Треугольник Серпинского" и паттерн Singleton.
    /// </summary>
    internal class SerpinskiyTriangleFractal : Fractal
    {
        /// <summary>
        /// Одиночный экземпляр класса.
        /// </summary>
        private static readonly SerpinskiyTriangleFractal s_instance = new SerpinskiyTriangleFractal();

        /// <summary>
        /// Приватный конструктор класса.
        /// </summary>
        private SerpinskiyTriangleFractal()
        {
        }

        /// <summary>
        /// Для реализации паттерна Singleton.
        /// </summary>
        /// <returns>Возвращает единственный экземпляр данного класса.</returns>
        public static SerpinskiyTriangleFractal GetInstance()
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
            double height = s_canvas.ActualHeight;
            double width = s_canvas.ActualWidth;
            double length = Math.Min(width / 3, height / 3);
            Point first = new(width / 3, 2 * height / 3);
            Point third = new(first.X + length, 2 * height / 3);
            Point second = new(first.X + length * Math.Cos(Math.PI / 3), first.Y - length * Math.Sin(Math.PI / 3));
            if (s_stackPanel.Children.Count > 0)
                ChangeAdditionalParametres();
            Draw(first, second, third);
        }

        /// <summary>
        /// Рекурсивный метод рисования фракталов.
        /// </summary>
        /// <param name="first">Первая вершина треугольника.</param>
        /// <param name="second">Вторая вершина треугольника.</param>
        /// <param name="third">Третья вершина треугольника.</param>
        /// <param name="currentIteration">Номер текущей итерации.</param>
        /// <param name="typePaint">Тип отрезка, который нужно сейчас рисовать.</param>
        private void Draw(Point first, Point second, Point third, int currentIteration = 1, int typePaint = -1)
        {
            if (currentIteration > (int) s_sliderRecursionDepth.Value)
            {
                return;
            }

            if (typePaint == -1)
            {
                DrawLine(first, second, s_brushes[currentIteration - 1]);
                DrawLine(second, third, s_brushes[currentIteration - 1]);
                DrawLine(first, third, s_brushes[currentIteration - 1]);
            }
            else if (typePaint == 0)
            {
                DrawLine(first, third, s_brushes[currentIteration - 1]);
            }
            else if (typePaint == 1)
            {
                DrawLine(first, second, s_brushes[currentIteration - 1]);
            }
            else
            {
                DrawLine(second, third, s_brushes[currentIteration - 1]);
            }

            Point newFirst = new((first.X + second.X) / 2, (first.Y + second.Y) / 2);
            Point newSecond = new((second.X + third.X) / 2, (second.Y + third.Y) / 2);
            Point newThird = new((first.X + third.X) / 2, (first.Y + third.Y) / 2);
            Draw(newFirst, second, newSecond, currentIteration + 1, 0);
            Draw(first, newFirst, newThird, currentIteration + 1, 2);
            Draw(newThird, newSecond, third, currentIteration + 1, 1);
        }
    }
}