using System;
using System.Collections.Generic;
using System.Text;

namespace GasInformServiceTestWork.Models
{
    public class DogAppiSettingsModel
    {
        public DogAppiSettingsModel()
        { 
        }

        public DogAppiSettingsModel(string link, string mediaType, string removedLinkPart)
        {
            DogApiLink = link;
            MediaTypeSettings = mediaType;
            RemovedLinkPart = removedLinkPart;
        }

        public string DogApiLink { get; set; }
        public string MediaTypeSettings { get; set; }
        public string RemovedLinkPart { get; set; }
    }
}
