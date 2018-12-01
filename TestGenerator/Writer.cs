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

        public async Task write(string fileName)
        {
            string filePath = directoryPath + "\\" + fileName;
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                await streamWriter.WriteAsync(fileName); 
            }
        }
    }
}