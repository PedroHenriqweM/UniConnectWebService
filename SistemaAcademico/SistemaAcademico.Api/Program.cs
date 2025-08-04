using Microsoft.EntityFrameworkCore;
using SistemaAcademico.Application.Interfaces;
using SistemaAcademico.Application.Services;
using SistemaAcademico.Domain.Entities;
using SistemaAcademico.Infrastructure.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar o banco de dados em memória
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("SistemaAcademicoDb"));

builder.Services.AddScoped<IAlunoService, AlunoService>();
builder.Services.AddScoped<ICursoService, CursoService>();
builder.Services.AddScoped<IDisciplinaService, DisciplinaService>();
builder.Services.AddScoped<IMatriculaService, MatriculaService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // URL do seu front-end
                 .AllowAnyHeader()
                 .AllowAnyMethod();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro durante o seeding do banco de dados.");
    }
}

app.MapPost("/api/reset", (AppDbContext context) =>
{
    // Limpa todas as tabelas
    context.Matriculas.RemoveRange(context.Matriculas);
    context.Disciplinas.RemoveRange(context.Disciplinas);
    context.Alunos.RemoveRange(context.Alunos);
    context.Cursos.RemoveRange(context.Cursos);
    context.SaveChanges();

    // Repopula o banco de dados
    SeedData(context);

    return Results.Ok("Banco de dados resetado com sucesso.");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers(); 

app.Run();


static void SeedData(AppDbContext context)
{
    if (!context.Cursos.Any())
    {
        var cursos = new List<Curso>
        {
            new Curso { Id = 1, Nome = "Ciência da Computação", Descricao = "Curso de bacharelado em computação.", DuracaoSemestres = 8, Modalidade = Modalidade.Presencial },
            new Curso { Id = 2, Nome = "Matemática Aplicada", Descricao = "Curso de bacharelado em matemática.", DuracaoSemestres = 7, Modalidade = Modalidade.Presencial }
        };
        context.Cursos.AddRange(cursos);
        context.SaveChanges();
    }

    if (!context.Disciplinas.Any())
    {
        var disciplinas = new List<Disciplina>
        {
            new Disciplina { Id = 1, Nome = "Introdução à Programação", Codigo = "INF101", CargaHoraria = 60, CursoId = 1 },
            new Disciplina { Id = 2, Nome = "Banco de Dados", Codigo = "BD202", CargaHoraria = 80, CursoId = 1 },
            new Disciplina { Id = 3, Nome = "Cálculo I", Codigo = "MAT101", CargaHoraria = 90, CursoId = 2 }
        };
        context.Disciplinas.AddRange(disciplinas);
        context.SaveChanges();
    }

    if (!context.Alunos.Any())
    {
        var alunos = new List<Aluno>
        {
            new Aluno { Id = 1, Nome = "João da Silva", Email = "joao@email.com", DataNascimento = DateTime.Parse("2000-01-15"), Matricula = "202012345", CursoId = 1 },
            new Aluno { Id = 2, Nome = "Maria Souza", Email = "maria@email.com", DataNascimento = DateTime.Parse("2001-03-20"), Matricula = "202167890", CursoId = 2 }
        };
        context.Alunos.AddRange(alunos);
        context.SaveChanges();
    }

    if (!context.Matriculas.Any())
    {
        var matriculas = new List<Matricula>
        {
            new Matricula { Id = 1, AlunoId = 1, DisciplinaId = 1, Periodo = "2025.1", NotaFinal = 8.5 },
            new Matricula { Id = 2, AlunoId = 2, DisciplinaId = 3, Periodo = "2025.1", NotaFinal = 7.0 }
        };
        context.Matriculas.AddRange(matriculas);
        context.SaveChanges();
    }
}