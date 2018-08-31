using System;
using System.Collections.Generic;
using System.IO;

namespace Audio_Manip
{
    public class DirectoryTree
    {
        public bool Expanded;
        public DirectoryTree Parent;
        public FileContainer node;
        private List<DirectoryTree> children;
        
        public DirectoryTree(string root, DirectoryTree _parent = null)
        {
            if (!Directory.Exists(root)){throw new ArgumentException(string.Format("Directory does not exist at {1}", root));}
            Expanded = false;
            node = new FileContainer(Path.GetDirectoryName(root), Path.GetFileName(root), new Format());
            Parent = _parent;
            string[] directories = HelperFunctions.GetTrueDirectories(root);
            children = new List<DirectoryTree>();
            foreach(string folder in directories)
            {
                children.Add(new DirectoryTree(Path.Combine(root,folder)));
            }
        }

       
  
        public void Unpack()
        {
            foreach (SymbolicFile file in node.Files)
            {
                file.Unpack();
            }
            foreach (DirectoryTree folder in children)
            {
                folder.Unpack();
            }
        }
    }
}