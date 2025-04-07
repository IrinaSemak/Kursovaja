using System;
using System.Collections.Generic;
using System.Linq;

namespace WildfireClustering
{
    public class DamageStat
    {
        public string Damage { get; set; }
        public int Count { get; set; }
        public double AverageValue { get; set; }
        public List<StructureTypeStat> StructureTypes { get; set; }
    }

    public class StructureTypeStat
    {
        public string StructureType { get; set; }
        public int Count { get; set; }
    }

    public static class Calculations
    {
        public static List<DamageStat> AnalyzeDamageAndStructure(List<WildfireRecord> records)
        {
            var damageStats = records
                .GroupBy(r => r.Damage)
                .Select(g => new DamageStat
                {
                    Damage = g.Key,
                    Count = g.Count(),
                    AverageValue = g.Select(r => r.AssessedImprovedValue)
                                   .Where(v => v != "-" && double.TryParse(v, out _))
                                   .Select(v => double.Parse(v))
                                   .DefaultIfEmpty(0)
                                   .Average(),
                    StructureTypes = g.GroupBy(r => r.StructureType)
                                      .Select(sg => new StructureTypeStat
                                      {
                                          StructureType = sg.Key,
                                          Count = sg.Count()
                                      })
                                      .ToList()
                })
                .ToList();

            // Вывод в консоль
            Console.WriteLine("\nСтатистика по степени повреждения:");
            foreach (var stat in damageStats)
            {
                Console.WriteLine($"Степень повреждения: {stat.Damage}, Количество: {stat.Count}, Средняя стоимость недвижимости: {stat.AverageValue:F2}");
                Console.WriteLine("Распределение по типу строения:");
                foreach (var st in stat.StructureTypes)
                {
                    Console.WriteLine($"  Тип строения: {st.StructureType}, Количество: {st.Count}");
                }
            }

            return damageStats;
        }
    }
}