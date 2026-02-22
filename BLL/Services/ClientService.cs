using System;
using System.Collections.Generic;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;
using SERVICES;
using SERVICES.Interfaces;

namespace BLL.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;

        /// <summary>
        /// Inicializa el servicio de clientes con sus dependencias
        /// </summary>
        /// <param name="clientRepo">Repositorio de clientes</param>
        /// <param name="auditRepo">Repositorio de auditoría</param>
        /// <param name="logService">Servicio de registro de eventos</param>
        public ClientService(IClientRepository clientRepo, IAuditLogRepository auditRepo, ILogService logService)
        {
            _clientRepo = clientRepo ?? throw new ArgumentNullException(nameof(clientRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Obtiene todos los clientes del sistema
        /// </summary>
        /// <returns>Lista de todos los clientes</returns>
        public List<Client> GetAllClients()
        {
            try
            {
                return _clientRepo.GetAll();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all clients", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los clientes activos del sistema
        /// </summary>
        /// <returns>Lista de clientes activos</returns>
        public List<Client> GetActiveClients()
        {
            try
            {
                return _clientRepo.GetAllActive();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving active clients", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un cliente por su identificador
        /// </summary>
        /// <param name="clientId">Identificador del cliente</param>
        /// <returns>Cliente encontrado o null si no existe</returns>
        public Client GetClientById(int clientId)
        {
            try
            {
                return _clientRepo.GetById(clientId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving client {clientId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo cliente en el sistema
        /// </summary>
        /// <param name="client">Datos del cliente a crear</param>
        /// <returns>Identificador del cliente creado</returns>
        public int CreateClient(Client client)
        {
            try
            {
                // Validations
                ValidateClient(client);

                // Check for duplicate DNI
                if (_clientRepo.DNIExists(client.DNI))
                {
                    throw new InvalidOperationException($"DNI '{client.DNI}' ya existe. Por favor use un DNI único.");
                }

                // Set audit fields
                client.CreatedAt = DateTime.Now;
                client.CreatedBy = SessionContext.Instance.CurrentUserId;
                client.IsActive = true;

                // Insert
                var clientId = _clientRepo.Insert(client);

                // Audit log
                _auditRepo.LogChange("Clients", clientId, AuditAction.Insert, null, null, 
                    $"Created client {client.Nombre} {client.Apellido} - DNI: {client.DNI}", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Client created: {client.Nombre} {client.Apellido} - DNI: {client.DNI} by {SessionContext.Instance.CurrentUsername}");

                return clientId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating client: {client.DNI}", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente
        /// </summary>
        /// <param name="client">Datos actualizados del cliente</param>
        public void UpdateClient(Client client)
        {
            try
            {
                // Validations
                ValidateClient(client);

                // Check for duplicate DNI (excluding current client)
                if (_clientRepo.DNIExists(client.DNI, client.ClientId))
                {
                    throw new InvalidOperationException($"DNI '{client.DNI}' ya existe. Por favor use un DNI único.");
                }

                // Get old values for audit
                var oldClient = _clientRepo.GetById(client.ClientId);
                if (oldClient == null)
                {
                    throw new InvalidOperationException($"Client with ID {client.ClientId} not found.");
                }

                // Set audit fields
                client.UpdatedAt = DateTime.Now;
                client.UpdatedBy = SessionContext.Instance.CurrentUserId;

                // Update
                _clientRepo.Update(client);

                // Audit log - log each changed field (DNI is not audited as it's read-only during edit)
                LogFieldChange("Clients", client.ClientId, "Nombre", oldClient.Nombre, client.Nombre);
                LogFieldChange("Clients", client.ClientId, "Apellido", oldClient.Apellido, client.Apellido);
                LogFieldChange("Clients", client.ClientId, "Correo", oldClient.Correo, client.Correo);
                LogFieldChange("Clients", client.ClientId, "Telefono", oldClient.Telefono, client.Telefono);
                LogFieldChange("Clients", client.ClientId, "Direccion", oldClient.Direccion, client.Direccion);

                _logService.Info($"Client updated: {client.Nombre} {client.Apellido} - DNI: {client.DNI} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating client: {client.ClientId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un cliente del sistema (borrado lógico)
        /// </summary>
        /// <param name="clientId">Identificador del cliente a eliminar</param>
        public void DeleteClient(int clientId)
        {
            try
            {
                var client = _clientRepo.GetById(clientId);
                if (client == null)
                {
                    throw new InvalidOperationException($"Client with ID {clientId} not found.");
                }

                // Soft delete
                _clientRepo.SoftDelete(clientId, SessionContext.Instance.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Clients", clientId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Client deleted (soft): {client.Nombre} {client.Apellido} - DNI: {client.DNI} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting client: {clientId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Valida que los datos del cliente cumplan con las reglas de negocio
        /// </summary>
        /// <param name="client">Cliente a validar</param>
        private void ValidateClient(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client), "Client cannot be null.");

            if (string.IsNullOrWhiteSpace(client.Nombre))
                throw new ArgumentException("Nombre is required.", nameof(client.Nombre));

            if (client.Nombre.Length > 100)
                throw new ArgumentException("Nombre must be 100 characters or less.", nameof(client.Nombre));

            if (string.IsNullOrWhiteSpace(client.Apellido))
                throw new ArgumentException("Apellido is required.", nameof(client.Apellido));

            if (client.Apellido.Length > 100)
                throw new ArgumentException("Apellido must be 100 characters or less.", nameof(client.Apellido));

            if (string.IsNullOrWhiteSpace(client.DNI))
                throw new ArgumentException("DNI is required.", nameof(client.DNI));

            if (client.DNI.Length > 20)
                throw new ArgumentException("DNI must be 20 characters or less.", nameof(client.DNI));

            if (!string.IsNullOrWhiteSpace(client.Correo) && client.Correo.Length > 100)
                throw new ArgumentException("Correo must be 100 characters or less.", nameof(client.Correo));

            if (!string.IsNullOrWhiteSpace(client.Telefono) && client.Telefono.Length > 20)
                throw new ArgumentException("Telefono must be 20 characters or less.", nameof(client.Telefono));

            if (!string.IsNullOrWhiteSpace(client.Direccion) && client.Direccion.Length > 200)
                throw new ArgumentException("Direccion must be 200 characters or less.", nameof(client.Direccion));
        }

        /// <summary>
        /// Registra un cambio de campo en la auditoría si el valor ha cambiado
        /// </summary>
        /// <param name="tableName">Nombre de la tabla</param>
        /// <param name="recordId">Identificador del registro</param>
        /// <param name="fieldName">Nombre del campo</param>
        /// <param name="oldValue">Valor anterior</param>
        /// <param name="newValue">Valor nuevo</param>
        private void LogFieldChange(string tableName, int recordId, string fieldName, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                _auditRepo.LogChange(tableName, recordId, AuditAction.Update, fieldName, oldValue, newValue, SessionContext.Instance.CurrentUserId);
            }
        }
    }
}
