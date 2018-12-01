using System.Collections.Generic;
using System.Reflection;

namespace TestGenerator
{
    public class ClassInfo
    {
        private string namespaceName;
        private string name;
        private List<MethodInfo> methods;

        public ClassInfo(string namespaceName, string name, List<MethodInfo> methods)
        {
            this.namespaceName = namespaceName;
            this.name = name;
            this.methods = methods;
        }

        public string getNamespace()
        {
            return namespaceName;
        }

        public string getName()
        {
            return name;
        }

        public List<MethodInfo> getMethodsList()
        {
            return methods;
        }
    }
}