using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FoodStore.Data.Entities
{
     [Owned]
    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired  => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}