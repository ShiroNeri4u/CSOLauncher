﻿namespace CSODataCore
{
    public sealed class ReinforceData
    {
        /// <summary>
        /// 伤害
        /// </summary>
        public byte Damage { get; set; }
        /// <summary>
        /// 命中率
        /// </summary>
        public byte Accuracy { get; set; }
        /// <summary>
        /// 后坐力
        /// </summary>
        public byte Rebound { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public byte Weight { get; set; }
        /// <summary>
        /// 连射性
        /// </summary>
        public byte Repeatedly { get; set; }
        /// <summary>
        /// 填弹数
        /// </summary>
        public byte Ammo { get; set; }
        /// <summary>
        /// 追加
        /// </summary>
        public byte OverDmg { get; set; }

    }
    public sealed class ItemData
    {
        public Item Item { get; set; }
        public int Slot { get; set; }
        public int GetData { get; set; }
        public int DQData { get; set; }
        public ReinforceData? ReinforceData { get; set; }
        public Item Paint { get; set; }
        public Item Part1 { get; set; }
        public Item Part2 { get; set; }

        public ItemData(Item item, int slot, Item paint, Item part1, Item part2)
        {
            Item = item;
            Slot = slot;
            Paint = paint;
            Part1 = part1;
            Part2 = part2;
        }
    }
}