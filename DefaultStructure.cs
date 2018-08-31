namespace Audio_Manip
{
    public static class DefaultStructure
    {
        private static DirectoryTree foundation;
        
        public static string GetDisplayName(string filename)
        {
            return "";
        }

        public static FileContainer GetSkelleton(string filename)
        {
            //TODO - Split string, walk path backwards until root is reached, there walk the foundation till the node is found.
            return foundation.node;
        }
        
    }
}