﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WildlifeLogAPI.Models.DTO;
using WildlifeLogAPI.Repositories;

namespace WildlifeLogAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]


	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly ITokenRepository tokenRepository;
		

		public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
		{
			this.userManager = userManager;
			this.tokenRepository = tokenRepository;
			
		}


		//POST: api/Auth/Register
		[HttpPost]
		[Route("Register")]

		public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
		{
			//create a new instance of IdetityUser (which includes the username and email they sumbmit)
			var identityUser = new IdentityUser
			{
				UserName = registerRequestDto.Username,
				Email = registerRequestDto.Email
			};

			//then use the userManager's built in CreateAsync (pass in the identityUser we just created, and the password passed into the dto)
			var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

			//Add roles to the user 

			//check to see if the user was successfully created 
			if (identityResult.Succeeded)
			{
				//Add the User Role by default when they register
				var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");

				//check if the userrole was successfully added, if so display message
				if (roleIdentityResult.Succeeded)
				{
					return Ok("User was registered. Please login.");
				}

			}
			//if it didnt succeed 
			return BadRequest("something went wrong. Please try again. ");


		}

		//POST: api/auth/login 
		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
		{

			//get the user by their email and store the user in the user variable
			var user = await userManager.FindByEmailAsync(loginRequestDto.Email);


			//check if the email is associated with a user
			//if it is filled in and we find it in the database then do the code required to log in 
			if (user != null)
			{
				//check that password matches the email using built in CheckPasswordAsync
				//pass in the email and password
				var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

				//if the password matches the email, then create a token 
				if (checkPasswordResult)
				{
					//Create token here 
					//get username 
					var username = user.UserName;

					
					//Get roles for this user 
					var roles = await userManager.GetRolesAsync(user);

					//if there are roles then create the token 
					if (roles != null)
					{
						//create jwt token
						var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList()); //this is our token 

						//put that token into a dto 
						var response = new LoginResponseDto
						{
							jwtToken = jwtToken,
							userName = username,
							roles = roles.ToList(),
						};

						return Ok(response);
					}

				}
			}


			return BadRequest("Username or password is incorect");
		}
	}
}
