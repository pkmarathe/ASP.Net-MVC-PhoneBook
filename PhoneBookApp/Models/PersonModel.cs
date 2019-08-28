using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhoneBookApp.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PersonModel
    {
        public long ID { get; set; }
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, Phone, Display(Name = "Phone")]
        public string Phone { get; set; }
        [EmailAddress, Display(Name = "Email")]
        public string Email { get; set; }
  

        public PersonModel(long id, string firstName, string lastName, string phone, string email)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
       
        }

        public PersonModel()
        {

        }
    }
}