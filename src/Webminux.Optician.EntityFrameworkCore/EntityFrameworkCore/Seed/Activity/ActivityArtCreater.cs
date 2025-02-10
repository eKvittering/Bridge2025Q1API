using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webminux.Optician.EntityFrameworkCore
{
    public class ActivityArtCreator
    {
        private readonly OpticianDbContext _context;

        public ActivityArtCreator(OpticianDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateActivityArts();
        }

        private void CreateActivityArts()
        {
            if (!_context.ActivityArts.Any())
            {
                var listOfActivityArts = new List<ActivityArt>();
                listOfActivityArts.Add(ActivityArt.Create(1,OpticianConsts.ActivityArts.LetterSend));
                listOfActivityArts.Add(ActivityArt.Create(1,OpticianConsts.ActivityArts.Activity));
                listOfActivityArts.Add(ActivityArt.Create(1,OpticianConsts.ActivityArts.CustomerResponse));
                listOfActivityArts.Add(ActivityArt.Create(1,OpticianConsts.ActivityArts.MachineActivity));

                _context.ActivityArts.AddRange(listOfActivityArts);
                _context.SaveChanges();
            }
        }
    }
}
