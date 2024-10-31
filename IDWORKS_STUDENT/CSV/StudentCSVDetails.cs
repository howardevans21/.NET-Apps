using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace IDWORKS_STUDENT.CSV
{
    public class StudentCSVDetails
    {
        public string Coverage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string School { get; set; }
        public string PolicyNumber { get; set; }
        public String EffectiveDate { get; set; }
        public string CoverageProvider { get; set; }
    }

    public class CsvUserDetailsMapping : CsvMapping<StudentCSVDetails>
    {
        public CsvUserDetailsMapping()
            : base()
        {
            MapProperty(0, x => x.Coverage);
            MapProperty(1, x => x.FirstName);
            MapProperty(2, x => x.LastName);
            MapProperty(3, x => x.School);
            MapProperty(4, x => x.PolicyNumber);
            MapProperty(5, x => x.EffectiveDate);
            MapProperty(6, x => x.CoverageProvider);
        }
    }

}
