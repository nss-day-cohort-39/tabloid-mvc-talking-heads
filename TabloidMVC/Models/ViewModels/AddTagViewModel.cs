using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class AddTagViewModel
    {
        public Post post { get; set; }

        public List<Tag> Tags { get; set; }
    }
}
