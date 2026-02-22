using DOMAIN.Entities;

namespace SERVICES
{
    /// <summary>
    /// Contexto global de sesión para mantener información del usuario autenticado.
    /// Implementa el patrón Singleton para garantizar una única instancia durante el ciclo de vida de la aplicación.
    /// </summary>
    public sealed class SessionContext
    {
        // Patrón Singleton: instancia única inicializada de forma lazy y thread-safe
        private static readonly SessionContext _instance = new SessionContext();

        /// <summary>
        /// Obtiene la instancia única del contexto de sesión (patrón Singleton)
        /// </summary>
        public static SessionContext Instance => _instance;

        /// <summary>
        /// Constructor privado para impedir la instanciación externa (patrón Singleton)
        /// </summary>
        private SessionContext() { }

        private User _currentUser;

        /// <summary>
        /// Obtiene o establece el usuario actualmente autenticado en la sesión
        /// </summary>
        public User CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser = value; }
        }

        /// <summary>
        /// Obtiene el ID del usuario actual si existe una sesión activa
        /// </summary>
        public int? CurrentUserId
        {
            get { return _currentUser?.UserId; }
        }

        /// <summary>
        /// Obtiene el nombre de usuario del usuario actual si existe una sesión activa
        /// </summary>
        public string CurrentUsername
        {
            get { return _currentUser?.Username; }
        }

        /// <summary>
        /// Limpia la sesión actual eliminando la información del usuario autenticado
        /// </summary>
        public void Clear()
        {
            _currentUser = null;
        }
    }
}
