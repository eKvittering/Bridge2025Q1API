using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.EntityFrameworkCore
{
    public class ActivityTypeCreator
    {
        private readonly OpticianDbContext _context;

        public ActivityTypeCreator(OpticianDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateActivityArts();
        }

        private void CreateActivityArts()
        {
            if (!_context.ActivityTypes.Any())
            {
                var listOfActivityTypes = new List<ActivityType>();
                listOfActivityTypes.Add(ActivityType.Create(null, "Eye test  (A1)", 4, 350));
                listOfActivityTypes.Add(ActivityType.Create(null, "Sale (A3)", 9, 14));
                listOfActivityTypes.Add(ActivityType.Create(null, "Letter test - 1 year (A4)", 1, 14));
                listOfActivityTypes.Add(ActivityType.Create(null, "Letter sale - thanks for buying (A9)", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Booking, eyes (A16)", 1, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Remember Write/call (R18)", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Note (A25)", 18, 7));
                listOfActivityTypes.Add(ActivityType.Create(null, "SMS from person form (A50)", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Email from person form (A60)", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Phone Call Note", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "SMS Note", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Email Note", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Check In", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Check Out", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Customer Booking", 0, 0));
                listOfActivityTypes.Add(ActivityType.Create(null, "Fault Phone Call", 0, 0));

                _context.ActivityTypes.AddRange((IEnumerable<ActivityType>)listOfActivityTypes);
                _context.SaveChanges();
            }
        }

    }
}
