using IoTExcercise.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoTExcercise.Services
{
    public interface ISensorsService
    {
        Task<SingleSensorResponseDto> GetSingleSensorData(DateTime date, string deviceId, string sensor);
        Task<List<SingleSensorResponseDto>> GetAllSensorsDataForDevice(DateTime date, string deviceId);
    }
}
