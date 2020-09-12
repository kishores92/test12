using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenPOS.Models
{
    public class UsersView
    {
        public IEnumerable<UserViewModel> Users { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }

    }
}
