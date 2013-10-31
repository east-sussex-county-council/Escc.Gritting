using System;
using System.Collections.Generic;

namespace Escc.Gritting.Fakes
{
    /// <summary>
    /// A service to store and retrieve fake test data about gritters
    /// </summary>
    public class FakeGritterRepository : IGritterDataRepository
    {
        /// <summary>
        /// Reads data about all gritters including their current position
        /// </summary>
        /// <returns></returns>
        public ICollection<Gritter> ReadAllGritters()
        {
            var gritters = new List<Gritter>();
            var random = new Random();
            gritters.Add(new Gritter()
                {
                    GritterId = "1",
                    GritterName = "Snowy Joey",
                    Latitude = Double.Parse("50." + random.Next(850000, 999999)),
                    Longitude = Double.Parse("0." + random.Next(0, 750000))
                });

            gritters.Add(new Gritter()
                {
                    GritterId = "2",
                    GritterName = "Gritney Spears",
                    Latitude = Double.Parse("50." + random.Next(850000, 999999)),
                    Longitude = Double.Parse("0." + random.Next(0, 750000))
                });

            gritters.Add(new Gritter()
                {
                    GritterId = "3",
                    GritterName = "Harry Pothole",
                    Latitude = Double.Parse("50." + random.Next(850000, 999999)),
                    Longitude = Double.Parse("0." + random.Next(0, 750000))
                });

            return gritters;
        }
    }
}
