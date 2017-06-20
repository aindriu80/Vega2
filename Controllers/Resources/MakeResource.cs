using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vega.Controllers.Resources
{
    public class MakeResource : KeyValuePairResource
    {
        public ICollection<KeyValuePairResource> Models { get; set; }

        public MakeResource()
        {
            Models = new Collection<KeyValuePairResource>();
        }
    }
}
