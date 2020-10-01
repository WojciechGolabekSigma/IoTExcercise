using IoTExcercise.Dtos;
using IoTExcercise.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTExcercise.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class SensorsController : ControllerBase
    {

        private readonly ISensorsService sensorsService;

        public SensorsController(ISensorsService sensorsService)
        {
            this.sensorsService = sensorsService;
        }

        [HttpGet]
        [Route("devices/{deviceId}/data/{date}/{sensor}")]
        public async Task<ActionResult<SingleSensorResponseDto>> GetSingleSensorData(string deviceId, DateTime date, string sensor)
        {
            var singleSensorData = await sensorsService.GetSingleSensorData(date, deviceId, sensor);
            if (singleSensorData == null || !singleSensorData.SensorReadings.Any())
            {
                return NoContent();
            }

            return Ok(singleSensorData);
        }

        [HttpGet]
        [Route("devices/{deviceId}/data/{date}")]
        public async Task<ActionResult<List<SingleSensorResponseDto>>> GetAllSensorsData(string deviceId, DateTime date)
        {
            var response  = await sensorsService.GetAllSensorsDataForDevice(date, deviceId);
            if (response == null || !response.Any())
            {
                return NoContent();
            }
            return response;
        }
    }
} 

