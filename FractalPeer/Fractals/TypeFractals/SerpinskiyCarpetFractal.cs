using System;
using System.Windows;
using System.Windows.Media;
using FractalPeer.Fractals.Base;

#pragma warning disable
namespace FractalPeer.Fractals.TypeFractals
{
    /// <summary>
    /// Реализует фрактал "Ковёр Серпинского" и паттерн Singleton.
    /// </summary>
    internal class SerpinskiyCarpetFractal : Fractal
    {
        /// <summary>
        /// Одиночный экземпляр класса.
        /// </summary>
        private static readonly SerpinskiyCarpetFractal s_instance = new SerpinskiyCarpetFractal();

        /// <summary>
        /// Приватный конструктор класса.
        /// </summary>
        private SerpinskiyCarpetFractal()
        {
        }

        /// <summary>
        /// Для реализации паттерна Singleton.
        /// </summary>
        /// <returns>Возвращает единственный экземпляр данного класса.</returns>
        public static SerpinskiyCarpetFractal GetInstance()
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
            double width = s_canvas.ActualWidth;
            double height = s_canvas.ActualHeight;
            double length = Math.Min(2 * width / 3, 2 * height / 3);
            Draw(new(width / 6, height / 6), length);
        }

        /// <summary>
        /// Рекурсивный метод рисования фракталов.
        /// </summary>
        /// <param name="start">Точка левого верхнего угла квадрата.</param>
        /// <param name="length">Текущая длина квадрата.</param>
        /// <param name="currentIteration">Текущая итерация.</param>
        private void Draw(Point start, double length, int currentIteration = 1)
        {
            if (currentIteration > (int) s_sliderRecursionDepth.Value || length < 1)
                return;
            Point end = new(start.X + length, start.Y + length);
            if (currentIteration == 1)
            {
                DrawPolygon(start, end, s_brushes[currentIteration - 1]);
            }
            else
            {
                Point newStart = new(start.X + length / 6, start.Y + length / 6);
                Point newEnd = new(start.X + 5 * length / 6, start.Y + 5 * length / 6);
                DrawPolygon(newStart, newEnd, s_brushes[currentIteration - 1]);
            }

            Point startCenter = new(start.X + length / 3, start.Y + length / 3);
            Point endCenter = new(end.X - length / 3, end.Y - length / 3);
            DrawPolygon(startCenter, endCenter, Brushes.White);
            Draw(start, length / 3, currentIteration + 1);
            Draw(new(start.X + length / 3, start.Y), length / 3, currentIteration + 1);
            Draw(new(start.X + 2 * length / 3, start.Y), length / 3, currentIteration + 1);
            Draw(new(start.X, start.Y + length / 3), length / 3, currentIteration + 1);
            Draw(new(start.X + 2 * length / 3, start.Y + length / 3), length / 3, currentIteration + 1);
            Draw(new(start.X, start.Y + 2 * length / 3), length / 3, currentIteration + 1);
            Draw(new(start.X + length / 3, start.Y + 2 * length / 3), length / 3, currentIteration + 1);
            Draw(new(start.X + 2 * length / 3, start.Y + 2 * length / 3), length / 3, currentIteration + 1);
        }
    }
}