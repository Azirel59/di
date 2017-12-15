using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.WordStores;

namespace TagsCloudContainer.FileParsers
{
    [Description("Продвинутый парсер для текстовых файлов, разбирает естественный текст")]
    public class NaturalTextFileParser: IFileParser
    {
        private readonly char[] Splitters = { ' ', '\n', '\r', '\'', '"', ',', '.', ':', ';', '!', '?' };
        public IWordStore ParseFile(StreamReader reader)
        {
            var words = new List<string>();
            while (!reader.EndOfStream)
            {
                var currentLine = reader.ReadLine() ?? "";
                while (currentLine.EndsWith("-"))
                {
                    currentLine = currentLine.TrimEnd('-');
                    currentLine += reader.ReadLine();
                }
                var wordsInLine = currentLine.Split(Splitters, StringSplitOptions.RemoveEmptyEntries).Where(word => word != "–");
                words.AddRange(wordsInLine);
            }
            return new BasicWordStore(words);
        }
    }
}
