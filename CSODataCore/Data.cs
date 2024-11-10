namespace CSODataCore
{
    public sealed class ReinforceData
    {
        /// <summary>
        /// 伤害
        /// </summary>
        public byte? Damage { get; set; }
        /// <summary>
        /// 精确度
        /// </summary>
        public byte? Accuracy { get; set; }
        /// <summary>
        /// 后坐力
        /// </summary>
        public byte? KickBack { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public byte? Wegiht { get; set; }
        /// <summary>
        /// 射速
        /// </summary>
        public byte? FireRate { get; set; }
        /// <summary>
        /// 填弹数
        /// </summary>
        public byte? Ammo { get; set; }
        /// <summary>
        /// +9终极提升类型
        /// </summary>
        public byte? OverDmg { get; set; }
    }
    public sealed class ItemData(Item item, int slot)
    {
        public Item Item { get; set; } = item;
        public int Slot { get; set; } = slot;

        public int GetData { get; set; }
        public int DQData { get; set; }
        public ReinforceData? ReinforceData { get; set; }
        public Item? Paint { get; set; }
        public Item? Part1 { get; set; }
        public Item? Part2 { get; set; }
    }
}