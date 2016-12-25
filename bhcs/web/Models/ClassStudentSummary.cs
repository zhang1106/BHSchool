using System;
using System.Collections.Generic;
using System.Linq;
using data;

namespace web.Models { 
    public class ClassStudentSummary
    {
  
        public  member Household { get; set; }

        public address Address { get; set; }

        public IList<ClassStudentModel> Classes { get; set; }

        public IList<Config> Fee { get; set; }

        public decimal? TotalTuition
        {
            get
            {
                return Classes.Where(c=>c.Confirmed != true).
                    Sum(c => c.Status==RegistrationStatus.CancelActive.ToString()?(-1)*c.Tuition : c.Tuition);
            }
        }

        public decimal? TotalTextbookFee
        {
            get
            {
               return Classes.Where(c => c.Confirmed != true && 
               c.Status != RegistrationStatus.CancelActive.ToString() ).Sum(c => c.TextbookFee);
            }
        }

        public decimal? TotalFee
        {
            get
            {
                return Fee.Sum(c => decimal.Parse(c.value));
            }
        }

        public const decimal AccountFee = 50.0m;
        public const decimal RegisterFee = 20.0m;

        public decimal? Total
        {
            get
            {
                return TotalTuition + TotalTextbookFee +  TotalFee;
            }
        }
    }
}