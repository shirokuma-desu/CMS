using CMS_API.ControllerModels;
using CMS_API.Helper;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private CMSContext _context;

        public MaterialsController(CMSContext context)
        {
            _context = context;
        }

        [HttpGet("Download")]
        public async Task<IActionResult> DownloadMaterial(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
            return File(fileStream, "application/octet-stream", Path.GetFileName(filePath));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var context = await _context.LearningMaterials.ToListAsync();
            return Ok(context);
        }
        [HttpGet("{course_id}")]
        public async Task<IActionResult> getListMaterialByCourse(int course_id)
        {
            var context = await _context.LearningMaterials.Include(c => c.Course).Where(c => c.CourseId == course_id).ToListAsync();
            return Ok(context);
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Post([FromForm] IFormCollection form)
        {
            try
            {
                var model = new MaterialModel
                {
                    Title = form["materialName"],
                    Description = form["materialDesc"],
                    CourseId = form["courseId"],
                    PostedFile = form.Files["materialUpload"]
                };

                var tokenEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if(tokenEmail == null)
                {
                    return BadRequest($"Not found claim email from token");
                }

                var teacherByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == tokenEmail);
                if(teacherByEmail == null)
                {
                    return BadRequest($"Teacher with email {tokenEmail} not existed");
                }

                if(int.TryParse(model.CourseId, out var courseId))
                {
                    var courseById = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
                    if(courseById == null)
                    {
                        return BadRequest($"Course with ID {courseById} not existed");
                    }

                    var uploadPathRs = await DirectoryHelper.ProcessUploadFile(model.PostedFile, "Material", "Material_");
                    if(uploadPathRs.StartsWith("Error: "))
                        return BadRequest(uploadPathRs);

                    // Create assignment
                    await _context.LearningMaterials.AddAsync(new LearningMaterial
                    {
                        CourseId = courseById.CourseId,
                        Title = model.Title,
                        Information = model.Description,
                        Url = uploadPathRs
                    });

                    await _context.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest("Something bad happened");
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("id")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Put(int id, [FromBody] MaterialModel model)
        {
            try
            {
                var tmp = await _context.LearningMaterials.FindAsync(id);
                if(tmp == null)
                {
                    return NotFound();
                }

                tmp.Title = model.Title;
                tmp.Information = model.Description;
                //tmp.Url = model.Url;

                _context.Entry(tmp).CurrentValues.SetValues(tmp);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("id")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.LearningMaterials.SingleOrDefaultAsync(c => c.LmId == id);

                if(context != null)
                {
                    var c = _context.LearningMaterials.Remove(context);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch
            (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
