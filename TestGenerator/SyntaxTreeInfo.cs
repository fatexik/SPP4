using System.Collections.Generic;

namespace TestGenerator
{
    public class SyntaxTreeInfo
    {
        private List<ClassInfo> сlassInfos;

        public SyntaxTreeInfo(List<ClassInfo> classInfos)
        {
            this.сlassInfos = classInfos;
        }

        public List<ClassInfo> getClassInfos()
        {
            return this.сlassInfos;
        }
    }
}