﻿using Microsoft.AspNetCore.Identity;

namespace MyShop_Backend.Models
{
	public class User :IdentityUser<int>,  IBaseEntity
	{
		public string FullName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}