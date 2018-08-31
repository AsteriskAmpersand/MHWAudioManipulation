using System;
using System.ComponentModel;
using System.IO;

namespace Audio_Manip
{
    public class SymbolicFile
    {
        private Format format;
        public string DisplayName;
        private string filename;
        private FileContainer _folder;
        private SymbolicFile parent;

        public SymbolicFile(string filename, SymbolicFile parent = null)
        {
            this.parent = parent;
            if (!File.Exists(filename))
            {
                throw new ArgumentException(string.Format("File {0} does not exist", filename));
            }
            this.filename = filename;
            format = Format.Guess(filename);
          
            _folder = null;
            string asocfolder = AssociatedFolder(filename);
            if (Directory.Exists(asocfolder))
            {
                _folder = new FileContainer(Path.GetDirectoryName(asocfolder),Path.GetFileName(asocfolder),format.LowerLevel());
            }

            DisplayName = DefaultStructure.GetDisplayName(filename); //TODO - DefaultStructure class
        }

        public void Unpack()
        {
            if (_folder!=null)
            {
                if (_folder.GetSkelleton() == DefaultStructure.GetSkelleton(filename)){_folder.Link(this);} //TODO - Decide on what to actually make this comparison behave like
                else{throw new InvalidOperationException(string.Format("Corrupted Structure at {0}",_folder.ToString()));}
            }
            else
            {
                string asocfolder = AssociatedFolder(filename);
                Directory.CreateDirectory(asocfolder);
                format.Unpack(filename, asocfolder);
                Format nextformat = format.LowerLevel();
                if (nextformat.Unpackable())
                {
                    _folder = new FileContainer(Path.GetDirectoryName(asocfolder), Path.GetFileName(asocfolder), nextformat);
                    _folder.Link(this);
                    _folder.Unpack();
                }
            }
        }

        public void Delete()
        {
            
        }

        public void Link(SymbolicFile parent)
        {
            this.parent = parent;
            //Make all of the folders also excecute the linking
        }

        private string AssociatedFolder(string path)
        {
            string filename = Path.GetFileName(path);
            string root = Path.GetDirectoryName(path);
            return Path.Combine(root, "#" + filename);
        }

    }
}