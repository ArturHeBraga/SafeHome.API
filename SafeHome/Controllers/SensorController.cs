using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SafeHome.Data;
using SafeHome.Entities;

namespace SafeHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly IMongoCollection<Sensor> _Sensors;

        public SensorController(MongoDbService mongoDbService)
        {
            _Sensors = mongoDbService.Database?.GetCollection<Sensor>("Sensor");
        }

        [HttpGet]
        public async Task<IEnumerable<Sensor>> Get()
        {
            return await _Sensors.Find(FilterDefinition<Sensor>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor?>> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var filter = Builders<Sensor>.Filter.Eq(x => x.Id, objectId);
            var Sensor = await _Sensors.Find(filter).FirstOrDefaultAsync();
            return Sensor is not null ? Ok(Sensor) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Sensor Sensor)
        {
            Sensor.Id = ObjectId.GenerateNewId(); // Gera um novo ObjectId
            await _Sensors.InsertOneAsync(Sensor);
            return CreatedAtAction(nameof(GetById), new { id = Sensor.Id.ToString() }, Sensor);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Sensor Sensor)
        {
            var filter = Builders<Sensor>.Filter.Eq(x => x.Id, Sensor.Id);
            var result = await _Sensors.ReplaceOneAsync(filter, Sensor);
            return result.MatchedCount > 0 ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var filter = Builders<Sensor>.Filter.Eq(x => x.Id, objectId);
            var result = await _Sensors.DeleteOneAsync(filter);
            return result.DeletedCount > 0 ? Ok() : NotFound();
        }
    }
}
