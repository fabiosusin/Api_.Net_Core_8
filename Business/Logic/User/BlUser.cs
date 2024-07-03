using Business.Interfaces;
using DAO.Interfaces;
using DTO.General.Base.Output;
using DTO.Logic.User.Database;

using DTO.Logic.User.Input;

/// <summary>
/// Classe responsável pela lógica de negócios relacionada aos usuários do sistema.
/// Implementa operações como login, criação, atualização e exclusão de usuários,
/// bem como recuperação de informações de usuário.
/// </summary>
public class BlUser : IBlUser
{
    private readonly IUserDAO _userDAO;      // Acesso aos dados dos usuários
    private readonly IBlRating _blRating;    // Lógica de negócios para avaliações
    private readonly IRatingDAO _ratingDAO;  // Acesso aos dados de avaliações

    /// <summary>
    /// Construtor da classe BlUser.
    /// </summary>
    /// <param name="userDAO">Instância do DAO de usuário.</param>
    /// <param name="blRating">Instância da lógica de negócios de avaliações.</param>
    /// <param name="ratingDAO">Instância do DAO de avaliações.</param>
    public BlUser(
        IUserDAO userDAO,
        IBlRating blRating,
        IRatingDAO ratingDAO)
    {
        _userDAO = userDAO;
        _blRating = blRating;
        _ratingDAO = ratingDAO;
    }

    /// <summary>
    /// Método para realizar o login de um usuário no sistema.
    /// </summary>
    /// <param name="input">Dados de entrada para o login (email e senha).</param>
    /// <returns>Objeto BaseApiOutput indicando sucesso ou falha na operação.</returns>
    public BaseApiOutput Login(UserInput input)
    {
        if (input == null)
            return new BaseApiOutput("Dados não informados!");

        var existingUser = _userDAO.FindOne(x => x.Password == input.Password && x.Email == input.Email);
        return existingUser == null ? new BaseApiOutput("Usuário não encontrado!") : new BaseApiOutput(true);
    }

    /// <summary>
    /// Método para criar um novo usuário no sistema.
    /// </summary>
    /// <param name="user">Dados do usuário a ser criado.</param>
    /// <returns>Objeto BaseApiOutput indicando sucesso ou falha na operação.</returns>
    public BaseApiOutput CreateUser(UserInput user)
    {
        if (string.IsNullOrEmpty(user?.Email))
            return new BaseApiOutput("Email não informado!");

        if (string.IsNullOrEmpty(user.Name))
            return new BaseApiOutput("Nome não informado!");

        if (string.IsNullOrEmpty(user.Password))
            return new BaseApiOutput("Senha não informada!");

        var existingUser = _userDAO.FindOne(x => x.Email == user.Email);
        if (existingUser != null)
            return new BaseApiOutput("Email se encontra vinculado a outro usuário!");

        return _userDAO.Insert(new User(user));
    }

    /// <summary>
    /// Método para excluir um usuário do sistema.
    /// </summary>
    /// <param name="id">Identificador único do usuário a ser excluído.</param>
    /// <returns>Objeto BaseApiOutput indicando sucesso ou falha na operação.</returns>
    public BaseApiOutput DeleteUser(string id)
    {
        var existingUser = _userDAO.FindById(id);
        if (existingUser == null)
            return new BaseApiOutput("Usuário não encontrado!");

        var ratings = _ratingDAO.Find(x => x.UserId == id);
        if (ratings?.Any() ?? false)
        {
            foreach (var rating in ratings)
                _blRating.DeleteRating(id);
        }

        return _userDAO.RemoveById(id);
    }

    /// <summary>
    /// Método para obter informações detalhadas de um usuário pelo seu ID.
    /// </summary>
    /// <param name="id">Identificador único do usuário.</param>
    /// <returns>Objeto do tipo User contendo as informações do usuário.</returns>
    public User GetUser(string id) => _userDAO.FindById(id);

    /// <summary>
    /// Método para atualizar as informações de um usuário no sistema.
    /// </summary>
    /// <param name="id">Identificador único do usuário a ser atualizado.</param>
    /// <param name="user">Dados atualizados do usuário.</param>
    /// <returns>Objeto BaseApiOutput indicando sucesso ou falha na operação.</returns>
    public BaseApiOutput UpdateUser(string id, UserInput user)
    {
        if (string.IsNullOrEmpty(user?.Email))
            return new BaseApiOutput("Email não informado!");

        if (string.IsNullOrEmpty(user.Name))
            return new BaseApiOutput("Nome não informado!");

        var existingUser = _userDAO.FindOne(x => x.Id != id && x.Email == user.Email);
        if (existingUser != null)
            return new BaseApiOutput("Email se encontra vinculado a outro usuário!");

        return _userDAO.Update(new User(user)
        {
            Id = id
        });
    }
}
