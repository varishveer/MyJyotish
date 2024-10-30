using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAccessLayer.ViewModels
{
    public class AppointmentSolution
    {
        [AllowNull]
        public ProblemSolutionViewModel[] ProblemSolution {get; set;}
    }
}
