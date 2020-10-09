﻿using System;

namespace ProfileBook.Models
{
    public class ProfileModel
    {
        public int ProfileId { get; set; }

        public string ProfileImage { get; set; }
        public string NickNameLabel { get; set; }
        public string NameLabel { get; set; }
        public DateTime DateLabel { get; set; }
    }
}