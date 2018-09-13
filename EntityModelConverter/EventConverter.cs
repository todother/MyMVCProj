using Cats.DataEntiry;
using CatsPrj.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModelConverter
{
    public class EventConverter
    {
        public static EventModel entityToModel(tbl_event tevent)
        {
            EventModel model = new EventModel();
            model.eventId = tevent.eventId;
            model.eventIndex = tevent.eventIndex;
            model.eventTime = tevent.eventTime;
            model.imagePath = tevent.imagePath;
            return model;
        }
    }
}
