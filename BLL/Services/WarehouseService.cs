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

        public WarehouseService(IWarehouseRepository warehouseRepo, IAuditLogRepository auditRepo, ILogService logService)
        {
            _warehouseRepo = warehouseRepo ?? throw new ArgumentNullException(nameof(warehouseRepo));
            _auditRepo = auditRepo ?? throw new ArgumentNullException(nameof(auditRepo));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

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
                warehouse.CreatedBy = SessionContext.CurrentUserId;
                warehouse.IsActive = true;

                // Insert
                var warehouseId = _warehouseRepo.Insert(warehouse);

                // Audit log
                _auditRepo.LogChange("Warehouses", warehouseId, AuditAction.Insert, null, null, 
                    $"Created warehouse {warehouse.Code} - {warehouse.Name}", SessionContext.CurrentUserId);

                _logService.Info($"Warehouse created: {warehouse.Code} - {warehouse.Name} by {SessionContext.CurrentUsername}");

                return warehouseId;
            }
            catch (Exception ex)
            {
                _logService.Error($"Error creating warehouse: {warehouse.Code}", ex);
                throw;
            }
        }

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
                warehouse.UpdatedBy = SessionContext.CurrentUserId;

                // Update
                _warehouseRepo.Update(warehouse);

                // Audit log - log each changed field
                LogFieldChange("Warehouses", warehouse.WarehouseId, "Name", oldWarehouse.Name, warehouse.Name);
                LogFieldChange("Warehouses", warehouse.WarehouseId, "Address", oldWarehouse.Address, warehouse.Address);

                _logService.Info($"Warehouse updated: {warehouse.Code} - {warehouse.Name} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error updating warehouse: {warehouse.WarehouseId}", ex);
                throw;
            }
        }

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
                _warehouseRepo.SoftDelete(warehouseId, SessionContext.CurrentUserId.Value);

                // Audit log
                _auditRepo.LogChange("Warehouses", warehouseId, AuditAction.Delete, "IsActive", "1", "0", SessionContext.CurrentUserId);

                _logService.Info($"Warehouse deleted (soft): {warehouse.Code} - {warehouse.Name} by {SessionContext.CurrentUsername}");
            }
            catch (Exception ex)
            {
                _logService.Error($"Error deleting warehouse: {warehouseId}", ex);
                throw;
            }
        }

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

        private void LogFieldChange(string tableName, int recordId, string fieldName, string oldValue, string newValue)
        {
            if (oldValue != newValue)
            {
                _auditRepo.LogChange(tableName, recordId, AuditAction.Update, fieldName, oldValue, newValue, SessionContext.CurrentUserId);
            }
        }
    }
}
