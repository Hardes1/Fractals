#pragma warning disable
namespace FractalPeer.Fractals.Base
{
    /// <summary>
    /// Родительский класс для тех фракталов, у которых есть дополнительные параметры отрисовки фрактала. 
    /// </summary>
    internal abstract class FractalWithParametres : Fractal
    {
        /// <summary>
        /// Добавляет элементы для настройки своих параметров.
        /// </summary>
        protected abstract void AddOwnControls();

        /// <summary>
        /// Создаёт хедер "дополнительные параметры".
        /// </summary>
        protected abstract void CreateHeader();
    }
}