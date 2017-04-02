using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using data;

namespace web.Models
{
    public class ClassStudentModel
    {
        [Display(Name="Student")]
        public string StudentName
        {
            get
            { return $"{Student.firstname} {Student.lastname}"; }
        }
        [Display(Name = "Class")]
        public string ClassName
        {
            get
            { return Course.name; }
        }
        public string Semester
        {
            get
            { return Class.semester; }
        }
        public decimal? Tuition
        {
            get { return Class.fee; }
        }

        public decimal? TextbookFee
        {
            get { return Course.textbookFee; }
        }

        public string Time
        {
            get { return $"{TimeSlot.start}-{TimeSlot.end}"; }
        }

        public string Status
        {
            get; set;
        }

        public bool Confirmed
        {
            get
            {
                return Status == RegistrationStatus.CancelConfirmed.ToString() || Status == RegistrationStatus.Confirmed.ToString();
            }
        }

        public DateTime ModifiedAt
        { get; set; }

        public string Warning { set; get; }

        public int id { get; set; }
        public member Student { get; set; }

        public bhclass Class { get; set; }

        public course Course { get; set; }

        public timeslot TimeSlot { get; set; }
    }
}