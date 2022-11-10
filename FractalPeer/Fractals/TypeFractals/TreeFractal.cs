using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FractalPeer.Fractals.Base;
using static System.Math;

#pragma warning disable
namespace FractalPeer.Fractals.TypeFractals
{
    /// <summary>
    /// Реализует фрактал "Фрактальное дерево" и паттерн Singleton.
    /// </summary>
    internal class TreeFractal : FractalWithParametres
    {
        /// <summary>
        /// Одиночный экземпляр класса.
        /// </summary>
        private static readonly TreeFractal s_instance = new TreeFractal();


        /// <summary>
        /// Слайдер коэффициента.
        /// </summary>
        private Slider SliderCoefficient { get; set; }

        /// <summary>
        /// Слайдер левого угла наклона.
        /// </summary>
        private Slider SliderAngleLeft { get; set; }

        /// <summary>
        /// Слайдер правого угла наклона.
        /// </summary>
        private Slider SliderAngleRight { get; set; }

        /// <summary>
        /// Приватный конструктор класса.
        /// </summary>
        private TreeFractal()
        {
        }

        /// <summary>
        /// Для реализации паттерна Singleton.
        /// </summary>
        /// <returns>Возвращает единственный экземпляр данного класса.</returns>
        public static TreeFractal GetInstance()
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
            AddOwnControls();
        }

        /// <summary>
        /// Добавляет собственные элементы управления на панель дополнительных параметров.
        /// </summary>
        protected override void AddOwnControls()
        {
            CreateHeader();
            CreateCoefficientLabel();
            CreateCoefficientSlider();
            CreateAngleLeftLabel();
            CreateAngleLeftSlider();
            CreateAngleRightLabel();
            CreateAngleRightSlider();
        }


        /// <summary>
        /// Создаёт хедер "Дополнительные параметры".
        /// </summary>
        protected override void CreateHeader()
        {
            Label header = new Label();
            header.Content = Constants.ParametresHeader;
            header.FontSize = Constants.MainFontSize;
            header.FontWeight = FontWeights.Bold;
            header.HorizontalAlignment = HorizontalAlignment.Center;
            s_stackPanel.Children.Add(header);
        }


        /// <summary>
        /// Создаёт текст для коэффициента.
        /// </summary>
        private void CreateCoefficientLabel()
        {
            Label label = new Label();
            label.Content = Constants.Coefficient;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.FontSize = Constants.SecondFontSize;
            s_stackPanel.Children.Add(label);
        }

        /// <summary>
        /// Создаёт слайдер для левого угла наклона.
        /// </summary>
        private void CreateAngleLeftSlider()
        {
            SliderAngleLeft = new Slider();
            SliderAngleLeft.HorizontalAlignment = HorizontalAlignment.Stretch;
            SliderAngleLeft.Minimum = Constants.SliderAngleMinimum;
            SliderAngleLeft.Maximum = Constants.SliderAngleMaximum;
            SliderAngleLeft.Value = Constants.SliderAngleDefaultValue;
            SliderAngleLeft.TickFrequency = Constants.SliderAngleDefaultFrequency;
            SliderAngleLeft.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
            SliderAngleLeft.Orientation = Orientation.Horizontal;
            SliderAngleLeft.IsSnapToTickEnabled = true;
            SliderAngleLeft.ValueChanged += Slider_ValueChanged;
            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            Binding b = new Binding();
            b.Source = SliderAngleLeft;
            b.Path = new PropertyPath("Value", SliderAngleLeft.Value);
            b.Mode = BindingMode.OneWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            text.SetBinding(TextBlock.TextProperty, b);
            s_stackPanel.Children.Add(SliderAngleLeft);
            s_stackPanel.Children.Add(text);
        }

