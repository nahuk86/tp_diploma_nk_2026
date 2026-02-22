using System;
using System.Collections.Generic;
using DOMAIN.Contracts;
using DOMAIN.Entities;
using DOMAIN.Enums;
using SERVICES;
using SERVICES.Interfaces;

namespace BLL.Services
{
    public class WarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepo;
        private readonly IAuditLogRepository _auditRepo;
        private readonly ILogService _logService;

        /// <summary>
        /// Inicializa el servicio de almacenes con sus dependencias
        /// </summary>
        /// <param name="warehouseRepo">Repositorio de almacenes</param>
        /// <param name="auditRepo">Repositorio de auditoría</param>
        /// <param name="logService">Servicio de registro de eventos</param>
        public WarehouseService(IWarehouseRepository warehouseRepo, IAuditLogRepository auditRepo, ILogService logService)
        {
            _warehouseRepo = warehouseRepo ?? throw new ArgumentNullException(nameof(warehouseRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        /// <summary>
        /// Obtiene todos los almacenes del sistema
        /// </summary>
        /// <returns>Lista de todos los almacenes</returns>
        public List<Warehouse> GetAllWarehouses()
        {
            try
            {
                return _warehouseRepo.GetAll();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving all warehouses", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene todos los almacenes activos del sistema
        /// </summary>
        /// <returns>Lista de almacenes activos</returns>
        public List<Warehouse> GetActiveWarehouses()
        {
            try
            {
                return _warehouseRepo.GetAllActive();
            }
            catch (Exception ex)
            {
                _logService.Error("Error retrieving active warehouses", ex);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un almacén por su identificador
        /// </summary>
        /// <param name="warehouseId">Identificador del almacén</param>
        /// <returns>Almacén encontrado o null si no existe</returns>
        public Warehouse GetWarehouseById(int warehouseId)
        {
            try
            {
                return _warehouseRepo.GetById(warehouseId);
            }
            catch (Exception ex)
            {
                _logService.Error($"Error retrieving warehouse {warehouseId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Crea un nuevo almacén en el sistema
        /// </summary>
        /// <param name="warehouse">Datos del almacén a crear</param>
        /// <returns>Identificador del almacén creado</returns>
        public int CreateWarehouse(Warehouse warehouse)
        {
            try
            {
                // Validations
                ValidateWarehouse(warehouse);

                // Check for duplicate code
                if (_warehouseRepo.CodeExists(warehouse.Code))
                {
                    throw new InvalidOperationException($"Code '{warehouse.Code}' already exists. Please use a unique code.");
                }

                // Set audit fields
                warehouse.CreatedAt = DateTime.Now;
                warehouse.CreatedBy = SessionContext.Instance.CurrentUserId;
                warehouse.IsActive = true;

                // Insert
                var warehouseId = _warehouseRepo.Insert(warehouse);

                // Audit log
                _auditRepo.LogChange("Warehouses", warehouseId, AuditAction.Insert, null, null, 
                    $"Created warehouse {warehouse.Code} - {warehouse.Name}", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Warehouse created: {warehouse.Code} - {warehouse.Name} by {SessionContext.Instance.CurrentUsername}");

                return warehouseId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating warehouse: {warehouse.Code}", ex);
                throw;
            }
        }

        /// <summary>
        /// Actualiza los datos de un almacén existente
        /// </summary>
        /// <param name="warehouse">Datos actualizados del almacén</param>
        public void UpdateWarehouse(Warehouse warehouse)
        {
            try
            {
                // Validations
                ValidateWarehouse(warehouse);

                // Check for duplicate code (excluding current warehouse)
                if (_warehouseRepo.CodeExists(warehouse.Code, warehouse.WarehouseId))
                {
                    throw new InvalidOperationException($"Code '{warehouse.Code}' already exists. Please use a unique code.");
                }

                // Get old values for audit
                var oldWarehouse = _warehouseRepo.GetById(warehouse.WarehouseId);
                if (oldWarehouse == null)
                {
                    throw new InvalidOperationException($"Warehouse with ID {warehouse.WarehouseId} not found.");
                }

                // Set audit fields
                warehouse.UpdatedAt = DateTime.Now;
                warehouse.UpdatedBy = SessionContext.Instance.CurrentUserId;

                // Update
                _warehouseRepo.Update(warehouse);

                // Audit log - log each changed field
                LogFieldChange("Warehouses", warehouse.WarehouseId, "Name", oldWarehouse.Name, warehouse.Name);
                LogFieldChange("Warehouses", warehouse.WarehouseId, "Address", oldWarehouse.Address, warehouse.Address);

                _logService.Info($"Warehouse updated: {warehouse.Code} - {warehouse.Name} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating warehouse: {warehouse.WarehouseId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina un almacén del sistema (borrado lógico)
        /// </summary>
        /// <param name="warehouseId">Identificador del almacén a eliminar</param>
        public void DeleteWarehouse(int warehouseId)
        {
            try
            {
                var warehouse = _warehouseRepo.GetById(warehouseId);
                if (warehouse == null)
                {
                    throw new InvalidOperationException($"Warehouse with ID {warehouseId} not found.");
                }

                // Soft delete
                _warehouseRepo.SoftDelete(warehouseId, SessionContext.Instance.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Warehouses", warehouseId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.Instance.CurrentUserId);

                _logService.Info($"Warehouse deleted (soft): {warehouse.Code} - {warehouse.Name} by {SessionContext.Instance.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting warehouse: {warehouseId}", ex);
                throw;
            }
        }

        /// <summary>
        /// Valida que los datos del almacén cumplan con las reglas de negocio
        /// </summary>
        /// <param name="warehouse">Almacén a validar</param>
        private void ValidateWarehouse(Warehouse warehouse)
        {
            if (warehouse == null)
                throw new ArgumentNullException(nameof(warehouse), "Warehouse cannot be null.");

            if (string.IsNullOrWhiteSpace(warehouse.Code))
                throw new ArgumentException("Code is required.", nameof(warehouse.Code));

            if (warehouse.Code.Length > 20)
                throw new ArgumentException("Code must be 20 characters or less.", nameof(warehouse.Code));

            if (string.IsNullOrWhiteSpace(warehouse.Name))
                throw new ArgumentException("Name is required.", nameof(warehouse.Name));

            if (warehouse.Name.Length > 100)
                throw new ArgumentException("Name must be 100 characters or less.", nameof(warehouse.Name));
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
