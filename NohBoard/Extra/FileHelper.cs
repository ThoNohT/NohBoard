namespace ThoNohT.NohBoard.Extra
{
    using System.Drawing;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml;
    using ThoNohT.NohBoard.Keyboard;

    public static class FileHelper
    {
        public static void EnsurePathExists(string path)
        {
            var file = new FileInfo(path);
            var dir = file.Directory;

            if (dir.Exists) return;

            EnsurePathExists(dir.Parent.FullName);
            dir.Create();
        }

        public static T Deserialize<T>(string filename) where T: class
        {
            if (!File.Exists(filename)) return null;

            using (var fileStream = new FileStream(filename, FileMode.Open))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var reader = JsonReaderWriterFactory.CreateJsonReader(
                    fileStream,
                    Encoding.UTF8,
                    XmlDictionaryReaderQuotas.Max,
                    dictionaryReader => { }))
                {
                    return (T)serializer.ReadObject(reader);
                }
            }
        }

        public static void Serialize<T>(string filename, T obj)
        {
            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(fileStream, Encoding.UTF8))
                {
                    serializer.WriteObject(writer, obj);
                }
            }
        }
    }
}