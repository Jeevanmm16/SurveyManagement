using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Domain.Entities
{

    public class Survey
    {
        public Guid SurveyId { get; set; }
        public string Title { get; set; } = default!;

        public Guid UserId { get; set; }   // Creator
        public User User { get; set; } = default!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<UserSurvey> UserSurveys { get; set; } = new List<UserSurvey>();
    }
}

