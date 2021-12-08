﻿using CRUD_EF.DbConnection;
using CRUD_EF.Model;
using CRUD_EF.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CRUD_EF.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // private IUserRepository<User> _userRepository;        

        UserRepository userRepository = new UserRepository();

        [Route("users")]
        [HttpGet]
        public ActionResult GetAllUser()
        {
            try
            {
                var userList = userRepository.GetAllUser();
                if (userRepository.IsUserListEmty(userList))
                {
                    return Ok("User List Is Emty ");
                }
                return Ok(userList);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("user/{id}")]
        [HttpGet]
        [Authorize]
        public ActionResult GetUserById([FromRoute] string id)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(); }

                Guid userId = userRepository.CheckFormatGuid(id);

                if (userId == Guid.Empty) { return BadRequest("Error Format"); }

                //note
                if (userRepository.IsExistUser(userId))
                {
                    var user = userRepository.GetUserById(userId);
                    return Ok(user);
                }
                else
                {
                    return BadRequest("User dose not exist");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("user")]
        [HttpGet]
        public ActionResult SearchByCondition([FromQuery] UserSearch user)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(); }

                var result = userRepository.SearchByCondition(user);
                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("user")]
        [HttpPost]
        public ActionResult AddUser([FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest("Binding false"); }

                bool isValidName = userRepository.IsValidName(user.FullName);

                if (isValidName)
                {
                    var result = userRepository.AddUser(user);
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("user/{id}")]
        [HttpPut]
        public ActionResult EditUser([FromRoute] string id, [FromBody] User user)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(); }

                Guid userId = userRepository.CheckFormatGuid(id);
                if (userId == Guid.Empty) { return BadRequest("Error Format"); }

                bool isValidName = userRepository.IsValidNameEdit(userId, user.FullName);
                if (!isValidName) { return BadRequest("Name is exist"); }

                if (userRepository.IsExistUser(userId))
                {
                    var result = userRepository.EditUser(userId, user);
                    return Ok(result);
                }
                return BadRequest("User dose not exist");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("user/{id}")]
        [HttpDelete]
        public ActionResult DeleteUser(string id)
        {
            try
            {
                Guid userId = userRepository.CheckFormatGuid(id);

                if (!ModelState.IsValid) { return BadRequest(); }

                if (userId == Guid.Empty) { return BadRequest("Error Format"); }

                if (userRepository.IsExistUser(userId))
                {
                    var result = userRepository.DeleteUser(userId);
                    return Ok(result);
                }
                return BadRequest("User dose not exist");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("test")]
        [HttpDelete]
        public string TestDB()
        {
            using var dbcontext = new UserContext();
            string dbname = dbcontext.Database.GetDbConnection().Database;
            var kq = dbcontext.Database.EnsureCreated();

            if (kq)
            {
                return dbname;
            }
            else
            {
                return "fail";
            }
        }
    }
}