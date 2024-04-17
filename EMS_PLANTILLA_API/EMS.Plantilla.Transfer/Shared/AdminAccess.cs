namespace EMS.Plantilla.Transfer.Shared
{
    public class AdminAccess
    {
        public int CurrentUserOrgGroupID { get; set; }

        public string OrgGroupDescendantsDelimited { get; set; }

        public bool IsAdminAccess { get; set; }
    }
}