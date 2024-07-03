using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.Streaming.Database;

using DTO.Logic.Streaming.Input;

/// <summary>
/// Classe responsável pela lógica de negócio relacionada aos Streamings.
/// </summary>
public class BlStreaming : IBlStreaming
{
    private readonly IStreamingDAO _streamingDAO;
    private readonly IMovieDAO _movieDAO;

    /// <summary>
    /// Construtor da classe BlStreaming.
    /// </summary>
    /// <param name="movieDAO">Instância do DAO de filmes.</param>
    /// <param name="streamingDAO">Instância do DAO de streamings.</param>
    public BlStreaming(
        IMovieDAO movieDAO,
        IStreamingDAO streamingDAO)
    {
        _streamingDAO = streamingDAO;
        _movieDAO = movieDAO;
    }

    /// <summary>
    /// Cria um novo Streaming com o nome especificado.
    /// </summary>
    /// <param name="name">Nome do Streaming a ser criado.</param>
    /// <returns>Objeto BaseApiOutput com o resultado da operação.</returns>
    public BaseApiOutput CreateStreaming(string name)
    {
        var existingName = _streamingDAO.FindByName(name);
        if (existingName != null)
            return new BaseApiOutput("Já existe um Streaming com este nome!");

        return _streamingDAO.Insert(new Streaming(name));
    }

    /// <summary>
    /// Remove um Streaming com o ID especificado, verificando se há filmes vinculados a ele.
    /// </summary>
    /// <param name="id">ID do Streaming a ser removido.</param>
    /// <returns>Objeto BaseApiOutput com o resultado da operação.</returns>
    public BaseApiOutput DeleteStreaming(string id)
    {
        var streaming = _streamingDAO.FindById(id);
        if (streaming == null)
            return new BaseApiOutput("Streaming não encontrado!");

        var movie = _movieDAO.FindOne(x => x.StreamingsId.Contains(id));
        if (movie != null)
            return new BaseApiOutput("Existem filmes vinculados a este Streaming!");

        return _streamingDAO.RemoveById(id);
    }

    /// <summary>
    /// Obtém um Streaming pelo ID especificado.
    /// </summary>
    /// <param name="id">ID do Streaming a ser obtido.</param>
    /// <returns>Objeto do tipo Streaming encontrado ou null se não existir.</returns>
    public Streaming GetStreaming(string id) => _streamingDAO.FindById(id);

    /// <summary>
    /// Lista os Streamings de acordo com os critérios especificados em input.
    /// </summary>
    /// <param name="input">Objeto de entrada para filtro e paginação.</param>
    /// <returns>Objeto BaseListOutput contendo a lista de Streamings encontrados.</returns>
    public BaseListOutput<Streaming> List(StreamingListInput input)
    {
        var result = _streamingDAO.List(input);
        if (!(result?.Items?.Any() ?? false))
            return new BaseListOutput<Streaming>("Nenhum Streaming encontrado!");

        return result;
    }

    /// <summary>
    /// Atualiza o nome de um Streaming com o ID especificado.
    /// </summary>
    /// <param name="id">ID do Streaming a ser atualizado.</param>
    /// <param name="name">Novo nome para o Streaming.</param>
    /// <returns>Objeto BaseApiOutput com o resultado da operação.</returns>
    public BaseApiOutput UpdateStreaming(string id, string name)
    {
        var streaming = _streamingDAO.FindById(id);
        if (streaming == null)
            return new BaseApiOutput("Streaming não encontrado!");

        streaming.Name = name;
        return _streamingDAO.Update(streaming);
    }
}
