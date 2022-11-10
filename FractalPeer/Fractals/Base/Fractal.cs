using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

#pragma warning disable
namespace FractalPeer.Fractals.Base
{
    /// <summary>
    /// Базовый класс для всех фракталов.
    /// </summary>
    internal abstract class Fractal
    {
        /// <summary>
        /// Элементы управления, которые находятся внутри окна.
        /// </summary>
        protected static Canvas s_canvas;

        protected static Slider s_sliderRecursionDepth;
        protected static StackPanel s_stackPanel;
        private static ColorPicker s_colorStart;
        private static ColorPicker s_colorEnd;
        protected static List<SolidColorBrush> s_brushes = new List<SolidColorBrush>();

        /// <summary>
        /// Рисует линию заданного цвета и толщины на канвасе по двум точкам.
        /// </summary>
        /// <param name="first">Первая точка.</param>
        /// <param name="second">Вторая точка.</param>
        /// <param name="brush">Кисть цвета.</param>
        /// <param name="lineWidth">Толщина линии.</param>
        protected void DrawLine(Point first, Point second, SolidColorBrush brush,
            int lineWidth = Constants.LineThickness)
        {
            Line line = new Line
            {
                Stroke = brush,
                StrokeThickness = lineWidth,
                Visibility = Visibility.Visible,
                X1 = first.X,
                Y1 = first.Y,
                X2 = second.X,
                Y2 = second.Y
            };
            s_canvas.Children.Add(line);
        }

        /// <summary>
        /// Рисует прямоугольник заданного цвета по двум точкам.
        /// </summary>
        /// <param name="first">Первая точка.</param>
        /// <param name="second">Вторая точка.</param>
        /// <param name="brush">Кисть цвета.</param>
        protected void DrawPolygon(Point first, Point second, SolidColorBrush brush)
        {
            Polygon polygon = new Polygon();
            polygon.Fill = brush;
            polygon.Points.Add(first);
            polygon.Points.Add(new(second.X, first.Y));
            polygon.Points.Add(second);
            polygon.Points.Add(new(first.X, second.Y));
            s_canvas.Children.Add(polygon);
        }

        /// <summary>
        /// Инициализирует все объекты для корректного рисования и взаимодействия с пользователем. Элементы (Controls)
        /// метод получает из класса окна.
        /// </summary>
        /// <param name="canvas">Текущий канвас окна.</param>
        /// <param name="slider">Слайдер глубины рекурсии.</param>
        /// <param name="middleStackPanel">Панель с дополнительными параметрами.</param>
        /// <param name="colorStart">Элемент выбора начального цвета.</param>
        /// <param name="colorEnd">Элемент выбора конечного цвета.</param>
        public void Initialize(Canvas canvas, Slider slider, StackPanel middleStackPanel, ColorPicker colorStart,
            ColorPicker colorEnd)
        {
            s_canvas = canvas;
            s_sliderRecursionDepth = slider;
            s_stackPanel = middleStackPanel;
            s_colorStart = colorStart;
            s_colorEnd = colorEnd;
            s_colorStart.SelectedColor = Color.FromRgb(0, 0, 0);
            s_colorEnd.SelectedColor = Color.FromRgb(0, 0, 0);
        }

        /// <summary>
        /// Задаёт начальные цвета для каждого шага.
        /// </summary>
        protected void SetColors()
        {
            s_brushes.Clear();
            s_brushes.Add(new(s_colorStart.SelectedColor!.Value));
            int steps = (int) s_sliderRecursionDepth.Value - 1;
            if (steps > 0)
            {
                int deltaR = (s_colorEnd.SelectedColor!.Value.R - s_colorStart.SelectedColor!.Value.R) / (steps);
                int deltaG = (s_colorEnd.SelectedColor!.Value.G - s_colorStart.SelectedColor!.Value.G) / (steps);
                int deltaB = (s_colorEnd.SelectedColor!.Value.B - s_colorStart.SelectedColor!.Value.B) / (steps);
                for (int i = 1; i < steps; ++i)
                {
                    SolidColorBrush back = s_brushes[^1];
                    s_brushes.Add(new SolidColorBrush(Color.FromRgb(
                        Convert.ToByte(back.Color.R + deltaR),
                        Convert.ToByte(back.Color.G + deltaG),
                        Convert.ToByte(back.Color.B + deltaB))));
                }

                s_brushes.Add(new(s_colorEnd.SelectedColor!.Value));
            }
        }

        /// <summary>
        /// Нужен для изменения содержимого панели дополнительных параметров.
        /// </summary>
        protected abstract void ChangeAdditionalParametres();

        /// <summary>
        /// Действия, совершающиеся перед тем, как начать рисовать фрактал на канвасе.
        /// </summary>
        public abstract void PrepareDraw();
    }
}