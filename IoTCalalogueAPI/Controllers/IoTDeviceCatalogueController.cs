using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace IoTCalalogueAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IoTDeviceCatalogueController : ControllerBase
    {
        private List<IoTDevice> catalogue = new();

        private JsonDBHelper jsonDBHelper = new();

        private readonly ILogger<IoTDeviceCatalogueController> _logger;

        public IoTDeviceCatalogueController(ILogger<IoTDeviceCatalogueController> logger)
        {
            _logger = logger;

            // load the catalogue from the json database each time the API is called
            catalogue = jsonDBHelper.LoadFromDB();
        }

        [HttpGet(Name = "GetCatalogue")]
        public IActionResult GetCatalogue()
        {
            if(!Request.Headers.TryGetValue("SecurityToken", out StringValues headerValue))
            {
                return Unauthorized("APIM Token Missing!");
            }

            if (headerValue.FirstOrDefault() != "123456")
            {
                return Unauthorized("APIM Token Missing!");
            }

            return Ok(catalogue);
        }

        [HttpGet("{deviceId}",  Name = "GetDevice")]
        public IoTDevice GetDevice(int deviceId)
        {
            var device = catalogue.SingleOrDefault(c => c.Id == deviceId);

            // save changed back to the json database
            jsonDBHelper.SaveToDB(catalogue);

            return device ?? new IoTDevice();
        }

        [HttpPost(Name = "AddDevice")]
        public IoTDevice AddDevice(IoTDevice device)
        {
            if (device == null)
                return new IoTDevice();

            int newId = catalogue.Any() ? catalogue.Max(c => c.Id) + 1 : 1;
            device.Id = newId;

            catalogue.Add(device);

            // save changes back to the json database
            jsonDBHelper.SaveToDB(catalogue);

            return device;
        }

        [HttpPut("{deviceId}/{newPrice}", Name = "UpdatePrice")]
        public IoTDevice UpdatePrice(int deviceId, double newPrice)
        {
            var device = catalogue.SingleOrDefault(c => c.Id == deviceId);
            if (device == null)
                return new IoTDevice();

            device.Price = newPrice;

            // save changes back to the json database
            jsonDBHelper.SaveToDB(catalogue);

            return device;
        }

        [HttpDelete("{deviceId}", Name = "DeleteDevice")]
        public void DeleteDevice(int deviceId)
        {
            var device = catalogue.SingleOrDefault(c => c.Id == deviceId);
            if (device != null)
            {
                catalogue.Remove(device);
            }

            // save changes back to the json database
            jsonDBHelper.SaveToDB(catalogue);
        }
    }
}