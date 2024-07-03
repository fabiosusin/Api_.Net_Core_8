using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Genre.Database;

using DTO.Logic.Genre.Input;

/// <summary>
/// Classe de lógica de negócio para operações relacionadas a Gêneros de filmes.
/// </summary>
public class BlGenre : IBlGenre
{
    private readonly IGenreDAO _genreDAO;
    private readonly IMovieDAO _movieDAO;

    /// <summary>
    /// Construtor da classe BlGenre que recebe instâncias de DAOs necessários para operações de banco de dados.
    /// </summary>
    /// <param name="movieDAO">Instância do DAO para operações relacionadas a filmes.</param>
    /// <param name="genreDAO">Instância do DAO para operações relacionadas a gêneros.</param>
    public BlGenre(IMovieDAO movieDAO, IGenreDAO genreDAO)
    {
        _genreDAO = genreDAO;
        _movieDAO = movieDAO;
    }

    /// <summary>
    /// Cria um novo gênero de filme com o nome especificado.
    /// </summary>
    /// <param name="name">Nome do novo gênero.</param>
    /// <returns>Objeto BaseApiOutput indicando o resultado da operação.</returns>
    public BaseApiOutput CreateGenre(string name)
    {
        var existingName = _genreDAO.FindByName(name);
        if (existingName != null)
            return new BaseApiOutput("Já existe um Gênero com este nome!");

        return _genreDAO.Insert(new Genre(name));
    }

    /// <summary>
    /// Deleta um gênero de filme pelo ID.
    /// </summary>
    /// <param name="id">ID do gênero a ser deletado.</param>
    /// <returns>Objeto BaseApiOutput indicando o resultado da operação.</returns>
    public BaseApiOutput DeleteGenre(string id)
    {
        var genre = _genreDAO.FindById(id);
        if (genre == null)
            return new BaseApiOutput("Gênero não encontrado!");

        var movie = _movieDAO.FindOne(x => x.GenreId == id);
        if (movie != null)
            return new BaseApiOutput("Existem filmes vinculados a este Gênero!");

        return _genreDAO.RemoveById(id);
    }

    /// <summary>
    /// Retorna um gênero de filme pelo ID.
    /// </summary>
    /// <param name="id">ID do gênero a ser retornado.</param>
    /// <returns>Objeto do tipo Genre correspondente ao ID especificado.</returns>
    public Genre GetGenre(string id) => _genreDAO.FindById(id);

    /// <summary>
    /// Lista todos os gêneros de filmes de acordo com os critérios de entrada.
    /// </summary>
    /// <param name="input">Critérios de listagem de gêneros.</param>
    /// <returns>Objeto BaseListOutput contendo a lista de gêneros encontrados.</returns>
    public BaseListOutput<Genre> List(GenreListInput input)
    {
        var result = _genreDAO.List(input);
        if (!(result?.Items?.Any() ?? false))
            return new BaseListOutput<Genre>("Nenhum Gênero encontrado!");

        return result;
    }

    /// <summary>
    /// Atualiza o nome de um gênero de filme pelo ID.
    /// </summary>
    /// <param name="id">ID do gênero a ser atualizado.</param>
    /// <param name="name">Novo nome para o gênero.</param>
    /// <returns>Objeto BaseApiOutput indicando o resultado da operação.</returns>
    public BaseApiOutput UpdateGenre(string id, string name)
    {
        var genre = _genreDAO.FindById(id);
        if (genre == null)
            return new BaseApiOutput("Gênero não encontrado!");

        genre.Name = name;
        return _genreDAO.Update(genre);
    }
}
