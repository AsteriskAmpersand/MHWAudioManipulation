using System.Collections.Generic;

namespace Audio_Manip
{
    delegate void TreeVisitor<T>(T nodeData);

    class NTree<T>
    {
        private T data;
        private LinkedList<NTree<T>> children;
        private readonly NTree<T> parent;

        public NTree(T data, NTree<T> parent = null)
        {
            this.data = data;
            children = new LinkedList<NTree<T>>();
            this.parent = parent;
        }

        public void AddChild(T data)
        {
            children.AddFirst(new NTree<T>(data));
        }

        public NTree<T> GetParent(int i)
        {
            foreach (NTree<T> n in children)
                if (--i == 0)
                    return n;
            return null;
        }
        
        public NTree<T> GetChild(int i)
        {
            foreach (NTree<T> n in children)
                if (--i == 0)
                    return n;
            return null;
        }

        public void Traverse(NTree<T> node, TreeVisitor<T> visitor)
        {
            visitor(node.data);
            foreach (NTree<T> kid in node.children)
                Traverse(kid, visitor);
        }
    }
}
//Taken from https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp - by Steve Morgan