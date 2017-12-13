using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CommandLine.Text;
using SimpleInjector;
using TagsCloudConsoleClient.Options;
using TagsCloudContainer;
using TagsCloudContainer.FileParsers;
using TagsCloudContainer.LayoutAlgorithms;
using TagsCloudContainer.WordProcessors;
using TagsCloudContainer.WordStores;
using TagsCloudVisualization;

namespace TagsCloudConsoleClient
{
    internal class Program
    {
        private static readonly Container Container;
        private static readonly TagsCloudClientOptions Options = new TagsCloudClientOptions();

        static Program()
        {
            try
            {
                Container = new Container();
                RegisterDependencies<IFileParser>();
                RegisterDependencies<ILayoutAlgorithm>();
                RegisterDependencies<IWordProcessor>();
                Container.Register(() => Options);

                Container.Register<FileParserPicker>(Lifestyle.Singleton);
                Container.Register<LayoutAlgorithmPicker>(Lifestyle.Singleton);
                Container.Register<WordProcessorPicker>(Lifestyle.Singleton);

                Container.Register(() => Container.GetInstance<FileParserPicker>()
                .GetOption(Container.GetInstance<TagsCloudClientOptions>().FileParserIndex).Implementation);

                Container.Register(() => Container.GetInstance<LayoutAlgorithmPicker>()
                    .GetOption(Container.GetInstance<TagsCloudClientOptions>().LayoutAlgorithmIndex).Implementation);

                Container.Register(() =>
                {
                    var picker = Container.GetInstance<WordProcessorPicker>();
                    var processors = Container.GetInstance<TagsCloudClientOptions>().WordProcessorIndices ?? new long[0];
                    return new WordProcessorStack(processors.Select(p => picker.GetOption(p).Implementation));
                });

                Container.Verify();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        private static void RegisterDependencies<T>()
        {
            var targetTypes =
                AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && t.IsInterface == false && t.IsAbstract == false)
                .ToArray();
            Container.RegisterCollection(typeof(T), targetTypes);
        }

        private static void Main(string[] args)
        {
            try
            {
                var argsValid = CommandLine.Parser.Default.ParseArguments(args, Options);
                if (!argsValid)
                {
                    Console.WriteLine(HelpText.AutoBuild(Options).ToString());
                }
                if (Options.WordProcessorIndices == null || Options.WordProcessorIndices.Any() == false)
                {
                    var picker = Container.GetInstance<WordProcessorPicker>();
                    Options.WordProcessorIndices = new []
                    {
                        picker.GetOptionIndexByType(typeof(TrimSpacesProcessor)),
                        picker.GetOptionIndexByType(typeof(ToLowerCaseProcessor))
                    };
                }
                IWordStore words;
                using (var file = File.OpenRead(Options.InputFileName))
                using (var fileReader = new StreamReader(file))
                {
                    words = Container.GetInstance<IFileParser>().ParseFile(fileReader);
                }
                var cloudBuilder = new TagsCloud(
                    Container.GetInstance<ILayoutAlgorithm>(),
                    words,
                    Container.GetInstance<WordProcessorStack>().WordProcessors
                );
                var resultImage = cloudBuilder.CreateTagsCloud();
                resultImage.Save(Options.ResultFileName);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}
