namespace iMARSARLIMS.Request_Model
{
    public class RoleMenuAccessRequestModel
    {
        public int? roleId { get; set; }
        public int? menuId { get; set; }
        public string? subMenuId { get; set; }
        public string? employeeId { get; set; }
        public bool isActive { get; set; } = true;
    }
}
