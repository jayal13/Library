using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    public UserController()
    {
        
    }
    
    [HttpGet("test/{s}")]
    public string[] Test(string s)
    {
        return ["oneString","twoString", s];
    }
}   
