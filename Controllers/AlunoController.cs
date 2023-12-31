using apiUniversidade.Context;
using apiUniversidade.Model;
using Microsoft.AspNetCore.Mvc;

namespace apiUniversidade.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlunoController : Controller
    {
        private readonly ILogger<AlunoController> _logger;
        private readonly apiUniversidadeContext _context;

        public AlunoController(ILogger<AlunoController> logger , apiUniversidadeContext context)
       {
        _logger = logger;
        _context = context;
       }

        [HttpGet]
        public ActionResult<IEnumerable<Aluno>> Get()
        {
            var alunos = _context.Alunos?.ToList();
                if (alunos is null)
                    return NotFound();

            return alunos;
        }

        [HttpGet("{id:int}", Name="GetAluno")]
        public ActionResult<Aluno> Get(int id)
        {
            var aluno = _context.Alunos?.FirstOrDefault(p => p.Id == id);
            if(aluno is null)
                return NotFound("Não encontrado, parcero");
            return aluno;
        }

        [HttpPost]
        public ActionResult Post(Aluno aluno)
        {
            _context.Alunos?.Add(aluno);
            _context.SaveChanges();

            return new CreatedAtRouteResult("GetAluno",
            new{ id = aluno.Id}, aluno);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Aluno aluno) 
        {
            if(id != aluno.Id)
                return BadRequest();

            _context.Entry(aluno).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            
            return Ok(aluno);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var aluno = _context.Alunos?.FirstOrDefault(p => p.Id == id);

            if(aluno is null)
                return NotFound();

            _context.Alunos?.Remove(aluno);
            _context.SaveChanges();

            return Ok(aluno);           
        }
    }
}