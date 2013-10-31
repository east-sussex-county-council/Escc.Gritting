using System.Collections.Generic;

namespace Escc.Gritting.Fakes
{
    /// <summary>
    /// A service to store and retrieve fake test data about gritters
    /// </summary>
    public class FakeRepository : IGritterDataRepository
    {
        /// <summary>
        /// Reads data about all gritters including their current position
        /// </summary>
        /// <returns></returns>
        public ICollection<Gritter> ReadAllGritters()
        {
            var gritters = new List<Gritter>();

            gritters.Add(new Gritter()
                {
                    GritterId = "1",
                    GritterName = "Snowy Joey",
                    Latitude = 50.76675,
                    Longitude = 0.156443
                });

            gritters.Add(new Gritter()
                {
                    GritterId = "2",
                    GritterName = "Gritney Spears",
                    Latitude = 50.95674,
                    Longitude = 0.256442
                });

            gritters.Add(new Gritter()
                {
                    GritterId = "3",
                    GritterName = "Harry Pothole",
                    Latitude = 50.96674,
                    Longitude = 0.545443
                });

            return gritters;
        }
    }
}
