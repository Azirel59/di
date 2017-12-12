using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsCloudContainer.WordStores;

namespace TagsCloudContainer.FileParsers
{
    public interface IFileParser
    {
        IWordStore ParseFile(StreamReader reader);
    }
}
