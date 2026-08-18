namespace API_Controllers.Model
{
    public class PetContent
    {
        // In-memory pet data.
        // The data is stored in a List and will be lost when the application is restarted.
        public static List<Pet> Pets { get; } = new List<Pet>
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
    }
}