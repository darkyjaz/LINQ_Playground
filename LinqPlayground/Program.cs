using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LinqPlayground {

    class CarData {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        
        [JsonPropertyName("car_make")]
        public string Make { get; set; }

        [JsonPropertyName("car_models")]
        public string Model { get; set; }

        [JsonPropertyName("car_year")]
        public int Year { get; set; }

        [JsonPropertyName("number_of_doors")]
        public int NumberOfDoors { get; set; }

        [JsonPropertyName("hp")]
        public int HP { get; set; }
    }

    class Program {

        static async Task Main(string[] args) {

            var file_content = await File.ReadAllTextAsync("data.json");
            var cars = JsonSerializer.Deserialize<IEnumerable<CarData>>(file_content);

            // Print cars with at least 4 doors
            var car_with_4_doors = cars.Where(car => car.NumberOfDoors >= 4).Select(x => x);

            // Print Mazda cars with 4 doors
            var mazda_with_4_doors = cars.Where(car => car.Make == "Mazda" && car.NumberOfDoors >= 4).Select(x => x);

            // Print Make + Model for all Makes that start with "M"
            var make_plus_model = cars.Where(car => car.Make.StartsWith("M"))
                .Select(car => $"{car.Make} {car.Model}");

            // Find 10 most powerful cars (in terms of hp)
            var most_powerful_cars = cars.OrderByDescending(car => car.HP).Take(10);

            // Display the number of models "per make" after 1995
            var res = cars.Where(car => car.Year > 1995)
                // it creates a list of list where each list (group) has key of car.Make
                .GroupBy(car => car.Make).Select(group => $"{group.Key} {group.Count()}");

            // sample solution
            cars.Where(car => car.Year > 1995)
                            .GroupBy(car => car.Make)
                            // create a new anonymous type
                            .Select(c => new { c.Key, NumberOfModels = c.Count() })
                            .ToList()
                            .ForEach(item => Console.WriteLine($"{item.Key}: {item.NumberOfModels}"));

            // Display the number of models "per make" after 2008, Makes with no models should be 0
            var res_3 = cars.GroupBy(car => car.Make)
                            .Select(group => 
                                // The "where" clause is specified against each group
                                new { group.Key, NumberOfModels = group.Where(car => car.Year > 2008).Count()});

            // Count can also take a lambda method
            var res_4 = cars.GroupBy(car => car.Make)
                            .Select(group =>
                                // The "where" clause is specified against each group
                                new { group.Key, NumberOfModels = group.Count(car => car.Year > 2008) });


            // Display a list of makes that have at least 2 models >= 400 hp
            var res_5 = cars.GroupBy(car => car.Make)
                            .Where(group => group.Count(car => car.HP >= 400) >= 2);

            // sample solution to above
            var res_6 = cars.Where(car => car.HP >= 400)
                .GroupBy(car => car.Make)
                        .Select(group =>
                            // The "where" clause is specified against each group
                            new { Make = group.Key, NumberOfPowerCars = group.Count()})
                        .Where(make => make.NumberOfPowerCars >= 2);

            // Display the average hp per make
            var average_hp_per_make = cars.GroupBy(car => car.Make)
                                          .Select(group => group.Average(car => car.HP));

            // sample solution to above
            var res_7 = cars.GroupBy(car => car.Make)
                            .Select(group => new { Make = group.Key, AvergaeHP = group.Average(car => car.HP) });

















        }
    }
}
