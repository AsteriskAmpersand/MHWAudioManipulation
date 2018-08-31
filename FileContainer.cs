using System;
using System.IO;

namespace Audio_Manip
{

    public class FileContainer
    {
        public readonly string Path;
        public readonly string Name;
        public readonly SymbolicFile[] Files;

        public FileContainer(string _path, string _name, Format level)
        {
            if (!Directory.Exists(System.IO.Path.Combine(Path, Name)))
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist at {1}", Name, Path));
            }
            Path = _path;
            Name = _name;
            Files = HelperFunctions.GetRelevantFiles(System.IO.Path.Combine(Path, Name), level).FunctionApply( p => new SymbolicFile(System.IO.Path.GetFileName(p)));
        }

        public void Link(SymbolicFile parent)
        {
            Array.ForEach(Files, x => x.Link(parent));
        }

        public void Unpack()
        {
            Array.ForEach(Files, x => x.Unpack());
        }

        public FileContainer GetSkelleton()
        {
            return this;
        }
    }
}
