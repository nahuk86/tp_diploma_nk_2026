using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL.Services;
using DOMAIN.Entities;

namespace UI.Forms
{
    public partial class UserRolesForm : Form
    {
        private readonly int _userId;
        private readonly string _username;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private List<Role> _allRoles;
        private List<Role> _userRoles;

        public UserRolesForm(int userId, string username, UserService userService, RoleService roleService)
        {
            InitializeComponent();
            
            _userId = userId;
            _username = username;
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            
            lblTitle.Text = $"Roles para el usuario: {_username}";
            
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                // Get all available roles
                _allRoles = _roleService.GetActiveRoles();
                
                // Get roles currently assigned to this user
                _userRoles = _userService.GetUserRoles(_userId);
                
                // Populate the checked list box
                clbRoles.Items.Clear();
                
                foreach (var role in _allRoles.OrderBy(r => r.RoleName))
                {
                    var displayText = role.RoleName;
                    if (!string.IsNullOrWhiteSpace(role.Description))
                    {
                        displayText += $" - {role.Description}";
                    }
                    
                    var index = clbRoles.Items.Add(new RoleItem
                    {
                        Role = role,
                        DisplayText = displayText
                    });
                    
                    // Check if this role is already assigned to the user
                    if (_userRoles.Any(ur => ur.RoleId == role.RoleId))
                    {
                        clbRoles.SetItemChecked(index, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al cargar roles: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRoleIds = new List<int>();
                
                foreach (var item in clbRoles.CheckedItems)
                {
                    if (item is RoleItem roleItem)
                    {
                        selectedRoleIds.Add(roleItem.Role.RoleId);
                    }
                }
                
                _userService.AssignRolesToUser(_userId, selectedRoleIds);
                
                MessageBox.Show(
                    "Roles actualizados exitosamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al guardar roles: {ex.Message}",
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

        // Helper class to display roles in the CheckedListBox
        private class RoleItem
        {
            public Role Role { get; set; }
            public string DisplayText { get; set; }

            public override string ToString()
            {
                return DisplayText;
            }
        }
    }
}
