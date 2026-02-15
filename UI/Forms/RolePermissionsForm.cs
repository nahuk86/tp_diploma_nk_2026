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

        public RolePermissionsForm(int roleId, string roleName, RoleService roleService)
        {
            InitializeComponent();
            
            _roleId = roleId;
            _roleName = roleName;
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            
            lblTitle.Text = $"Permisos para el rol: {_roleName}";
            
            LoadPermissions();
        }

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

            public override string ToString()
            {
                return DisplayText;
            }
        }
    }
}