        /// <summary>
        /// Происходит при изменении значения на каком-то из слайдеров доп параметров.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PrepareDraw();
        }

        /// <summary>
        /// Создаёт текст для левого угла наклона.
        /// </summary>
        private void CreateAngleLeftLabel()
        {
            Label label = new Label();
            label.Content = Constants.AngleLeft;
            label.FontSize = Constants.SecondFontSize;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            s_stackPanel.Children.Add(label);
        }

        /// <summary>
        /// Создаёт текст для правого угла наклона.
        /// </summary>
        private void CreateAngleRightLabel()
        {
            Label label = new Label();
            label.Content = Constants.AngleRight;
            label.FontSize = Constants.SecondFontSize;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            s_stackPanel.Children.Add(label);
        }

        /// <summary>
        /// Создаёт слайдер задания коэффициента сжатия.
        /// </summary>
        private void CreateCoefficientSlider()
        {
            SliderCoefficient = new Slider();
            SliderCoefficient.HorizontalAlignment = HorizontalAlignment.Stretch;
            SliderCoefficient.Minimum = Constants.SliderCoefficientMinimum;
            SliderCoefficient.Maximum = Constants.SliderCoefficientMaximum;
            SliderCoefficient.Value = Constants.SliderCoefficientDefaultValue;
            SliderCoefficient.TickFrequency = Constants.SliderCoefficientDefaultFrequency;
            SliderCoefficient.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
            SliderCoefficient.Orientation = Orientation.Horizontal;
            SliderCoefficient.IsSnapToTickEnabled = true;
            SliderCoefficient.ValueChanged += Slider_ValueChanged;
            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            Binding b = new Binding();
            b.Source = SliderCoefficient;
            b.Path = new PropertyPath("Value", SliderCoefficient.Value);
            b.Mode = BindingMode.OneWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            text.SetBinding(TextBlock.TextProperty, b);
            s_stackPanel.Children.Add(SliderCoefficient);
            s_stackPanel.Children.Add(text);
        }


        /// <summary>
        /// Переводит градусы в радианы.
        /// </summary>
        /// <param name="value">Значение в градусах.</param>
        /// <returns></returns>
        private double ConvertDegreeToRadian(double value) => value / 180 * Math.PI;

        /// <summary>
        /// Создаёт слайдер для правого угла наклона.
        /// </summary>
        private void CreateAngleRightSlider()
        {
            SliderAngleRight = new Slider();
            SliderAngleRight.HorizontalAlignment = HorizontalAlignment.Stretch;
            SliderAngleRight.Minimum = Constants.SliderAngleMinimum;
            SliderAngleRight.Maximum = Constants.SliderAngleMaximum;
            SliderAngleRight.Value = Constants.SliderAngleDefaultValue;
            SliderAngleRight.TickFrequency = Constants.SliderAngleDefaultFrequency;
            SliderAngleRight.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
            SliderAngleRight.Orientation = Orientation.Horizontal;
            SliderAngleRight.IsSnapToTickEnabled = true;
            SliderAngleRight.ValueChanged += Slider_ValueChanged;
            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            Binding b = new Binding();
            b.Source = SliderAngleRight;
            b.Path = new PropertyPath("Value", SliderAngleRight.Value);
            b.Mode = BindingMode.OneWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            text.SetBinding(TextBlock.TextProperty, b);
            s_stackPanel.Children.Add(SliderAngleRight);
            s_stackPanel.Children.Add(text);
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
            double length = Math.Min(height / 3, width / 3);
            if (!s_stackPanel.Children.Contains(SliderCoefficient))
                ChangeAdditionalParametres();
            Draw(new(width / 2, height), length, PI / 2);
        }

        /// <summary>
        /// Рекурсивный метод рисования фрактала.
        /// </summary>
        /// <param name="currentPosition">Текущая позиция.</param>
        /// <param name="length">Длина отрезка.</param>
        /// <param name="angle">текущий угол наклона.</param>
        /// <param name="currentIteration">Номер текущей итерации.</param>
        private void Draw(Point currentPosition, double length, double angle, int currentIteration = 1)
        {
            if (currentIteration > (int) s_sliderRecursionDepth.Value)
                return;

            else if (currentIteration == 1)
            {
                Point point = new Point(currentPosition.X + length * Cos(angle),
                    currentPosition.Y - length * Sin(angle));
                DrawLine(currentPosition, point, s_brushes[currentIteration - 1]);
                Draw(point, length * SliderCoefficient.Value, angle, currentIteration + 1);
            }
            else
            {
                double leftAngle = angle + ConvertDegreeToRadian(SliderAngleLeft.Value);
                double rightAngle = angle - ConvertDegreeToRadian(SliderAngleRight.Value);
                Point pointLeft = new Point(currentPosition.X + length * Cos(leftAngle),
                    currentPosition.Y - length * Sin(leftAngle));
                DrawLine(currentPosition, pointLeft, s_brushes[currentIteration - 1]);
                Point pointRight = new Point(currentPosition.X + length * Cos(rightAngle),
                    currentPosition.Y - length * Sin(rightAngle));
                DrawLine(currentPosition, pointRight, s_brushes[currentIteration - 1]);
                Draw(pointLeft, length * SliderCoefficient.Value, leftAngle, currentIteration + 1);
                Draw(pointRight, length * SliderCoefficient.Value, rightAngle, currentIteration + 1);
            }
        }
    }
}