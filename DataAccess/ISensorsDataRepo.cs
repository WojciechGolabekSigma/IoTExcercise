using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTExcercise.DataAccess
{
    public interface ISensorsDataRepo
    {
        Task<string[]> GetSingleSensorReadings(DateTime date, string deviceId, string sensor);
        Task<List<string>> GetSensorsTypesForDeviceFromMetadata(string deviceId);
    }
}
