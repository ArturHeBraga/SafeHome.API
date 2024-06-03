using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SafeHome.Data;
using SafeHome.Entities;

namespace SafeHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMongoCollection<Customer> _customers;

        public CustomerController(MongoDbService mongoDbService)
        {
            _customers = mongoDbService.Database.GetCollection<Customer>("Customer");
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customers.Find(FilterDefinition<Customer>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer?>> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var filter = Builders<Customer>.Filter.Eq(x => x.Id, objectId);
            var customer = await _customers.Find(filter).FirstOrDefaultAsync();
            return customer is not null ? Ok(customer) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Customer customer)
        {
            customer.Id = ObjectId.GenerateNewId(); // Gera um novo ObjectId
            await _customers.InsertOneAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id.ToString() }, customer);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, customer.Id);
            var result = await _customers.ReplaceOneAsync(filter, customer);
            return result.MatchedCount > 0 ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid ID format.");
            }

            var filter = Builders<Customer>.Filter.Eq(x => x.Id, objectId);
            var result = await _customers.DeleteOneAsync(filter);
            return result.DeletedCount > 0 ? Ok() : NotFound();
        }
    }
}
