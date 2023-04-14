using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using webapi.SharedServices.Authentication;
using webapi.SharedServices.Db;

namespace webapi.StudentFeatures;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ILogger<StudentsController> _logger;
    private readonly AppDbContext _context;
    private readonly IAuthenticationService _authenticationService;

    public StudentsController(ILogger<StudentsController> logger, AppDbContext context, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _context = context;
        _authenticationService = authenticationService;
    }

    [HttpGet("students")]
    [AllowAnonymous]
    public async Task<IEnumerable<StudentDTO>> GetStudents()
    {
        var entities = await _context.Students.ToListAsync();
        return entities.ToDTO();
    }

    [HttpGet("token")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> GetTokenAsync(string nickname)
    {
        try
        {
            var found = await _context.Students.SingleAsync(x=> x.Nickname == nickname);

            var strToken = _authenticationService.GetToken(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, found.Nickname),
                new Claim(JwtRegisteredClaimNames.FamilyName, found.Lastname),
                new Claim(JwtRegisteredClaimNames.GivenName, found.Firstname),
            });

            return Ok(new
            {
                token = strToken
            });
        }
        catch (InvalidOperationException) // not found element exception
        {
            return NotFound(new { error = "Cannot find user" });
        }
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody]StudentDTO request)
    {
        bool alreadyExistsWithGivenId = await _context.Students
            .SingleOrDefaultAsync(x => x.StudentId == request.StudentId) != null;

        if (alreadyExistsWithGivenId)
        {
            return Conflict(new
            {
                error = "User exists with given id"
            });
        }

        bool alreadyExistsWithGivenNickname= await _context.Students
            .SingleOrDefaultAsync(x => x.Nickname == request.Nickname) != null;

        if (alreadyExistsWithGivenNickname)
        {
            return Conflict(new
            {
                error = "User exists with given nickname"
            });
        }

        var entity = request.FromDTO();
        _context.Students.Add(entity);

        await _context.SaveChangesAsync();

        return entity.ToDTO();
    }
}
