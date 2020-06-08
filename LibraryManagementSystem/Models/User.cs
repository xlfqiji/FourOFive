﻿using FreeSql.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    [Table()]
    [Index("uk_NationalIdentificationNumber", "NationalIdentificationNumber", true)]
    [Index("uk_UserName", "UserName", true)]
    public class User : DatabaseModel
    {
        [Column(IsNullable = false)]
        public string UserName { get; set; }
        [Column(IsNullable = false)]
        public string Salt { get; set; }
        [Column(IsNullable = false)]
        public string Password { get; set; }
        [Column(IsNullable = false)]
        public string Name { get; set; }
        [Column(IsNullable = false)]
        public string NationalIdentificationNumber { get; set; }
        [Column()]
        public int CreditValue { get; set; }

    }
}