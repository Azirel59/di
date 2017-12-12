using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.Injection;
using FractalPainting.Infrastructure.UiActions;

namespace FractalPainting.App.Actions
{
	public class ImageSettingsAction : IUiAction, INeed<IImageHolder>
	{
		private IImageHolder imageHolder;
		private readonly ImageSettings imageSettings;

	    public ImageSettingsAction(ImageSettings imageSettings)
	    {
	        this.imageSettings = imageSettings;
	    }

	    public void SetDependency(IImageHolder dependency)
		{
			imageHolder = dependency;
		}
        
		public string Category => "Настройки";
		public string Name => "Изображение...";
		public string Description => "Размеры изображения";

		public void Perform()
		{
			SettingsForm.For(imageSettings).ShowDialog();
			imageHolder.RecreateImage(imageSettings);
		}
	}
}