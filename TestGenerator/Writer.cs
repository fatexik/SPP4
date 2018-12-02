using System;
using System.IO;
using System.Threading.Tasks;


namespace TestGenerator
{
    public class Writer
    {
        private string directoryPath;

        public Writer(string directoryPath)
        {
            this.directoryPath = directoryPath;

            if (!Directory.Exists(this.directoryPath))
            {
                Directory.CreateDirectory(this.directoryPath);
            }
        }

        public async Task write(TestClassSignature generatedClassCode)
        {
            string filePath = directoryPath + "\\" + generatedClassCode.getTestClassName();
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                await streamWriter.WriteAsync(generatedClassCode.getTestClassData());
            }
        }
    }
}