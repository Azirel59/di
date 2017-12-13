using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.FileParsers;

namespace TagsCloudConsoleClient.Options
{
    internal class FileParserPicker: OptionPicker<IFileParser>
    {
        public FileParserPicker(IEnumerable<IFileParser> options) : base(options)
        {
        }
    }
}
