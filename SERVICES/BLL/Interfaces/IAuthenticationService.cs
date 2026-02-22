using DOMAIN.Entities;

namespace SERVICES.Interfaces
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Autentica un usuario verificando sus credenciales contra la base de datos
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña en texto plano</param>
        /// <returns>Objeto User si la autenticación es exitosa, null en caso contrario</returns>
        User Authenticate(string username, string password);
        
        /// <summary>
        /// Genera un hash seguro de la contraseña utilizando PBKDF2 con un salt aleatorio
        /// </summary>
        /// <param name="password">Contraseña en texto plano</param>
        /// <param name="salt">Parámetro de salida que contiene el salt generado en Base64</param>
        /// <returns>Hash de la contraseña en formato Base64</returns>
        string HashPassword(string password, out string salt);
        
        /// <summary>
        /// Verifica si una contraseña coincide con un hash almacenado
        /// </summary>
        /// <param name="password">Contraseña en texto plano a verificar</param>
        /// <param name="hash">Hash almacenado en Base64</param>
        /// <param name="salt">Salt utilizado en el hash original en Base64</param>
        /// <returns>True si la contraseña es correcta, false en caso contrario</returns>
        bool VerifyPassword(string password, string hash, string salt);
        
        /// <summary>
        /// Inicializa o actualiza la contraseña de un usuario administrativo
        /// </summary>
        /// <param name="username">Nombre del usuario</param>
        /// <param name="newPassword">Nueva contraseña en texto plano</param>
        void InitializeAdminPassword(string username, string newPassword);
    }
}
