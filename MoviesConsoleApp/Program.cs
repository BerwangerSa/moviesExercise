using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MoviesConsoleApp
{
    public static class Program
    {
        static void Main(String[] args)
        {
            // acesso ao EF serah realizado pela variavel _db
            // essa dependencia da camada de apresentacao com
            // a camada de persistencia eh errada!
            MovieContext _db = new MovieContext();

            var dataBase = ExtractDataFromMovies(_db);

            Console.WriteLine();
            Console.WriteLine("1. Listar o nome de todos personagens desempenhados por um determinado ator, incluindo a informação de qual o título do filme e o diretor");
            foreach (var actor in _db.Actors)
            {
                Console.WriteLine($"Ator: {actor.Name}");
                foreach (var e in dataBase)
                {
                    if (e.ActorMovie.ActorId == actor.ActorId)
                    {
                        Console.WriteLine($"Personagem: {e.ActorMovie.Character} | Filme: {e.Movie.Title} | Diretor: {e.Movie.Director}");
                    }
                }
            }
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("2. Mostrar o nome e idade de todos atores que desempenharam um determinado personagem(por exemplo, quais os atores que já atuaram como '007' ?");

            var actorMovies = dataBase.Select(x => x.ActorMovie).ToList();
            var characters = actorMovies.Select(x => x.Character).ToList().Distinct();

            foreach (var c in characters)
            {
                Console.WriteLine($"Personagem: {c}");
                List<int> ids = new List<int>();
                foreach (var d in dataBase)
                {
                    if (c == d.ActorMovie.Character && !ids.Contains(d.Actor.ActorId))
                    {
                        Console.WriteLine($"Ator: {d.Actor.Name} | Idade: {DateTime.Now.Year - d.Actor.DateBirth.Year}");
                        ids.Add(d.Actor.ActorId);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("3. Informar qual o ator desempenhou mais vezes um determinado personagem(por exemplo: qual o ator que realizou mais filmes como o 'agente 007'");
            int idFinal = FindActor(actorMovies);
            var name = _db.Actors.Where(x => x.ActorId == idFinal).ToList().FirstOrDefault().Name;
            Console.WriteLine($"Ator que desempenhou mais vezes um determinado personagem foi {idFinal} - {name}");

            Console.WriteLine();
            Console.WriteLine("4. Mostrar o nome e a data de nascimento do ator mais idoso");
            var actorOld = _db.Actors.OrderBy(x => x.DateBirth).ToList().FirstOrDefault();
            Console.WriteLine($"Ator: { actorOld.Name} | Idade: {DateTime.Now.Year - actorOld.DateBirth.Year}");

            Console.WriteLine();
            Console.WriteLine("5. Mostrar o nome e a data de nascimento do ator mais novo a atuar em um determinado gênero");
            var youngActors = _db.Actors.OrderByDescending(x => x.DateBirth).ToList().FirstOrDefault();
            foreach (var x in dataBase)
            {
                if (x.Actor.ActorId == youngActors.ActorId)
                {
                    var movie = _db.Movies.Where(y => y.MovieId == x.Movie.MovieId).ToList().FirstOrDefault();
                    Console.WriteLine($"Genero: {movie.Genre.Name} | Ator: {x.Actor.Name} | Idade: {DateTime.Now.Year - x.Actor.DateBirth.Year}");
                    break;
                }
            }


            Console.WriteLine();
            Console.WriteLine("6. Mostrar o valor médio das avaliações dos filmes de um determinado diretor");
            var rating = _db.Movies.Where(x => x.Director == "Steven Spielberg").ToList().Average(y => y.Rating);
            Console.WriteLine($"Diretor: Steven Spielberg | Media de avaliação {rating}");

            Console.WriteLine();
            Console.WriteLine("7. Qual o elenco do filme PIOR avaliado ?");
            var worstMovie = _db.Movies.OrderBy(x => x.Rating).ToList().FirstOrDefault();
            Console.WriteLine($"Filme: {worstMovie.Title} | Nota: {worstMovie.Rating}");
            var charactersWorstMovie = _db.Characters.Where(x => x.MovieId == worstMovie.MovieId).ToList();
            var actorsWorstMovie = new List<Actor>();
            foreach(var c in charactersWorstMovie)
            {
                actorsWorstMovie.Add(_db.Actors.Where(x => x.ActorId == c.ActorId).ToList().FirstOrDefault());
            }
            foreach(var a in actorsWorstMovie)
            {
                Console.WriteLine($"Ator: {a.Name}");
            }
            Console.WriteLine($"O filme com elenco MELHOR avaliado não possui elenco na base de dados! (filme {_db.Movies.OrderByDescending(x => x.Rating).ToList().FirstOrDefault().Title})");

            Console.WriteLine();
            Console.WriteLine("8. Qual o elenco do filme com o MENOR faturamento?");
            var worstBilingMovie = _db.Movies.OrderBy(x => x.Gross).ToList().FirstOrDefault();
            Console.WriteLine($"Filme: {worstBilingMovie.Title} | Faturamento: {worstBilingMovie.Gross}");
            var charactersWorstBilingMovie = _db.Characters.Where(x => x.MovieId == worstBilingMovie.MovieId).ToList();
            var actorsWorstBilingMovie = new List<Actor>();
            foreach (var c in charactersWorstBilingMovie)
            {
                actorsWorstBilingMovie.Add(_db.Actors.Where(x => x.ActorId == c.ActorId).ToList().FirstOrDefault());
            }
            foreach (var a in actorsWorstBilingMovie)
            {
                Console.WriteLine($"Ator: {a.Name}");
            }
            Console.WriteLine($"O filme com MELHOR faturamento não possui elenco na base de dados! (filme {_db.Movies.OrderByDescending(x => x.Gross).ToList().FirstOrDefault().Title})");

            Console.WriteLine();
            Console.WriteLine("9. Gerar um relatório de aniversariantes, agrupando os atores pelo mês de aniverário.");
            string[] months = new DateTimeFormatInfo().MonthNames;
            foreach (string month in months)
            {
                Console.WriteLine($"Mês: {month}");
                foreach (var actor in _db.Actors)
                {
                    var mfi = new DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(actor.DateBirth.Month).ToString();
                    if (strMonthName.ToLower() == month.ToLower())
                    {
                        Console.WriteLine($"Ator: {actor.Name} | Dt Nascimento: {actor.DateBirth}");
                    }
                }
            }

            Console.WriteLine("- - -   feito!  - - - ");
            Console.WriteLine();
        }

        private static int FindActor(List<ActorMovie> cs)
        {
            var newCs = cs.OrderBy(x => x.ActorId);
            var newCs2 = newCs.Select(x => x.ActorId).ToList();
            int major = 0;
            int cont = 1;
            int id = 0;
            int idFinal = 0;

            for (int i = 0; i< newCs2.Count; i++)
            {
                if (i==0)
                {
                    major = cont;
                    id = newCs2[i];
                }
                else
                {
                    if (id != newCs2[i])
                    {
                        id = newCs2[i];
                        cont =1;
                    }
                    else
                    {
                        cont++;
                    }
                }
                if (cont > major)
                {
                    major = cont;
                    idFinal = newCs2[i];
                }

            }

            return idFinal;
        }

        private static List<MovieDto> ExtractDataFromMovies(MovieContext db)
        {
            var query =
                from actor in db.Actors
                from movie in db.Movies.Include("Genre")
                from actorMovie in db.Characters.Where(c => c.ActorId == actor.ActorId && movie.MovieId == c.MovieId)
                select new
                {
                    actor,
                    movie,
                    actorMovie
                };

            var result = query.ToList();
            var json = JsonConvert.SerializeObject(result);
            return JsonConvert.DeserializeObject<List<MovieDto>>(json);
        }

        static void Main_presencial(String[] args)
        {
            // acesso ao EF serah realizado pela variavel _db
            // essa dependencia da camada de apresentacao com
            // a camada de persistencia eh errada!
            MovieContext _db = new MovieContext();

            #region # LINQ - consultas

            Console.WriteLine();
            Console.WriteLine("1. Todos os filmes de acao");

            Console.WriteLine("1a. Modelo tradicional");
            List<Movie> filmes1a = new List<Movie>();
            foreach (Movie f in _db.Movies.Include("Genre"))
            {
                if (f.Genre.Name == "Action")
                    filmes1a.Add(f);
            }

            foreach (Movie filme in filmes1a)
            {
                Console.WriteLine("\t{0} - {1}", filme.Title, filme.ReleaseDate.Year);
            }

            Console.WriteLine("\n1b. Usando linq - query syntax");
            var filmes1b = from f in _db.Movies
                          where f.Genre.Name == "Action"
                          select f;
            foreach (Movie filme in filmes1b)
            {
                Console.WriteLine("\t{0} - {1}", filme.Title, filme.Director);
            }

            Console.WriteLine("\n1c. Usando linq - method syntax");
            var filmes1c = _db.Movies.Where(m => m.Genre.Name == "Action");
            foreach (Movie filme in filmes1c)
            {
                Console.WriteLine("\t{0}", filme.Title);
            }

 
            Console.WriteLine();
            Console.WriteLine("2. Todos os diretores de filmes do genero 'Action', com projecao");
            var filmes2 = from f in _db.Movies
                          where f.Genre.Name == "Action"
                          select f.Director;

            foreach (var nome in filmes2)
            {
                Console.WriteLine("\t{0}", nome);
            }

            Console.WriteLine();
            Console.WriteLine("3a. Todos os filmes de cada genero (query syntax):");
            var generosFilmes3a = from g in _db.Genres.Include(gen => gen.Movies)
                                select g;
            foreach (var gf in generosFilmes3a)
            {
                if (gf.Movies.Count > 0)
                {
                    Console.WriteLine("\nFilmes do genero: " + gf.Name);
                    foreach (var f in gf.Movies)
                    {
                        Console.WriteLine("\t{0} - {1}", f.Title, f.Rating);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("3b. Todos os filmes de cada genero (method syntax):");

            var generosFilmes3b = _db.Genres.Include(gen => gen.Movies).ToList();

            foreach (Genre gf in generosFilmes3a)
            {
                if (gf.Movies.Count > 0)
                {
                    Console.WriteLine("\nFilmes do genero: " + gf.Name);
                    foreach (var f in gf.Movies)
                    {
                        Console.WriteLine("\t{0}", f.Title);
                    }
                }
            }


            Console.WriteLine();
            Console.WriteLine("4. Titulo e ano dos filmes do diretor Quentin Tarantino, com projcao em uma class anonima:");
            var tarantino = from f in _db.Movies
                            where f.Director == "Quentin Tarantino"
                             select new
                             {
                                 Ano = f.ReleaseDate.Year,
                                 f.Title
                             };

            foreach (var item in tarantino)
            {
                Console.WriteLine("{0} - {1}", item.Ano, item.Title);
            }

            Console.WriteLine();
            Console.WriteLine("5. Todos os gêneros ordenados pelo nome:");
            var q5 = _db.Genres.OrderByDescending(g => g.Name);
            foreach (var genero in q5)
            {
                Console.WriteLine("{0, 20}\t {1}", genero.Name, genero.Description.Substring(0, 30));
            }

            Console.WriteLine();
            Console.WriteLine("6. Numero de filmes agrupados pelo anos de lançamento:");
            var q6 = from f in _db.Movies
                     group f by f.ReleaseDate.Year into grupo
                     select new
                     {
                         Chave = grupo.Key,
                         NroFilmes = grupo.Count()
                     };

            foreach (var ano in q6.OrderByDescending(g => g.NroFilmes))
            {
                Console.WriteLine("Ano: {0}  Numero de filmes: {1}", ano.Chave, ano.NroFilmes);

            }

            Console.WriteLine();
            Console.WriteLine("7. Projeção do faturamento total, quantidade de filmes e avaliação média agrupadas por gênero:");
            var q7 = from f in _db.Movies
                     group f by f.Genre.Name into grpGen
                     select new
                     {
                         Categoria = grpGen.Key,
                         Faturamento = grpGen.Sum(e => e.Gross),
                         Avaliacao = grpGen.Average(e => e.Rating),
                         Quantidade = grpGen.Count()
                     };

            foreach (var genero in q7)
            {
                Console.WriteLine("Genero: {0}", genero.Categoria);
                Console.WriteLine("\tFaturamento total: {0}\n\t Avaliação média: {1}\n\tNumero de filmes: {2}",
                                genero.Faturamento, genero.Avaliacao, genero.Quantidade);
            }
            #endregion



        }

        static void Main_CRUd(string[] args)
        {
            Console.WriteLine("Hello World!");

            MovieContext _context = new MovieContext();

            Genre g1 = new Genre()
            {
                Name = "Comedia",
                Description = "Filmes de comedia"
            };

            Genre g2 = new Genre()
            {
                Name = "Ficcao",
                Description = "Filmes de ficcao"
            };

            _context.Genres.Add(g1);
            _context.Genres.Add(g2);

            _context.SaveChanges();

            List<Genre> genres = _context.Genres.ToList();

            foreach (Genre g in genres)
            {
                Console.WriteLine(String.Format("{0,2} {1,-10} {2}",
                                    g.GenreId, g.Name, g.Description));
            }

        }
    }
}
