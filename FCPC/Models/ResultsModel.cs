using System.Collections.Generic;

namespace FCPC.Models
{
    public class GeneralModel
    {
        public List<ResultsModel> Results { get; set; }
    }


    public class ResultsModel
    {
        public Region Region { get; set; }
        
        public List<ResultItem> Results { get; set; }

        public List<VoteItem> Votes { get; set; }

        public string Labels { get; set; }
    }

    public class ResultItem
    {

        public Vote Vote { get; set; }
        public User User { get; set; }
        public Candidate Candidate { get; set; }
    }

    public class VoteItem
    {
        public Candidate Candidate { get; set; }
        public string Label { get; set; }
        public int Vote { get; set; }
    }
}
