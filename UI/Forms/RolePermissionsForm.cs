using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL.Services;
using DOMAIN.Entities;

namespace UI.Forms
{
    public partial class RolePermissionsForm : Form
    {
        private readonly int _roleId;
        private readonly string _roleName;
        private readonly RoleService _roleService;
        private List<Permission> _allPermissions;
        private List<Permission> _rolePermissions;

        /// <summary>
        /// Inicializa una nueva instancia del formulario de asignación de permisos a roles
        /// </summary>
        /// <param name="roleId">Identificador único del rol</param>
        /// <param name="roleName">Nombre del rol</param>
        /// <param name="roleService">Servicio para gestionar roles y permisos</param>
        public RolePermissionsForm(int roleId, string roleName, RoleService roleService)
        {
            InitializeComponent();
            
            _roleId = roleId;
            _roleName = roleName;
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            
            lblTitle.Text = $"Permisos para el rol: {_roleName}";
            
            LoadPermissions();
        }

        /// <summary>
        /// Carga todos los permisos disponibles y marca los asignados al rol actual
        /// </summary>
        private void LoadPermissions()
        {
            try
            {
                // Get all available permissions
                _allPermissions = _roleService.GetAllPermissions();
                
                // Get permissions currently assigned to this role
                _rolePermissions = _roleService.GetRolePermissions(_roleId);
                
                // Populate the checked list box
                clbPermissions.Items.Clear();
                
                foreach (var permission in _allPermissions.OrderBy(p => p.Module).ThenBy(p => p.PermissionName))
                {
                    var displayText = $"[{permission.Module}] {permission.PermissionName}";
                    if (!string.IsNullOrWhiteSpace(permission.Description))
                    {
                        displayText += $" - {permission.Description}";
                    }
                    
                    var index = clbPermissions.Items.Add(new PermissionItem
                    {
                        Permission = permission,
                        DisplayText = displayText
                    });
                    
                    // Check if this permission is already assigned to the role
                    if (_rolePermissions.Any(rp => rp.PermissionId == permission.PermissionId))
                    {
                        clbPermissions.SetItemChecked(index, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar permisos: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Maneja el evento Click del botón Guardar para actualizar los permisos del rol
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedPermissionIds = new List<int>();
                
                foreach (var item in clbPermissions.CheckedItems)
                {
                    if (item is PermissionItem permissionItem)
                    {
                        selectedPermissionIds.Add(permissionItem.Permission.PermissionId);
                    }
                }
                
                _roleService.AssignPermissions(_roleId, selectedPermissionIds);
                
                MessageBox.Show(
                    "Permisos actualizados exitosamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al guardar permisos: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Maneja el evento Click del botón Cancelar para cerrar el formulario sin guardar cambios
        /// </summary>
        /// <param name="sender">Objeto que generó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Helper class to display permissions in the CheckedListBox
        private class PermissionItem
        {
            public Permission Permission { get; set; }
            public string DisplayText { get; set; }

            /// <summary>
            /// Devuelve la representación en cadena del permiso
            /// </summary>
            /// <returns>Texto para mostrar en el control</returns>
            public override string ToString()
            {
                return DisplayText;
            }
        }
    }
}
