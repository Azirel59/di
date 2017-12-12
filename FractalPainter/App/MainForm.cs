using System;
using System.Drawing;
using System.Windows.Forms;
using FractalPainting.App.Actions;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.Injection;
using FractalPainting.Infrastructure.UiActions;
using Ninject;

namespace FractalPainting.App
{
	public class MainForm : Form
	{
		public MainForm(IUiAction[] actions, PictureBoxImageHolder pictureBox, Palette pallete, AppSettings appSettings)
		{
			var imageSettings = appSettings.ImageSettings;
			ClientSize = new Size(imageSettings.Width, imageSettings.Height);

			var mainMenu = new MenuStrip();
			mainMenu.Items.AddRange(actions.ToMenuItems());
			Controls.Add(mainMenu);

			pictureBox.RecreateImage(imageSettings);
			pictureBox.Dock = DockStyle.Fill;
			Controls.Add(pictureBox);

			DependencyInjector.Inject<IImageHolder>(actions, pictureBox);
			DependencyInjector.Inject<IImageDirectoryProvider>(actions, appSettings);
			DependencyInjector.Inject(actions, imageSettings);
			DependencyInjector.Inject(actions, pallete);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Text = "Fractal Painter";
		}
	}
}