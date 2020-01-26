﻿using Smart_Trix.Models.StData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Smart_Trix.Models.StViewModel.Account
{
    public class UserProfileVM
    {
        public UserProfileVM()
        {

        }
        public UserProfileVM(StUserDT yadav)
        {
            Id = yadav.Id;
            FirstName = yadav.FirstName;
            LastName = yadav.LastName;
            EmailAddress = yadav.EmailAddress;
            UserName = yadav.UserName;
            Password = yadav.Password;

        }
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}