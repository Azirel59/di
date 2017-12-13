using System.Collections.Generic;
using System.ComponentModel;
using CommandLine;
using CommandLine.Text;

namespace TagsCloudConsoleClient.Options
{
    internal class TagsCloudClientOptions
    {
        [Option("file", HelpText = "Название файла, содержащего слова", Required = true)]
        public string InputFileName { get; set; }

        [Option("resultFile", DefaultValue="result.png", HelpText = "Название файла с итоговым изображением")]
        public string ResultFileName { get; set; }

        [Option("parser", DefaultValue = 0, HelpText = "Способ парсинга файла")]
        public long FileParserIndex { get; set; }

        [Option("algorithm", DefaultValue = 0, HelpText = "Алгоритм раскладки слов")]
        public long LayoutAlgorithmIndex { get; set; }

        [Option("filters", HelpText = "Применяемые обработчики слов")]
        public IEnumerable<long> WordProcessorIndices { get; set; }
    }
}
