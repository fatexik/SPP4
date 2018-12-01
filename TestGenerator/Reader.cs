using System.IO;
using System.Threading.Tasks;

namespace TestGenerator
{
    public class Reader
    {
        public async Task<string> ReadAsync(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}