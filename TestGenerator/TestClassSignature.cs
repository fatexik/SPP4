namespace TestGenerator
{
    public class TestClassSignature
    {
        private string testClassName;
        private string testClassData;

        public TestClassSignature(string testClassName, string testClassData)
        {
            this.testClassName = testClassName;
            this.testClassData = testClassData;
        }

        public string getTestClassName()
        {
            return this.testClassName;
        }

        public string getTestClassData()
        {
            return this.testClassData;
        }
    }
}