using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyDotnetProject.Controllers.Resources
{
    public class MakeResource : KeyValueResource
    {  
        public ICollection<KeyValueResource> Models { get; set; }

        public MakeResource() 
        {
            Models = new Collection<KeyValueResource>();
        }
    }
    
}