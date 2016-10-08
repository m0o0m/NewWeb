using GL.Data.AWeb.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AWeb.Models
{

    public class SelectUserRolesViewModel
    {
        //private IQueryable<AgentRole> roles;

        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }


        // Enable initialization with an instance of ApplicationUser:
        public SelectUserRolesViewModel(AgentUser user, IQueryable<AgentRole> allRoles)
            : this()
        {
            this.UserName = user.UserName;

            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                this.Roles.Add(rvm);
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:
            foreach (var userRole in user.Roles)
            {
                var checkUserRole = this.Roles.Find(r => r.RoleID == userRole.RoleId);
                checkUserRole.Selected = true;
            }
        }

        public string UserName { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }
    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }

        public SelectRoleEditorViewModel(AgentRole role)
        {
            this.RoleName = role.Name;
            this.RoleID = role.Id;
            this.Description = role.Description;
        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }
        public int RoleID { get; set; }

        public string Description { get; set; }
    }


    public class RoleViewModel
    {
        [Display(Name = "IDs")]
        public int RoleID { get; set; }
        [Display(Name = "权限名")]
        [Required(ErrorMessage = "权限名不能为空")]
        [MaxLength(6)]
        public string RoleName { get; set; }
        [Display(Name = "描述")]
        [Required(ErrorMessage = "描述不能为空")]
        [MaxLength(100)]
        public string Description { get; set; }

        public RoleViewModel() { }
        public RoleViewModel(AgentRole role)
        {
            this.RoleID = role.Id;
            this.RoleName = role.Name;
            this.Description = role.Description;
        }
    }


    public class EditRoleViewModel
    {
        public int RoleID { get; set; }
        [Display(Name = "原权限名")]
        public string OriginalRoleName { get; set; }
        [Display(Name = "新权限名")]
        [Required(ErrorMessage = "新权限名不能为空")]
        [MaxLength(6)]
        public string RoleName { get; set; }
        [Display(Name = "描述")]
        [Required(ErrorMessage = "描述不能为空")]
        [MaxLength(100)]
        public string Description { get; set; }

        public EditRoleViewModel() { }
        public EditRoleViewModel(AgentRole role)
        {
            this.RoleID = role.Id;
            this.OriginalRoleName = role.Name;
            this.RoleName = role.Name;
            this.Description = role.Description;
        }
    }
}