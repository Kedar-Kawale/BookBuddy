using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookBuddy.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using BookBuddy.API.Models.DTO;

namespace BookBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       private  readonly  UserManager<ApplicationUser> userManager;
        public AuthController(UserManager<ApplicationUser> userManager)
        {
            this.userManager=  userManager;
        }
        //-------------------Constructor + DI done above //-------------------

        //-------------------registration end point below//-------------------

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            //the user object is  being created in memory.
            var user = new ApplicationUser
            {
               UserName =  registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                PhoneNumber = registerRequestDTO.PhoneNumber,
                RegisteredAt = DateTime.UtcNow
            };

            // create the user in the database using the user manager and pass the password as well. the user manager will hash the password and store it securely in the database.
            var identityResult =  await userManager.CreateAsync( user, registerRequestDTO.Password);

            //What if CreateAsync() fails? means - email already exists, passwoword is weak,invalid email format , etc.
            //we can check the identityResult.Succeeded property to see if the user was created successfully or not. if it is false,
            //we can return the errors to the client.
            if (identityResult.Succeeded)
            {
                return Ok("User Registered Successfully");
            }

            // If registration fails, Identity gives detailed errors to the client , such as 
            //Passwords must have a non alphanumeric character,Passwords must have an uppercase letter,Email already exists, etc.
            return BadRequest(identityResult.Errors);

        }
    }
}
