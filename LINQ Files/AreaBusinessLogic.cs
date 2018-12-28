using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RunescapeMVC_BLL
{
    public class AreaBusinessLogic
    {
        //method to group beast and area together and calculate how many beasts are in each area
        public List<IAreaInfoBO> CalculateBeastsInArea(List<IBeastBO> iBeast, List<IAreaInfoBO> iArea)
        {
            var grouping = from x in iBeast
                           group x by x.AreaID into y
                           let count = y.Count()
                           select new { Value = y.Key, Count = count };

            //find where they match and count occurrences 
            foreach (IAreaInfoBO area in iArea)
            {
                if (grouping.FirstOrDefault(m => m.Value == area.AreaID) != null)
                {
                    area.BeastsInArea = grouping.FirstOrDefault(m => m.Value == area.AreaID).Count;
                }
                else { }
            }
            return iArea;
        }
    }
}
