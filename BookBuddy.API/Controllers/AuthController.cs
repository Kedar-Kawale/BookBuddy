using BookBuddy.API.Models.Domain;
using BookBuddy.API.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//below usings are required for JWT token generation
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.roleManager = roleManager;

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
                UserName = registerRequestDTO.Email,
                Email = registerRequestDTO.Email,
                FirstName = registerRequestDTO.FirstName,
                LastName = registerRequestDTO.LastName,
                PhoneNumber = registerRequestDTO.PhoneNumber,
                RegisteredAt = DateTime.UtcNow
            };

            // create the user in the database using the user manager and pass the password as well. the user manager will hash the password and store it securely in the database.
            var identityResult = await userManager.CreateAsync(user, registerRequestDTO.Password);

            //What if CreateAsync() fails? means - email already exists, passwoword is weak,invalid email format , etc.
            //we can check the identityResult.Succeeded property to see if the user was created successfully or not. if it is false,
            //we can return the errors to the client.
            if (identityResult.Succeeded)
            {

                //Now we're going to assign the role immediately after the user is created.
                await userManager.AddToRoleAsync(user, "Customer");  //here after User is Created, we are immediately assign "Customer" role to User

                return Ok("User Registered Successfully");
            }

            // If registration fails, Identity gives detailed errors to the client , such as 
            //Passwords must have a non alphanumeric character,Passwords must have an uppercase letter,Email already exists, etc.
            return BadRequest(identityResult.Errors);

        }


        //====================================================================================================================
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            /* Flow of the Login process:
             
             Email received
                 ↓
            FindByEmailAsync()
                 ↓
               User Exists?
                 ↓
           CheckPasswordAsync()
                 ↓
            Password Valid?
                 ↓
            Claims Created
                 ↓
           Signing Key Ready
                    ↓
            JWT Token obj Created
                    ↓
            JWT Token string created
                    ↓
            Token sent to client(Return LoginResponseDTO)                                         
             */


            //First, we need to find the user by email using the user manager. if the user is not found, we can return an error message to the client.
            var user = await userManager.FindByEmailAsync(loginRequestDTO.Email);


            //If User not found , return error message to client.
            if (user == null)
            {
                return BadRequest("Email Does not exists");
            }

            //Now we need to verify the password. we can use the user manager's CheckPasswordAsync() method to check if the provided password is correct for the user.
            //if the password is incorrect, we can return an error message to the client.
            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            // if the password is incorrect, return error message to client
            if (!isPasswordValid)
            {
                return BadRequest("Invalid Password");
            }

            //{This step is after creating the Claims,}For the logged-in user, Identity will fetch: Customer or Admin AspNetUserRoles
            var roles = await userManager.GetRolesAsync(user);

            //if the password is correct, we can generate a JWT token for the user and return it to the client.
            //now first create a claims identity for the user. claims are pieces of information about the user that we want to include in the token. for example, we can include the user's email, id, and roles as claims in the token.
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email!),
                };

            //{This step is after creating the Claims}Now we need to add the user's roles as claims in the token as well. this is important because it allows us to implement role-based authorization in our application. when the client sends the token back to the server in subsequent requests, the server can check the claims in the token to determine what actions the user is authorized to perform based on their roles.
            foreach (var role in roles)
            {
                claims.Add(
                    new Claim(ClaimTypes.Role, role)
                );
            }

            //Now we're ready to create the actual JWT token.

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));


            //digitally sing the token using the signing key and the HMAC SHA256 algorithm. this ensures that the token cannot be tampered with and can be verified by the server when the client sends it back in subsequent requests.

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //creating JWT toeken here.
            //We're creating the JWT object with: issuer, audience, claims, expiration time, and signing credentials. the issuer and audience are typically defined in the appsettings.json file and can be accessed through the configuration object.
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            // converts JWT token object into a string format that can be sent to the client. this string is what the client will include in the Authorization header of subsequent requests to authenticate itself to the server.
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Finally, we return the JWT token string to the client in the response. the client can then use this token to authenticate itself in future requests by including it in the Authorization header.
            //that is, Now we need to return the token to the client.

            return Ok(new LoginResponseDTO
            {

                JwtToken = jwtToken,
            });
        }

        //====================================================================================================================
        //Development: / Initial Setup Endpoint to create roles in the database. This endpoint is not meant to be used in production, but only for initial setup or development purposes. In a real-world application, you would typically create roles through a separate admin interface or during application startup.
        [HttpPost]
        [Route("CreateRoles")]
        public async Task<ActionResult> CreateRoles()
        {
            //Define the roles you want to create: Admin and Customer
            var roles = new List<string>
            {
                "Admin",
                "Customer"
            };

            //Loop through the list of roles and create each role using the RoleManager. the RoleManager will check if the role already exists before creating it to avoid duplicates.
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role)) //Does ROles already exist? 
                {
                    //If the role does not exist, create it using the RoleManager's CreateAsync() method.
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            //return a meaningful message to the client indicating that the roles have been created successfully.
            return Ok("Roles created successfully");
        }



    }
}
