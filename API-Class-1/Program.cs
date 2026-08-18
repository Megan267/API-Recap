namespace API_Class_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(
                        "/openapi/v1.json",
                        "Pet Adoption API v1");

                    // Optional: Sets Swagger UI as the root launch page (localhost:7245/)
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // In-memory pet data.
            // The data is stored in a List and will be lost when the application is restarted.
            var pets = new List<Pet>
            {
                new Pet
                {
                    Id = 1,
                    Name = "Luna",
                    Type = "Dog",
                    Breed = "Labrador",
                    Age = 3,
                    Status = "Available"
                },

                new Pet
                {
                    Id = 2,
                    Name = "Milo",
                    Type = "Cat",
                    Breed = "Siamese",
                    Age = 2,
                    Status = "Adopted"
                },

                new Pet
                {
                    Id = 3,
                    Name = "Coco",
                    Type = "Dog",
                    Breed = "Poodle",
                    Age = 5,
                    Status = "Available"
                },

                new Pet
                {
                    Id = 4,
                    Name = "Nala",
                    Type = "Cat",
                    Breed = "Persian",
                    Age = 1,
                    Status = "Adopted"
                }
            };

            // GET all pets.
            // Returns every pet in the list.
            app.MapGet("/pets", () =>
            {
                return Results.Ok(pets);
            });

            // GET pet by ID.
            // Finds and returns a specific pet using its ID.
            app.MapGet("/pets/{id:int}", (int id) =>
            {
                var pet = pets.FirstOrDefault(p => p.Id == id);

                // If the pet does not exist, return 404 Not Found.
                if (pet == null)
                {
                    return Results.NotFound();
                }
                else
                {
                    // Return the pet with a 200 OK response.
                    return Results.Ok(pet);
                }
            });

            // POST create a new pet.
            // Adds a new pet to the in-memory list.
            app.MapPost("/pets", (Pet pet) =>
            {
                // If the list is empty, start the ID at 1.
                if (pets.Count == 0)
                {
                    pet.Id = 1;
                }
                else
                {
                    // Find the highest existing ID and add 1.
                    pet.Id = pets.Max(p => p.Id) + 1;
                }

                // New pets are automatically marked as Available.
                pet.Status = "Available";

                // Add the new pet to the list.
                pets.Add(pet);

                // Return 201 Created with the new pet.
                return Results.Created($"/pets/{pet.Id}", pet);
            });

            // PUT update a pet.
            // Updates the details of an existing pet.
            app.MapPut("/pets/{id:int}", (int id, Pet updatedPet) =>
            {
                // Find the pet using the ID from the URL.
                var pet = pets.FirstOrDefault(p => p.Id == id);

                // If the pet does not exist, return 204 No Content.
                if (pet == null)
                {
                    return Results.NoContent();
                }
                else
                {
                    // Update the pet's information.
                    pet.Name = updatedPet.Name;
                    pet.Type = updatedPet.Type;
                    pet.Breed = updatedPet.Breed;
                    pet.Age = updatedPet.Age;
                    pet.Status = updatedPet.Status;

                    // Return the updated pet with a 200 OK response.
                    return Results.Ok(pet);
                }
            });

            // DELETE a pet.
            // Removes a pet from the list.
            app.MapDelete("/pets/{id:int}", (int id) =>
            {
                // Find the pet using its ID.
                var pet = pets.FirstOrDefault(p => p.Id == id);

                // If the pet does not exist, return 404 Not Found.
                if (pet == null)
                {
                    return Results.NotFound();
                }

                // Remove the pet from the list.
                pets.Remove(pet);

                // Return the deleted pet with a 200 OK response.
                return Results.Ok(pet);
            });

            // GET available pets.
            // Returns only pets with an Available status.
            app.MapGet("/pets/available", () =>
            {
                var availablePets = pets
                    .Where(p => p.Status == "Available")
                    .ToList();

                return Results.Ok(availablePets);
            });

            // GET pets by type.
            // Returns pets matching the specified type, such as Dog or Cat.
            app.MapGet("/pets/type/{type}", (string type) =>
            {
                // Ignore capitalisation when comparing the pet type.
                // For example, "dog", "Dog" and "DOG" are treated the same.
                var matchingPets = pets
                    .Where(p => p.Type.Equals(
                        type,
                        StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return Results.Ok(matchingPets);
            });

            // Start the application.
            app.Run();

            //200 OK means the request was successful
            //201 Created means in this case a new pet was successfully created.
            //204 No Content means the request was processed but there is no content to return.
            //404 Not Found in this case means the requested pet could not be found.
        }
    }
}