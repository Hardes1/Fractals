using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FractalPeer.Fractals;
using FractalPeer.Fractals.Base;
using FractalPeer.Fractals.TypeFractals;
using Xceed.Wpf.Toolkit;

#pragma warning disable

namespace FractalPeer
{
    /// <summary>
    /// Класс главного окна.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Экземпляры классов-фракталов-одиночек.
        /// </summary>
        private readonly CantorSetFractal _cantorSetFractal;
        private readonly KochCurveFractal _kochCurveFractal;
        private readonly SerpinskiyCarpetFractal _serpinskiyCarpetFractal;
        private readonly SerpinskiyTriangleFractal _serpinskiyTriangleFractal;
        private readonly TreeFractal _treeFractal;

        /// <summary>
        /// Конструктор класса, в котором инициализируются значения.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _cantorSetFractal = CantorSetFractal.GetInstance();
            _kochCurveFractal = KochCurveFractal.GetInstance();
            _serpinskiyCarpetFractal = SerpinskiyCarpetFractal.GetInstance();
            _serpinskiyTriangleFractal = SerpinskiyTriangleFractal.GetInstance();
            _treeFractal = TreeFractal.GetInstance();
            _treeFractal.Initialize(DrawPaneCanvas, RecursionDepthSlider, MiddleStackPanel, ColorStart, ColorEnd);
        }

        /// <summary>
        /// Происходит при изменении типа фрактал.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void FractalType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DrawPaneCanvas.Children.Clear();
            if (RecursionDepthSlider is not null)
            {
                switch (FractalType.SelectedIndex)
                {
                    case 0:
                        RecursionDepthSlider.Maximum = Constants.MaxTreeFractal;
                        RecursionDepthSlider.Value = Constants.ValueTreeFractal;
                        _treeFractal.PrepareDraw();
                        break;
                    case 1:
                        RecursionDepthSlider.Maximum = Constants.MaxKochFractal;
                        RecursionDepthSlider.Value = Constants.ValueKochFractal;
                        _kochCurveFractal.PrepareDraw();
                        break;
                    case 2:
                        RecursionDepthSlider.Maximum = Constants.MaxCarpetFractal;
                        RecursionDepthSlider.Value = Constants.ValueCarpetFractal;
                        _serpinskiyCarpetFractal.PrepareDraw();
                        break;
                    case 3:
                        RecursionDepthSlider.Maximum = Constants.MaxTriangleFractal;
                        RecursionDepthSlider.Value = Constants.ValueTriangleFractal;
                        _serpinskiyTriangleFractal.PrepareDraw();
                        break;
                    case 4:
                        RecursionDepthSlider.Maximum = Constants.MaxSetFractal;
                        RecursionDepthSlider.Value = Constants.ValueSetFractal;
                        _cantorSetFractal.PrepareDraw();
                        break;
                }
            }
        }


        /// <summary>
        /// Происходит при нажатии на кнопку сохранения.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void SaveFractalButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Document", // Default file name
                DefaultExt = ".jpg", // Default file extension
                Filter = "Text documents (.png)|*.png|(.jpg)|*.jpg" // Filter files by extension
            };

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                RenderTargetBitmap rtb = new RenderTargetBitmap((int) DrawPaneCanvas.RenderSize.Width,
                    (int) DrawPaneCanvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
                rtb.Render(DrawPaneCanvas);
                var crop = new CroppedBitmap(rtb,
                    new Int32Rect(0, 0, (int) DrawPaneCanvas.ActualWidth, (int) DrawPaneCanvas.ActualHeight));
                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(crop));
                using (var fs = System.IO.File.OpenWrite(filename))
                {
                    pngEncoder.Save(fs);
                }
            }
        }


        /// <summary>
        /// Выбирает какой фрактал нарисовать
        /// </summary>
        private void ChooseFractalToDraw()
        {
            if (_treeFractal is not null)
            {
                switch (FractalType.SelectedIndex)
                {
                    case 0:
                        _treeFractal.PrepareDraw();
                        break;
                    case 1:
                        _kochCurveFractal.PrepareDraw();
                        break;
                    case 2:
                        _serpinskiyCarpetFractal.PrepareDraw();
                        break;
                    case 3:
                        _serpinskiyTriangleFractal.PrepareDraw();
                        break;
                    case 4:
                        _cantorSetFractal.PrepareDraw();
                        break;
                }
            }
        }

        /// <summary>
        /// Происходит при изменении глубины рекурсии рисования.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void RecursionDepthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (DrawPaneCanvas is not null)
            {
                ChooseFractalToDraw();
            }
        }

        /// <summary>
        /// Происходит при изменении размера окна.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChooseFractalToDraw();
        }


        /// <summary>
        /// Происходит при изменении стартового цвета.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void ColorStart_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ChooseFractalToDraw();
        }

        /// <summary>
        /// Происходит при изменении конечного цвета.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Дополнительные параметры.</param>
        private void ColorEnd_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            ChooseFractalToDraw();
        }
    }
}