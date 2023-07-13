using System.Text.Json;
using System.Text.Json.Serialization;

namespace IoTCalalogueAPI
{
    public class JsonDBHelper
    {
        private string _jsonFile = "catalogue.json";

        public JsonDBHelper()
        {
            if (!File.Exists(_jsonFile))
            {
                File.WriteAllText(_jsonFile, JsonSerializer.Serialize(new List<IoTDevice>()));
            }
        }

        public void SaveToDB(List<IoTDevice> catalogue)
        {
            var json = JsonSerializer.Serialize(catalogue);
            File.WriteAllText(_jsonFile, json);
        }

        public List<IoTDevice> LoadFromDB()
        {
            if (!File.Exists(_jsonFile))
                return new List<IoTDevice>();

            List<IoTDevice> catalogue = 
                JsonSerializer.Deserialize<List<IoTDevice>>(File.ReadAllText(_jsonFile));

            return catalogue;
        }
    }
}
