
using apiUniversidade.Model;
using Microsoft.AspNetCore.Mvc;
using apiUniversidade.Context;
using Microsoft.AspNetCore.Authorization;

namespace apiUniversidade.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("[controller]")]
    public class CursoController : Controller
    {
       private readonly ILogger<CursoController> _logger;
       private readonly apiUniversidadeContext _context;

       public CursoController(ILogger<CursoController> logger , apiUniversidadeContext context)
       {
        _logger = logger;
        _context = context;
       }
       
        [HttpGet]

        public ActionResult<IEnumerable<Curso>> Get()
        {
            var curso = _context.Cursos?.ToList();
            if(curso is null)
                return NotFound();
            
            return curso;
        }

        [HttpGet("{id:int}", Name="GetCurso")]
        public ActionResult<Curso> Get(int id)
        {
            var curso = _context.Cursos?.FirstOrDefault(p => p.Id == id);
            if(curso is null)
                return NotFound("Tem não, parceiro");
            return curso;
        }

        [HttpPost]
        public ActionResult Post(Curso curso){
            _context.Cursos?.Add(curso);
            _context.SaveChanges();
            
            return new CreatedAtRouteResult("GetCurso", 
            new{ id = curso.Id},
            curso);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Curso curso) 
        {
            if(id != curso.Id)
            return BadRequest();

            _context.Entry(curso).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return Ok(curso);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var curso = _context.Cursos?.FirstOrDefault(p => p.Id == id);

            if(curso is null)
                return NotFound();

            _context.Cursos?.Remove(curso);
            _context.SaveChanges();

            return Ok(curso);           
        }

    }
}