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

        public IList<Credit> Checks { get; set; }

        public IList<Credit> Discounts { get; set; }

        public decimal? TotalTuition
        {
            get
            {
                return Classes.Where(c=>IsActive(c)).Sum(c => c.Tuition);
            }
        }

        public decimal? TotalTextbookFee
        {
            get
            {
               return Classes.Where(c => IsActive(c)).Sum(c => c.TextbookFee);
            }
        }

        public decimal? TotalPayments
        {
            get
            {
                return Checks.Sum(c => c.amount);
            }
        }

        public decimal? TotalDiscounts
        {
            get
            {
                return Discounts.Sum(c =>c.amount);
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
                return TotalTuition + TotalTextbookFee +  TotalFee + TotalPayments + TotalDiscounts;
            }
        }

        private bool IsActive(ClassStudentModel registration )
        {
            var active = registration.Status != RegistrationStatus.CancelConfirmed.ToString() &&
               registration.Status != RegistrationStatus.CancelActive.ToString();

            return active;
        }
    }
}