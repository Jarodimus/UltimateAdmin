using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateAdmin.Core.ActiveDirectory;

namespace UltimateAdmin.Core.Demo
{
    public class ComputerRepository
    {
        List<Computer> FakeADComputers;
        public ComputerRepository()
        {
            CreateComputers();
        }

        private void CreateComputers()
        {
            FakeADComputers = new List<Computer>
            {
                new Computer("COMP-AXRK40", "John Smith's PC"),
                new Computer("COMP-AXRK40", "Laura Ackerman's PC"),
                new Computer("COMP-AXRK40", "George McGee's PC"),
                new Computer("COMP-AXRK40", "Martin Morris's PC"),
                new Computer("COMP-AXRK40", "Lisa Phillips's PC"),
                new Computer("COMP-AXRK40", "Anthony Wright's PC"),
                new Computer("COMP-AXRK40", "Amanda Chang's PC"),
                new Computer("COMP-AXRK40", "August Hall's PC"),
                new Computer("COMP-AXRK40", "Michael Finley's PC"),
                new Computer("COMP-AXRK40", "Roy Hall's PC"),
                new Computer("COMP-AXRK40", "Kathleen Franks's PC"),
                new Computer("COMP-AXRK40", "Monica Collins's PC"),
                new Computer("COMP-AXRK40", "Jesse Gibson's PC"),
                new Computer("COMP-AXRK40", "Mark Harris's PC"),
                new Computer("COMP-AXRK40", "Margaret Robles's PC"),
                new Computer("COMP-AXRK40", "Alfredo Richards's PC"),
                new Computer("COMP-AXRK40", "Ricardo Delgado's PC"),
                new Computer("COMP-AXRK40", "Calvin Jones's PC"),
                new Computer("COMP-AXRK40", "Marcos Gray's PC"),
                new Computer("COMP-AXRK40", "Larry Hoffman's PC"),
                new Computer("COMP-AXRK40", "Casey Dunavant's PC"),
                new Computer("COMP-AXRK40", "Alton Brown's PC"),
                new Computer("COMP-AXRK40", "Debra Boylan's PC"),
                new Computer("COMP-AXRK40", "Sandra Carter's PC"),
                new Computer("COMP-AXRK40", "Gail Conley's PC"),
                new Computer("COMP-AXRK40", "Peter Tinsley's PC"),
                new Computer("COMP-AXRK40", "Leon Smith's PC"),
                new Computer("COMP-AXRK40", "Marian Kemp's PC"),
                new Computer("COMP-AXRK40", "Thomas Kirk's PC"),
                new Computer("COMP-AXRK40", "Bethany Cooper's PC"),
                new Computer("COMP-AXRK40", "Jeanne Hill's PC"),
                new Computer("COMP-AXRK40", "Tilda McGrath's PC"),
                new Computer("COMP-AXRK40", "Susan Wilson's PC"),
                new Computer("COMP-AXRK40", "Mary Minor's PC"),
                new Computer("COMP-AXRK40", "Claudia Rutledge's PC"),
                new Computer("COMP-AXRK40", "Julia Bridges's PC"),
                new Computer("COMP-AXRK40", "Joanna Romero's PC"),
                new Computer("COMP-AXRK40", "Gerald McKee's PC"),
                new Computer("COMP-AXRK40", "Deena Jones's PC"),
                new Computer("COMP-AXRK40", "Wilson Perry's PC"),
                new Computer("COMP-AXRK40", "Mario Neal's PC"),
            };
        }
    }
}
