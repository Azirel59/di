using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.WordStores;

namespace TagsCloudContainer.FileParsers
{
    [Description("Базовый парсер для текстовых файлов, одно слово в строке")]
    public class TextFileParser: IFileParser
    {
        public IWordStore ParseFile(StreamReader reader)
        {
            var words = new List<string>();
            while (!reader.EndOfStream)
            {
                words.Add(reader.ReadLine());
            }
            return new BasicWordStore(words);
        }
    }
}
