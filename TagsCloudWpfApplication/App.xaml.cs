using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SimpleInjector;
using TagsCloudContainer;
using TagsCloudContainer.FileParsers;

namespace TagsCloudWpfApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Container _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            _container = new Container();
            _container.Register<IFileParser, TextFileParser>();
            _container.Register<TagsCloud>();
            base.OnStartup(e);
        }
    }
}
