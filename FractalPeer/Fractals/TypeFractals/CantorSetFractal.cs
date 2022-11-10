using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FractalPeer.Fractals.Base;

#pragma warning disable
namespace FractalPeer.Fractals.TypeFractals
{
    /// <summary>
    /// Реализует фрактал "Множество Кантора" и паттерн Singleton.
    /// </summary>
    internal class CantorSetFractal : FractalWithParametres
    {
        /// <summary>
        /// Одиночный экземпляр класса.
        /// </summary>
        private static readonly CantorSetFractal s_instance = new CantorSetFractal();

        /// <summary>
        /// Слайдер изменения расстояния.
        /// </summary>
        private Slider SliderDistance { get; set; }

        /// <summary>
        /// Приватный конструктор класса.
        /// </summary>
        private CantorSetFractal()
        {
        }

        /// <summary>
        /// Для реализации паттерна Singleton.
        /// </summary>
        /// <returns>Возвращает единственный экземпляр данного класса.</returns>
        public static CantorSetFractal GetInstance()
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
            CreateDistanceLabel();
            CreateDistanceSlider();
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
        /// Инициализирует лейбл "Расстояние".
        /// </summary>
        private void CreateDistanceLabel()
        {
            Label label = new Label();
            label.Content = Constants.Distance;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.FontSize = Constants.SecondFontSize;
            s_stackPanel.Children.Add(label);
        }

        /// <summary>
        /// Инициализирует слайдер для настройки расстояния.
        /// </summary>
        private void CreateDistanceSlider()
        {
            SliderDistance = new Slider();
            SliderDistance.HorizontalAlignment = HorizontalAlignment.Stretch;
            SliderDistance.Minimum = Constants.SliderDistanceMinimum;
            SliderDistance.Maximum = Constants.SliderDistanceMaximum;
            SliderDistance.Value = Constants.SliderDistanceDefaultValue;
            SliderDistance.TickFrequency = Constants.SliderDistanceDefaultFrequency;
            SliderDistance.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.BottomRight;
            SliderDistance.Orientation = Orientation.Horizontal;
            SliderDistance.IsSnapToTickEnabled = true;
            SliderDistance.ValueChanged += SliderDistance_ValueChanged;
            TextBlock text = new TextBlock();
            text.HorizontalAlignment = HorizontalAlignment.Center;
            Binding b = new Binding();
            b.Source = SliderDistance;
            b.Path = new PropertyPath("Value", SliderDistance.Value);
            b.Mode = BindingMode.OneWay;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            text.SetBinding(TextBlock.TextProperty, b);
            s_stackPanel.Children.Add(SliderDistance);
            s_stackPanel.Children.Add(text);
        }

        /// <summary>
        /// Происходит при изменении значения на слайдере расстояния.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void SliderDistance_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PrepareDraw();
        }

        /// <summary>
        /// Происходит перед тем, как начать рисовать фрактал.
        /// </summary>
        public override void PrepareDraw()
        {
            s_canvas.Children.Clear();
            SetColors();
            if (!s_stackPanel.Children.Contains(SliderDistance))
                ChangeAdditionalParametres();
            double height = s_canvas.ActualHeight;
            double width = s_canvas.ActualWidth;
            double length = 2 * width / 3;
            Draw(new(width / 3 - width / 6, height / 10), length);
        }

        /// <summary>
        /// Рекурсивный метод рисования фрактала.
        /// </summary>
        /// <param name="start">Начальная точка.</param>
        /// <param name="length">Длина отрекза.</param>
        /// <param name="currentIteration">Номер текущей итерации.</param>
        private void Draw(Point start, double length, int currentIteration = 1)
        {
            if (currentIteration > (int) s_sliderRecursionDepth.Value)
                return;
            DrawPolygon(start, new Point(start.X + length, start.Y + Constants.CantorWidth),
                s_brushes[currentIteration - 1]);
            Draw(new(start.X, start.Y + Constants.CantorWidth + SliderDistance.Value), length / 3,
                currentIteration + 1);
            Draw(new(start.X + 2 * length / 3, start.Y + Constants.CantorWidth + SliderDistance.Value), length / 3,
                currentIteration + 1);
        }
    }
}