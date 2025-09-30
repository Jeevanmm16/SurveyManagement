using SurveyManagement.Application.DTOS;
using SurveyManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyManagement.Application.DTOS
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = default!;
    }
}




