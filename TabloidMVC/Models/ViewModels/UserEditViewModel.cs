using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class UserEditViewModel
    {
        public Post Post { get; set; }
        public List<Category> Category { get; set; }

        public List<UserProfile> UserProfiles { get; set; }

    }
}
