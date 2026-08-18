using API_Controllers.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Controllers.Controllers
{
    [Route("api/[controller]")]// the route is api/pets
    [ApiController]
    public class PetsController : ControllerBase
    {
        // GET all pets.
        // Returns every pet in the list.
        [HttpGet]
        public ActionResult<IEnumerable<Pet>> GetAll()
        {
            return Ok(PetContent.Pets);
        }

        // GET pet by ID.
        // Finds and returns a specific pet using its ID.
        [HttpGet("{id:int}")]
        public ActionResult<Pet> GetByID(int id)
        {
            var pet = PetContent.Pets.FirstOrDefault(p => p.Id == id);

            // If the pet does not exist, return 404 Not Found.
            if (pet is null)
            {
                return NotFound();
            }

            // Return the pet with a 200 OK response.
            return Ok(pet);
        }

        // POST create a new pet.
        // Adds a new pet to the in-memory list.
        [HttpPost]
        public ActionResult<Pet> Create([FromBody] Pet newPet)
        {
            // If the list is empty, start the ID at 1.
            if (PetContent.Pets.Count == 0)
            {
                newPet.Id = 1;
            }
            else
            {
                // Find the highest existing ID and add 1.
                newPet.Id = PetContent.Pets.Max(p => p.Id) + 1;
            }

            // New pets are automatically marked as Available.
            newPet.Status = "Available";

            // Add the new pet to the list.
            PetContent.Pets.Add(newPet);

            // Return 201 Created with the new pet.
            return CreatedAtAction(
                nameof(GetByID),
                new { id = newPet.Id },
                newPet);
        }

        // PUT update a pet.
        // Updates the details of an existing pet.
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Pet updatedPet)
        {
            // Find the pet using the ID from the URL.
            var existingPet = PetContent.Pets.FirstOrDefault(p => p.Id == id);

            // If the pet does not exist, return 404 Not Found.
            if (existingPet is null)
            {
                return NotFound();
            }

            // Update the pet's information.
            existingPet.Name = updatedPet.Name;
            existingPet.Type = updatedPet.Type;
            existingPet.Breed = updatedPet.Breed;
            existingPet.Age = updatedPet.Age;
            existingPet.Status = updatedPet.Status;

            // Return 204 No Content.
            return NoContent();
        }

        // DELETE a pet.
        // Removes a pet from the list.
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            // Find the pet using its ID.
            var pet = PetContent.Pets.FirstOrDefault(p => p.Id == id);

            // If the pet does not exist, return 404 Not Found.
            if (pet is null)
            {
                return NotFound();
            }

            // Remove the pet from the list.
            PetContent.Pets.Remove(pet);

            // Return the deleted pet with a 200 OK response.
            return Ok(pet);
        }

        // GET available pets.
        // Returns only pets with an Available status.
        [HttpGet("available")]
        public ActionResult<IEnumerable<Pet>> GetAvailable()
        {
            var availablePets = PetContent.Pets
                .Where(p => p.Status == "Available")
                .ToList();

            return Ok(availablePets);
        }

        // GET pets by type.
        // Returns pets matching the specified type, such as Dog or Cat.
        [HttpGet("type/{type}")]
        public ActionResult<IEnumerable<Pet>> GetByType(string type)
        {
            // Ignore capitalisation when comparing the pet type.
            // For example, "dog", "Dog" and "DOG" are treated the same.
            var matchingPets = PetContent.Pets
                .Where(p => p.Type.Equals(
                    type,
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(matchingPets);
        }
    }
}