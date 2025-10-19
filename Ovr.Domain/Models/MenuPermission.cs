namespace Ovr.Domain.Models
{
    public class MenuPermission
    {
        public int MenuPermissionId { get; set; } // Identificador único del permiso (cuando aplica por usuario)

        // Identificadores relacionales
        public long UserId { get; set; } // ID del usuario (si es permiso por usuario)
        public int RoleId { get; set; } // ID del rol (si es permiso por rol)
        public int MenuId { get; set; } // ID del menú asociado

        public string MenusName { get; set; } = string.Empty; // Nombre del menú

        // Permisos específicos
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanExport { get; set; }
        public bool CanPrint { get; set; }
        public bool CanApprove { get; set; }
        public bool CanCancel { get; set; }
        public bool CanAuthorize { get; set; }

        public bool IsActive { get; set; } // Estado del permiso
    }
}
