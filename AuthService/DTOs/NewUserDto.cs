using System;

namespace AuthService.DTOs;

public class NewUserDto {
	public string Username { get; set; }
	public string Email { get; set; }

	public string Password { get; set; }

}
