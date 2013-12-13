using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LotusEngine
{
    public class SceneData
    {
        public SceneData(FileInfo file)
        {
            name = file.Name;
            path = file.FullName;
        }

        public readonly string name;
        public readonly string path;
    }
}
