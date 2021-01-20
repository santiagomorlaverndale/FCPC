using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCPC.Models
{
    public class FormModel
    {
        public string Name { get; set; }

        public int Limit { get; set; }

        public string Id { get; set; }

        public List<Candidate> Candidates { get; set; }

        [Required(ErrorMessage = "Candidato es requerido.")]
        public string Candidate { get; set; }
    }
}
