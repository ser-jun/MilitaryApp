namespace MilitaryApp.DTO
{
    public class MilitaryStructureItem
    {
        public int? ArmyId { get; set; }
        public string? ArmyName { get; set; } = null!;
        public int? DivisionId { get; set; }
        public string? DivisionName { get; set; } = null!;
        public int? CorpsId { get; set; }
        public string? CorpsName { get; set; } = null!;
        public int? UnitId { get; set; }
        public string? UnitName { get; set; } = null!;
        public int? SubUnitId { get; set; }
        public string? SubUnitName { get; set; }=null!;
        public string Name => ArmyName ?? DivisionName ?? CorpsName ?? UnitName ?? SubUnitName;
    }
}
