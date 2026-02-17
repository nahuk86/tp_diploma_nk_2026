using DOMAIN.Entities;

namespace SERVICES
{
    /// <summary>
    /// Contexto global de sesión para mantener información del usuario autenticado
    /// </summary>
    public static class SessionContext
    {
        private static User _currentUser;

        /// <summary>
        /// Obtiene o establece el usuario actualmente autenticado en la sesión
        /// </summary>
        public static User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        /// <summary>
        /// Obtiene el ID del usuario actual si existe una sesión activa
        /// </summary>
        /// <returns>ID del usuario o null si no hay sesión activa</returns>
        public static int? CurrentUserId
        {
            get { return _currentUser?.UserId; }
        }

        /// <summary>
        /// Obtiene el nombre de usuario del usuario actual si existe una sesión activa
        /// </summary>
        /// <returns>Nombre de usuario o null si no hay sesión activa</returns>
        public static string CurrentUsername
        {
            get { return _currentUser?.Username; }
        }

        /// <summary>
        /// Limpia la sesión actual eliminando la información del usuario autenticado
        /// </summary>
        public static void Clear()
        {
            _currentUser = null;
        }
    }
}
