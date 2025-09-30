using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Domain.Entities
{

    public class User
    {
        public Guid Id { get; set; }  // GUID for unique ID
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

        public int RoleId { get; set; }
        public Role? Role { get; set; } = default!;  // Navigation property

        public string Address { get; set; } = default!;

        public ICollection<Survey>? Surveys { get; set; } = new List<Survey>();
        public ICollection<UserSurvey>? UserSurveys { get; set; } = new List<UserSurvey>();
    }

}


