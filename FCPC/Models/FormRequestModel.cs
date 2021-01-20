using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCPC.Models
{
    public class FormRequestModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Candidato es requerido.")]
        public List<string> Candidate { get; set; }

        public bool VoteNull { get; set; }
    }
}
